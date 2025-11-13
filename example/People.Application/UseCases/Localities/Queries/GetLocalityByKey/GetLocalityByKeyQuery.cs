namespace People.Application.UseCases.Localities.Queries.GetLocalityByKey;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to get a locality by its alternate key.
/// </summary>
public class GetLocalityByKeyQuery : IQuery<GetLocalityByKeyQueryResponse?>
{
    /// <summary>
    /// Gets the locality's alternate key.
    /// </summary>
    public Guid LocalityKey { get; init; }
}

/// <summary>
/// Response for the GetLocalityByKeyQuery.
/// </summary>
public class GetLocalityByKeyQueryResponse
{
    /// <summary>
    /// Gets the locality reference information.
    /// </summary>
    public LocalityReferenceDto Locality { get; init; } = null!;
}

