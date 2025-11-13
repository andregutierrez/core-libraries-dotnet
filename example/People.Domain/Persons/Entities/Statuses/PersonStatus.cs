namespace People.Domain.Persons.Entities.Statuses;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Represents a status record for a person entity.
/// </summary>
public class PersonStatus : Status<PersonStatusType>
{
    /// <summary>
    /// Protected constructor for EF Core and deserialization.
    /// </summary>
    protected PersonStatus() { }

    /// <summary>
    /// Initializes a new instance of <see cref="PersonStatus"/>.
    /// </summary>
    /// <param name="key">The alternate key for this status.</param>
    /// <param name="createdAt">The timestamp when this status was created.</param>
    /// <param name="type">The type of status.</param>
    /// <param name="notes">Optional notes or context for this status.</param>
    public PersonStatus(AlternateKey key, DateTime createdAt, PersonStatusType type, string notes = "")
        : base(key.Value, createdAt, type, notes) { }
}

