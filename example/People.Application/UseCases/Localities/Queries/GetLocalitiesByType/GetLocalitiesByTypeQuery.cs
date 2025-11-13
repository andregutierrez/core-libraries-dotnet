namespace People.Application.UseCases.Localities.Queries.GetLocalitiesByType;

using Core.Libraries.Application.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to get all localities of a specific type.
/// </summary>
public class GetLocalitiesByTypeQuery : IQuery<GetLocalitiesByTypeQueryResponse>
{
    /// <summary>
    /// Gets the locality type code (City = 1, Country = 2, State = 3, Neighborhood = 4, Street = 5).
    /// </summary>
    public int LocalityTypeCode { get; init; }
}

/// <summary>
/// Response for the GetLocalitiesByTypeQuery.
/// </summary>
public class GetLocalitiesByTypeQueryResponse
{
    /// <summary>
    /// Gets the collection of locality references.
    /// </summary>
    public IReadOnlyList<LocalityReferenceDto> Localities { get; init; } = Array.Empty<LocalityReferenceDto>();
}

