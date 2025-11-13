namespace People.Application.Services.Repositories;

using People.Application.DTOs;

/// <summary>
/// Read repository interface for contact queries that return DTOs directly.
/// This repository is optimized for read operations and returns data already mapped to DTOs.
/// </summary>
public interface IContactReadRepository
{
    /// <summary>
    /// Gets a contact by its alternate key.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The contact DTO, or null if not found.</returns>
    Task<ContactDto?> GetByKeyAsync(Guid contactKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all contacts for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of contact DTOs.</returns>
    Task<IReadOnlyList<ContactDto>> GetByPersonKeyAsync(Guid personKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all contacts of a specific type for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="contactTypeCode">The contact type code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of contact DTOs.</returns>
    Task<IReadOnlyList<ContactDto>> GetByPersonKeyAndTypeAsync(Guid personKey, int contactTypeCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the primary contact of a specific type for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="contactTypeCode">The contact type code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The primary contact DTO, or null if not found.</returns>
    Task<ContactDto?> GetPrimaryByPersonKeyAndTypeAsync(Guid personKey, int contactTypeCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches contacts and returns results with pagination.
    /// </summary>
    /// <param name="personKey">Optional person key to filter by.</param>
    /// <param name="contactTypeCode">Optional contact type code to filter by.</param>
    /// <param name="isPrimary">Optional flag to filter by primary status.</param>
    /// <param name="email">Optional email address to search for.</param>
    /// <param name="phoneNumber">Optional phone number to search for.</param>
    /// <param name="socialMediaPlatformCode">Optional social media platform code to filter by.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the collection of contact DTOs and the total count.</returns>
    Task<(IReadOnlyList<ContactDto> Contacts, int TotalCount)> SearchAsync(
        Guid? personKey = null,
        int? contactTypeCode = null,
        bool? isPrimary = null,
        string? email = null,
        string? phoneNumber = null,
        int? socialMediaPlatformCode = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}

