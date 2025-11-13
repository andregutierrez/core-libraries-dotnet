namespace People.Domain.Persons.Identifiers;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Represents an identifier for a person in an external system.
/// Used for mapping persons between different systems (DEPara).
/// </summary>
public class PersonIdentifier : Identifier
{
    /// <summary>
    /// Gets the external system's identifier value for this person.
    /// </summary>
    public string ExternalId { get; private set; } = string.Empty;

    /// <summary>
    /// Protected constructor for EF Core and deserialization.
    /// </summary>
    protected PersonIdentifier() { }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonIdentifier"/>.
    /// </summary>
    /// <param name="key">The alternate key for this identifier.</param>
    /// <param name="type">The type of external system.</param>
    /// <param name="externalId">The identifier value in the external system.</param>
    /// <exception cref="ArgumentException">Thrown when externalId is null or whitespace.</exception>
    public PersonIdentifier(AlternateKey key, IdentifierType type, string externalId)
        : base(key.Value, type)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("External ID cannot be null or whitespace.", nameof(externalId));

        ExternalId = externalId;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonIdentifier"/> with an internal ID.
    /// </summary>
    /// <param name="id">The internal identifier for this identifier instance.</param>
    /// <param name="key">The alternate key for this identifier.</param>
    /// <param name="type">The type of external system.</param>
    /// <param name="externalId">The identifier value in the external system.</param>
    /// <exception cref="ArgumentException">Thrown when externalId is null or whitespace.</exception>
    protected PersonIdentifier(IdentifierId id, AlternateKey key, IdentifierType type, string externalId)
        : base(id, key.Value, type)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("External ID cannot be null or whitespace.", nameof(externalId));

        ExternalId = externalId;
    }
}

