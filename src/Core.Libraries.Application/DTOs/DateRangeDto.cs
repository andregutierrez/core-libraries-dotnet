namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents a date range filter with optional start and end dates.
/// Used for filtering data within a specific time period.
/// </summary>
public record DateRangeDto
{
    /// <summary>
    /// Gets or sets the start date of the range (inclusive).
    /// If null, the range has no lower bound.
    /// </summary>
    public DateTimeOffset? From { get; init; }

    /// <summary>
    /// Gets or sets the end date of the range (inclusive).
    /// If null, the range has no upper bound.
    /// </summary>
    public DateTimeOffset? To { get; init; }

    /// <summary>
    /// Gets a value indicating whether at least one date boundary is specified.
    /// Returns <c>true</c> if either <see cref="From"/> or <see cref="To"/> has a value.
    /// </summary>
    public bool HasRange => From.HasValue || To.HasValue;
}
