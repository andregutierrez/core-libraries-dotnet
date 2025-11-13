namespace People.Application.UseCases.Localities.Queries.SearchLocalities;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;
using People.Application.Services.Repositories;

/// <summary>
/// Handler for the SearchLocalitiesQuery.
/// </summary>
public class SearchLocalitiesQueryHandler : IQueryHandler<SearchLocalitiesQuery, SearchLocalitiesQueryResponse>
{
    private readonly ILocalityReadRepository _localityReadRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="SearchLocalitiesQueryHandler"/>.
    /// </summary>
    /// <param name="localityReadRepository">The locality read repository.</param>
    public SearchLocalitiesQueryHandler(ILocalityReadRepository localityReadRepository)
    {
        _localityReadRepository = localityReadRepository ?? throw new ArgumentNullException(nameof(localityReadRepository));
    }

    /// <summary>
    /// Handles the SearchLocalitiesQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The search results with pagination information.</returns>
    public async Task<SearchLocalitiesQueryResponse> Handle(
        SearchLocalitiesQuery request,
        CancellationToken cancellationToken)
    {
        var (localities, totalCount) = await _localityReadRepository.SearchAsync(
            request.Name,
            request.LocalityTypeCode,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return new SearchLocalitiesQueryResponse
        {
            Localities = localities,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

