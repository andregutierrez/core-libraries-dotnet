namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing summarized person information.
/// Used for Search queries that return a list of persons with minimal information.
/// </summary>
public class PersonSummaryDto
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the person's display name.
    /// </summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the person's current status type (Active = 1, Inactive = 2, Merged = 3).
    /// </summary>
    public int? CurrentStatusType { get; init; }

    /// <summary>
    /// Gets a value indicating whether this person is currently active.
    /// </summary>
    public bool IsActive { get; init; }
}

