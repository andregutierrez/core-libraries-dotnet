namespace Core.LibrariesDomain.Services.Repositories;

/// <summary>
/// Value Object that encapsulates pagination settings using page-based semantics.
/// </summary>
public readonly record struct PaginationOptions
{
    /// <summary>
    /// Gets the current page number (1-based index).
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Gets the number of records per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the sorting criteria (e.g., "CreatedAt desc", "Name asc").
    /// </summary>
    public string Sorting { get; }

    /// <summary>
    /// Calculates how many items to skip based on current page and page size.
    /// </summary>
    public int Skip => (Page - 1) * PageSize;

    /// <summary>
    /// Alias for PageSize, used directly in take operations.
    /// </summary>
    public int Take => PageSize;

    public PaginationOptions(int page, int pageSize, string sorting = "Id asc")
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page must be >= 1.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be >= 1.");
        if (string.IsNullOrWhiteSpace(sorting)) throw new ArgumentNullException(nameof(sorting));

        Page = page;
        PageSize = pageSize;
        Sorting = sorting;
    }

    public static PaginationOptions Default => new(1, 10);
}
