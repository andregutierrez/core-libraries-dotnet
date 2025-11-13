namespace People.Domain.Persons.Entities.Statuses;

/// <summary>
/// Represents the possible status types for a person.
/// </summary>
public enum PersonStatusType
{
    /// <summary>
    /// Person is active and operational.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Person is inactive (disabled or archived).
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// Person has been merged with another person.
    /// </summary>
    Merged = 3
}

