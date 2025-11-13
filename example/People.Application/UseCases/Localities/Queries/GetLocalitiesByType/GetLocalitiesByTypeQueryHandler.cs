namespace People.Application.UseCases.Localities.Queries.GetLocalitiesByType;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;
using People.Application.Services.Repositories;

/// <summary>
/// Handler for the GetLocalitiesByTypeQuery.
/// </summary>
public class GetLocalitiesByTypeQueryHandler : IQueryHandler<GetLocalitiesByTypeQuery, GetLocalitiesByTypeQueryResponse>
{
    private readonly ILocalityReadRepository _localityReadRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="GetLocalitiesByTypeQueryHandler"/>.
    /// </summary>
    /// <param name="localityReadRepository">The locality read repository.</param>
    public GetLocalitiesByTypeQueryHandler(ILocalityReadRepository localityReadRepository)
    {
        _localityReadRepository = localityReadRepository ?? throw new ArgumentNullException(nameof(localityReadRepository));
    }

    /// <summary>
    /// Handles the GetLocalitiesByTypeQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of locality references for the specified type.</returns>
    public async Task<GetLocalitiesByTypeQueryResponse> Handle(
        GetLocalitiesByTypeQuery request,
        CancellationToken cancellationToken)
    {
        var localities = await _localityReadRepository.GetByTypeAsync(request.LocalityTypeCode, cancellationToken);

        return new GetLocalitiesByTypeQueryResponse
        {
            Localities = localities
        };
    }
}

