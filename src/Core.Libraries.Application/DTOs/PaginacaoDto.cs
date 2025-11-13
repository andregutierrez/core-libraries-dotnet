namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents pagination parameters for querying paginated data.
/// Used to specify which page and how many items per page should be returned.
/// </summary>
public record PaginationDto
{
    /// <summary>
    /// Gets or sets the page number (1-based index).
    /// Defaults to 1 if not specified.
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Gets or sets the number of records per page.
    /// Defaults to 10 if not specified.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Gets the default pagination configuration: page 1 with 10 records per page.
    /// </summary>
    public static PaginationDto Default => new() { Page = 1, PageSize = 10 };
}
