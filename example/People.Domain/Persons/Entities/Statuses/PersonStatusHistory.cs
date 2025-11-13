namespace People.Domain.Persons.Entities.Statuses;

using Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Manages the status history for a person entity.
/// Ensures only one status is current at a time.
/// </summary>
public class PersonStatusHistory : StatusHistory<PersonStatus>
{
    /// <summary>
    /// Extracts the enum type from the status.
    /// </summary>
    /// <param name="status">The status entity.</param>
    /// <returns>The enum value of the status.</returns>
    protected override Enum GetStatusType(PersonStatus status)
        => status.Type;

    /// <summary>
    /// Gets the current active status, if any.
    /// </summary>
    /// <returns>The current status, or <c>null</c> if no status is active.</returns>
    public PersonStatus? GetCurrentStatus()
    {
        try
        {
            return GetCurrent();
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the current status type, if any.
    /// </summary>
    /// <returns>The current status type, or <c>null</c> if no status is active.</returns>
    public PersonStatusType? GetCurrentStatusType()
        => GetCurrentStatus()?.Type;

    /// <summary>
    /// Checks if the person is currently active.
    /// </summary>
    /// <returns><c>true</c> if the current status is Active; otherwise, <c>false</c>.</returns>
    public bool IsActive()
        => GetCurrentStatusType() == PersonStatusType.Active;

    /// <summary>
    /// Checks if the person is currently inactive.
    /// </summary>
    /// <returns><c>true</c> if the current status is Inactive; otherwise, <c>false</c>.</returns>
    public bool IsInactive()
        => GetCurrentStatusType() == PersonStatusType.Inactive;

    /// <summary>
    /// Checks if the person has been merged.
    /// </summary>
    /// <returns><c>true</c> if the current status is Merged; otherwise, <c>false</c>.</returns>
    public bool IsMerged()
        => GetCurrentStatusType() == PersonStatusType.Merged;
}

