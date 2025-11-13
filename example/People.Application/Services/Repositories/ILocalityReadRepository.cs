namespace People.Application.Services.Repositories;

using People.Application.DTOs;

/// <summary>
/// Read repository interface for locality queries that return DTOs directly.
/// This repository is optimized for read operations and returns data already mapped to DTOs.
/// </summary>
public interface ILocalityReadRepository
{
    /// <summary>
    /// Gets a locality by its alternate key.
    /// </summary>
    /// <param name="localityKey">The locality's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The locality reference DTO, or null if not found.</returns>
    Task<LocalityReferenceDto?> GetByKeyAsync(Guid localityKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets localities by type.
    /// </summary>
    /// <param name="localityTypeCode">The locality type code (City = 1, Country = 2, State = 3, Neighborhood = 4, Street = 5).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of locality reference DTOs.</returns>
    Task<IReadOnlyList<LocalityReferenceDto>> GetByTypeAsync(int localityTypeCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches localities by name and type.
    /// </summary>
    /// <param name="name">The locality name to search for (partial match).</param>
    /// <param name="localityTypeCode">Optional locality type code to filter by.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the collection of locality reference DTOs and the total count.</returns>
    Task<(IReadOnlyList<LocalityReferenceDto> Localities, int TotalCount)> SearchAsync(
        string? name = null,
        int? localityTypeCode = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}

