namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents a paginated result containing a collection of data items along with pagination metadata.
/// Used to return paginated data from API endpoints or service methods.
/// </summary>
/// <typeparam name="T">The type of items in the data collection.</typeparam>
/// <param name="Data">The collection of data records for the current page.</param>
/// <param name="Page">The current page number (1-based). If null, pagination information is not provided.</param>
/// <param name="PageSize">The number of items per page. If null, pagination information is not provided.</param>
/// <param name="TotalCount">The total number of records available across all pages. If null, the total count is not provided.</param>
public record PaginatedResultDto<T>(
    /// <summary>
    /// Gets the collection of data records for the current page.
    /// </summary>
    IReadOnlyList<T> Data,

    /// <summary>
    /// Gets the current page number (1-based). Returns <c>null</c> if pagination information is not available.
    /// </summary>
    int? Page = null,

    /// <summary>
    /// Gets the number of items per page. Returns <c>null</c> if pagination information is not available.
    /// </summary>
    int? PageSize = null,

    /// <summary>
    /// Gets the total number of records available across all pages. Returns <c>null</c> if the total count is not available.
    /// </summary>
    int? TotalCount = null
);
