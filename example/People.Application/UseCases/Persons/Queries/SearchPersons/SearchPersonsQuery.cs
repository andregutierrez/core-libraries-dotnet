namespace People.Application.UseCases.Persons.Queries.SearchPersons;

using Core.LibrariesApplication.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to search persons with filters.
/// </summary>
public class SearchPersonsQuery : IQuery<SearchPersonsQueryResponse>
{
    /// <summary>
    /// Gets the person's name to search for (partial match). Optional.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the status type to filter by (Active = 1, Inactive = 2, Merged = 3). Optional.
    /// </summary>
    public int? StatusType { get; init; }

    /// <summary>
    /// Gets a value indicating whether to return only active persons.
    /// </summary>
    public bool? IsActive { get; init; }

    /// <summary>
    /// Gets the page number for pagination. Defaults to 1.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets the page size for pagination. Defaults to 10.
    /// </summary>
    public int PageSize { get; init; } = 10;
}

/// <summary>
/// Response for the SearchPersonsQuery.
/// </summary>
public class SearchPersonsQueryResponse
{
    /// <summary>
    /// Gets the collection of summarized persons matching the search criteria.
    /// </summary>
    public IReadOnlyList<PersonSummaryDto> Persons { get; init; } = Array.Empty<PersonSummaryDto>();

    /// <summary>
    /// Gets the total number of persons matching the search criteria.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

