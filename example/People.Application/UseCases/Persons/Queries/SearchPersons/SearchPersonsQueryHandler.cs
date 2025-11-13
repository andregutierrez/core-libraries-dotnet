namespace People.Application.UseCases.Persons.Queries.SearchPersons;

using Core.LibrariesApplication.Queries;
using People.Application.DTOs;
using People.Application.Services.Repositories;

/// <summary>
/// Handler for the SearchPersonsQuery.
/// </summary>
public class SearchPersonsQueryHandler : IQueryHandler<SearchPersonsQuery, SearchPersonsQueryResponse>
{
    private readonly IPersonReadRepository _personReadRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="SearchPersonsQueryHandler"/>.
    /// </summary>
    /// <param name="personReadRepository">The person read repository.</param>
    public SearchPersonsQueryHandler(IPersonReadRepository personReadRepository)
    {
        _personReadRepository = personReadRepository ?? throw new ArgumentNullException(nameof(personReadRepository));
    }

    /// <summary>
    /// Handles the SearchPersonsQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The search results with pagination information.</returns>
    public async Task<SearchPersonsQueryResponse> Handle(
        SearchPersonsQuery request,
        CancellationToken cancellationToken)
    {
        var (persons, totalCount) = await _personReadRepository.SearchSummariesAsync(
            request.Name,
            request.StatusType,
            request.IsActive,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return new SearchPersonsQueryResponse
        {
            Persons = persons,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

