namespace People.Application.UseCases.Localities.Queries.GetLocalityByKey;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;
using People.Application.Services.Repositories;

/// <summary>
/// Handler for the GetLocalityByKeyQuery.
/// </summary>
public class GetLocalityByKeyQueryHandler : IQueryHandler<GetLocalityByKeyQuery, GetLocalityByKeyQueryResponse?>
{
    private readonly ILocalityReadRepository _localityReadRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="GetLocalityByKeyQueryHandler"/>.
    /// </summary>
    /// <param name="localityReadRepository">The locality read repository.</param>
    public GetLocalityByKeyQueryHandler(ILocalityReadRepository localityReadRepository)
    {
        _localityReadRepository = localityReadRepository ?? throw new ArgumentNullException(nameof(localityReadRepository));
    }

    /// <summary>
    /// Handles the GetLocalityByKeyQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The locality reference information, or null if not found.</returns>
    public async Task<GetLocalityByKeyQueryResponse?> Handle(
        GetLocalityByKeyQuery request,
        CancellationToken cancellationToken)
    {
        var locality = await _localityReadRepository.GetByKeyAsync(request.LocalityKey, cancellationToken);

        if (locality == null)
            return null;

        return new GetLocalityByKeyQueryResponse
        {
            Locality = locality
        };
    }
}

