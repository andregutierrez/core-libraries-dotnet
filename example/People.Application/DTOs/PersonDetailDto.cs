namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing detailed person information.
/// Used for GET queries that return complete person details.
/// </summary>
public class PersonDetailDto
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the person's first name.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the person's last name.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the person's middle name, if any.
    /// </summary>
    public string? MiddleName { get; init; }

    /// <summary>
    /// Gets the person's social name (nome social), if any.
    /// </summary>
    public string? SocialName { get; init; }

    /// <summary>
    /// Gets the person's full registered name.
    /// </summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the person's display name (social name if available, otherwise full name).
    /// </summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the person's birth date, if any.
    /// </summary>
    public DateOnly? BirthDate { get; init; }

    /// <summary>
    /// Gets the person's gender code, if any.
    /// </summary>
    public int? GenderCode { get; init; }

    /// <summary>
    /// Gets the person's current status type (Active = 1, Inactive = 2, Merged = 3).
    /// </summary>
    public int? CurrentStatusType { get; init; }

    /// <summary>
    /// Gets a value indicating whether this person is currently active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets a value indicating whether this person is currently inactive.
    /// </summary>
    public bool IsInactive { get; init; }

    /// <summary>
    /// Gets a value indicating whether this person has been merged.
    /// </summary>
    public bool IsMerged { get; init; }
}

