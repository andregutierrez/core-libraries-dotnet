namespace People.Application.Services.Repositories;

using People.Application.DTOs;

/// <summary>
/// Read repository interface for person queries that return DTOs directly.
/// This repository is optimized for read operations and returns data already mapped to DTOs.
/// </summary>
public interface IPersonReadRepository
{
    /// <summary>
    /// Gets a detailed person by its alternate key.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed person DTO, or null if not found.</returns>
    Task<PersonDetailDto?> GetDetailByKeyAsync(Guid personKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches persons and returns summarized results with pagination.
    /// </summary>
    /// <param name="name">Optional name to search for (partial match).</param>
    /// <param name="statusType">Optional status type to filter by (Active = 1, Inactive = 2, Merged = 3).</param>
    /// <param name="isActive">Optional flag to filter by active status.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the collection of summarized person DTOs and the total count.</returns>
    Task<(IReadOnlyList<PersonSummaryDto> Persons, int TotalCount)> SearchSummariesAsync(
        string? name = null,
        int? statusType = null,
        bool? isActive = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}

