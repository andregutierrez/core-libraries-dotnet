using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Core.Libraries.Domain.Entities.DomainEvents;
using Core.Libraries.Domain.Services.DomainEvents;

namespace Core.Libraries.Infra.Data.Context;

/// <summary>
/// Provides a base Entity Framework Core <see cref="DbContext"/> implementation
/// that supports domain event publishing via <see cref="IMediator"/> after persistence.
/// </summary>
/// <remarks>
/// This context tracks entities implementing <see cref="IHasDomainEvents"/> and
/// publishes their domain events after a successful save.
/// </remarks>
public abstract class CoreDbContext : DbContext
{
    private readonly ILogger<CoreDbContext> _logger;
    private readonly IMediator? _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoreDbContext"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for diagnostic output.</param>
    /// <param name="options">The database context options.</param>
    /// <param name="mediator">Optional <see cref="IMediator"/> instance used to publish domain events.</param>
    protected CoreDbContext(ILogger<CoreDbContext> logger, DbContextOptions options, IMediator? mediator)
        : base(options)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Saves all changes to the database and publishes any domain events captured during the unit of work.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">Whether to accept all changes if the operation succeeds.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        try
        {
            var inicio = DateTime.Now;
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            var termino = DateTime.Now;

            _logger.LogDebug("Tempo de {tempo} para a execução do SaveChanges", termino - inicio);

            var eventReport = CreateEventReport();
            await PublishEntityEvents(eventReport);

            return result;
        }
        finally
        {
            ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    /// <summary>
    /// Extracts and clears all domain events from tracked entities.
    /// </summary>
    /// <returns>A service containing a list of collected domain events.</returns>
    protected virtual DomainEventReportService CreateEventReport()
    {
        var eventReport = new DomainEventReportService();

        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            if (entry.Entity is not IHasDomainEvents generatesDomainEventsEntity)
            {
                continue;
            }

            var domainEvents = generatesDomainEventsEntity.Events
                .GetAll()?
                .ToArray();

            if (domainEvents != null && domainEvents.Any())
            {
                eventReport.Events.AddRange(domainEvents.ToList());
                generatesDomainEventsEntity.Events.Clear();
            }
        }

        return eventReport;
    }

    /// <summary>
    /// Publishes the domain events collected from entities using MediatR.
    /// </summary>
    /// <param name="changeReport">A report containing domain events to publish.</param>
    private async Task PublishEntityEvents(DomainEventReportService changeReport)
    {
        if (_mediator == null)
        {
            _logger.LogWarning("Os eventos não serão publicados pois o mediator não foi ativado");
            return;
        }

        var localEvents = changeReport
            .Events
            .OrderBy(o => o.Order);

        foreach (var localEvent in localEvents)
        {
            if (localEvent.EventData is INotification notification)
            {
                await _mediator.Publish(notification);
            }
        }
    }
}
