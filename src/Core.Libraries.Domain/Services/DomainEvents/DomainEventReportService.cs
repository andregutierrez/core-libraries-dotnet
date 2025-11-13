using Core.Libraries.Domain.Entities.DomainEvents;

namespace Core.LibrariesDomain.Services.DomainEvents;

/// <summary>
/// Collects and exposes domain events raised during the execution of a use case or request.
/// Useful for auditing, debugging, or dispatching domain events after a unit of work completes.
/// </summary>
public class DomainEventReportService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventReportService"/> class.
    /// </summary>
    public DomainEventReportService()
    {
        Events = new();
    }

    /// <summary>
    /// Gets the list of domain events that were captured.
    /// </summary>
    public List<DomainEvent> Events { get; }

    /// <summary>
    /// Returns a string summary of the current domain event report.
    /// </summary>
    /// <returns>A string showing the number of domain events collected.</returns>
    public override string ToString()
    {
        return $"[{nameof(DomainEventReportService)}] DomainEvents: {Events.Count}";
    }
}
