namespace People.Domain.Persons.Events;

using Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Domain event raised when an external system identifier is added to a person.
/// </summary>
public record PersonIdentifierAddedEvent
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the type of external system.
    /// </summary>
    public IdentifierType Type { get; init; } = null!;

    /// <summary>
    /// Gets the external system's identifier value.
    /// </summary>
    public string ExternalId { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of <see cref="PersonIdentifierAddedEvent"/>.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="type">The type of external system.</param>
    /// <param name="externalId">The external system's identifier value.</param>
    public PersonIdentifierAddedEvent(Guid personKey, IdentifierType type, string externalId)
    {
        PersonKey = personKey;
        Type = type;
        ExternalId = externalId;
    }
}

