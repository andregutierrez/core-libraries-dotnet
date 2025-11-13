using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.LibrariesApplication.Commands;

namespace Core.LibrariesApplication.Behavior;

public class OutboxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IEventCollector _eventCollector;
    private readonly IOutboxStorage _outboxStorage;
    private readonly ILogger<OutboxBehavior<TRequest, TResponse>> _logger;

    public OutboxBehavior(
        IEventCollector eventCollector,
        IOutboxStorage outboxStorage,
        ILogger<OutboxBehavior<TRequest, TResponse>> logger)
    {
        _eventCollector = eventCollector;
        _outboxStorage = outboxStorage;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        var events = _eventCollector.CollectUnpublishedEvents();
        if (events.Count == 0)
            return response;

        var outboxMessages = events.Select(@event => new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = @event.GetType().FullName!,
            Payload = SafeSerialize(@event),
            OccurredAtUtc = DateTime.UtcNow
        });

        await _outboxStorage.SaveAsync(outboxMessages, cancellationToken);
        _eventCollector.ClearCollectedEvents();

        return response;
    }

    private static string SafeSerialize(object @event)
    {
        return JsonSerializer.Serialize(@event, new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EventType { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;
    public bool Published { get; set; } = false;
    public DateTime? PublishedAtUtc { get; set; }
}

public interface IEventCollector
{
    IReadOnlyCollection<object> CollectUnpublishedEvents();
    void ClearCollectedEvents();
}

public interface IOutboxStorage
{
    Task SaveAsync(IEnumerable<OutboxMessage> messages, CancellationToken cancellationToken);
}



/*
 
O OutboxDispatcherWorker é um serviço de background (BackgroundService) que roda de forma contínua (ou por agendamento) e:

Lê eventos não publicados do storage (Published == false)

Publica os eventos para Kafka, SQS, RabbitMQ, etc.

Marca os eventos como publicados (Published = true, PublishedAtUtc = now)

✅ 1. Interface para leitura e marcação de mensagens
csharp
Copiar
Editar
public interface IOutboxReader
{
    Task<List<OutboxMessage>> GetUnpublishedAsync(int maxBatchSize, CancellationToken cancellationToken);
    Task MarkAsPublishedAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}
✅ 2. Interface para o publisher real (ex: Kafka/SQS)
csharp
Copiar
Editar
public interface IDomainEventPublisher
{
    Task PublishAsync(string eventType, string payload, CancellationToken cancellationToken);
}
✅ 3. Worker OutboxDispatcherWorker
csharp
Copiar
Editar
public class OutboxDispatcherWorker : BackgroundService
{
    private readonly IOutboxReader _reader;
    private readonly IDomainEventPublisher _publisher;
    private readonly ILogger<OutboxDispatcherWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10); // ou cron/externo

    public OutboxDispatcherWorker(
        IOutboxReader reader,
        IDomainEventPublisher publisher,
        ILogger<OutboxDispatcherWorker> logger)
    {
        _reader = reader;
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var messages = await _reader.GetUnpublishedAsync(50, stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        await _publisher.PublishAsync(message.EventType, message.Payload, stoppingToken);
                        _logger.LogInformation("Evento publicado: {EventType} ({Id})", message.EventType, message.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao publicar evento {Id}", message.Id);
                    }
                }

                await _reader.MarkAsPublishedAsync(messages.Select(x => x.Id), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no dispatcher do Outbox");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
✅ 4. Exemplo de implementação do IOutboxReader com EF Core
csharp
Copiar
Editar
public class EfCoreOutboxReader : IOutboxReader
{
    private readonly DbContext _db;

    public EfCoreOutboxReader(DbContext db)
    {
        _db = db;
    }

    public async Task<List<OutboxMessage>> GetUnpublishedAsync(int maxBatchSize, CancellationToken cancellationToken)
    {
        return await _db.Set<OutboxMessage>()
            .Where(o => !o.Published)
            .OrderBy(o => o.OccurredAtUtc)
            .Take(maxBatchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsPublishedAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var messages = await _db.Set<OutboxMessage>()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var msg in messages)
        {
            msg.Published = true;
            msg.PublishedAtUtc = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
✅ Registro no Program.cs
csharp
Copiar
Editar
services.AddScoped<IOutboxReader, EfCoreOutboxReader>();
services.AddHostedService<OutboxDispatcherWorker>();
🧪 Exemplo de IDomainEventPublisher para Kafka
csharp
Copiar
Editar
public class KafkaDomainEventPublisher : IDomainEventPublisher
{
    private readonly IKafkaProducer _kafka;

    public KafkaDomainEventPublisher(IKafkaProducer kafka)
    {
        _kafka = kafka;
    }

    public Task PublishAsync(string eventType, string payload, CancellationToken cancellationToken)
    {
        var topic = $"event.{eventType}";
        return _kafka.ProduceAsync(topic, payload, cancellationToken);
    }
}
✅ Com isso você tem:
Componente	Responsabilidade
OutboxBehavior	Armazena eventos em OutboxMessage
OutboxReader	Lê e marca eventos como publicados
DomainEventPublisher	Publica para Kafka/SQS/Bus
Worker	Coordena leitura, publicação e marcação

Se quiser posso gerar a versão com lock pessimista (e.g., SKIP LOCKED com PostgreSQL ou FOR UPDATE) para escalabilidade horizontal. Deseja essa variante?









Perguntar ao ChatGPT


 
 */