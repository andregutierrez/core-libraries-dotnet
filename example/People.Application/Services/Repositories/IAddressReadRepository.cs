namespace People.Application.Services.Repositories;

using People.Application.DTOs;

/// <summary>
/// Read repository interface for address queries that return DTOs directly.
/// This repository is optimized for read operations and returns data already mapped to DTOs.
/// </summary>
public interface IAddressReadRepository
{
    /// <summary>
    /// Gets a detailed address by its alternate key.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed address DTO, or null if not found.</returns>
    Task<AddressDetailDto?> GetDetailByKeyAsync(Guid addressKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all detailed addresses for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of detailed address DTOs.</returns>
    Task<IReadOnlyList<AddressDetailDto>> GetDetailsByPersonKeyAsync(Guid personKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the primary detailed address for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed primary address DTO, or null if not found.</returns>
    Task<AddressDetailDto?> GetPrimaryDetailByPersonKeyAsync(Guid personKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches addresses and returns summarized results with pagination.
    /// </summary>
    /// <param name="personKey">Optional person key to filter by.</param>
    /// <param name="addressTypeCode">Optional address type code to filter by.</param>
    /// <param name="isPrimary">Optional flag to filter by primary status.</param>
    /// <param name="cityKey">Optional city key to filter by.</param>
    /// <param name="stateKey">Optional state key to filter by.</param>
    /// <param name="countryKey">Optional country key to filter by.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the collection of summarized address DTOs and the total count.</returns>
    Task<(IReadOnlyList<AddressSummaryDto> Addresses, int TotalCount)> SearchSummariesAsync(
        Guid? personKey = null,
        int? addressTypeCode = null,
        bool? isPrimary = null,
        Guid? cityKey = null,
        Guid? stateKey = null,
        Guid? countryKey = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}

