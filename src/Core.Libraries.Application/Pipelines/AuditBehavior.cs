using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.LibrariesApplication.Commands;

namespace Core.LibrariesApplication.Behavior;

public class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly ILogger<AuditBehavior<TRequest, TResponse>> _logger;
    private readonly IAuditStore _auditStore;

    public AuditBehavior(ILogger<AuditBehavior<TRequest, TResponse>> logger, IAuditStore auditStore)
    {
        _logger = logger;
        _auditStore = auditStore;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not BaseCommand baseCommand)
            return await next();

        var permission = baseCommand.GetPermission();

        var entry = new AuditEntry
        {
            UserId = permission?.UserId ?? "anonymous",
            Roles = permission?.Roles ?? Array.Empty<string>(),
            CommandName = typeof(TRequest).Name,
            CommandPayload = SafeSerialize(request),
            ExecutedAtUtc = DateTime.UtcNow
        };

        try
        {
            var response = await next();
            entry.Success = true;

            return response;
        }
        catch (Exception ex)
        {
            entry.Success = false;
            entry.ExceptionMessage = ex.Message;
            throw;
        }
        finally
        {
            try
            {
                await _auditStore.SaveAsync(entry, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar auditoria para {Command}", entry.CommandName);
            }
        }
    }

    private string SafeSerialize(object obj)
    {
        try
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
        catch
        {
            return "[Erro ao serializar comando]";
        }
    }
}

public interface IAuditStore
{
    Task SaveAsync(AuditEntry entry, CancellationToken cancellationToken);
}

public class AuditEntry
{
    public string UserId { get; set; } = default!;
    public string[] Roles { get; set; } = Array.Empty<string>();
    public string CommandName { get; set; } = default!;
    public string CommandPayload { get; set; } = default!;
    public DateTime ExecutedAtUtc { get; set; }
    public bool Success { get; set; }
    public string? ExceptionMessage { get; set; }
}
