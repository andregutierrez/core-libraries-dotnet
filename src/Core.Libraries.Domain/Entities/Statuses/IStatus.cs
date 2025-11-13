using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Defines a contract for domain entities that expose activation state behavior.
/// </summary>
/// <remarks>
/// Implementing this interface allows a domain entity to transition to an inactive state,
/// which can be used to enforce business rules such as logical disabling, archival, or state-based visibility.
/// </remarks>
public interface IStatus<TType> : IStatusBase
{
    /// <summary>
    /// Gets or sets the category of this status, 
    /// as defined by the <typeparamref name="TStatus"/> enum.
    /// </summary>
    TType Type { get; }
}

public interface IStatusBase
{
    /// <summary>
    /// Gets the alternate GUID key for this status, 
    /// used for external references or secondary indexing.
    /// </summary>
    AlternateKey Key { get; }

    /// <summary>
    /// Gets the UTC timestamp when this status record was created.
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Gets or sets additional notes or context for this status.
    /// </summary>
    string Notes { get; }

    /// <summary>
    /// Gets a value indicating whether this status is the current active status.
    /// </summary>
    bool IsCurrent { get; }

    /// <summary>
    /// Transitions the entity to an inactive state according to domain rules.
    /// </summary>
    /// <remarks>
    /// This method should be implemented with business logic that marks the entity as inactive or non-operational,
    /// without necessarily removing it from persistence.
    /// </remarks>
    void Deactivate();
}

