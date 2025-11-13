namespace People.Application.UseCases.Contacts.Queries.SearchContacts;

using Core.Libraries.Application.Queries;
using People.Application.UseCases.Contacts.Queries.GetContactsByPersonKey;

/// <summary>
/// Query to search contacts with filters.
/// </summary>
public class SearchContactsQuery : IQuery<SearchContactsQueryResponse>
{
    /// <summary>
    /// Gets the person's alternate key to filter by. Optional.
    /// </summary>
    public Guid? PersonKey { get; init; }

    /// <summary>
    /// Gets the contact type ID to filter by. Optional.
    /// </summary>
    public int? ContactTypeId { get; init; }

    /// <summary>
    /// Gets a value indicating whether to return only primary contacts.
    /// </summary>
    public bool? IsPrimary { get; init; }

    /// <summary>
    /// Gets the email address to search for. Optional.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets the phone number to search for. Optional.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets the social media platform ID to filter by. Optional.
    /// </summary>
    public int? SocialMediaPlatformId { get; init; }

    /// <summary>
    /// Gets the page number for pagination. Defaults to 1.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets the page size for pagination. Defaults to 10.
    /// </summary>
    public int PageSize { get; init; } = 10;
}

/// <summary>
/// Response for the SearchContactsQuery.
/// </summary>
public class SearchContactsQueryResponse
{
    /// <summary>
    /// Gets the collection of contacts matching the search criteria.
    /// </summary>
    public IReadOnlyList<ContactDto> Contacts { get; init; } = Array.Empty<ContactDto>();

    /// <summary>
    /// Gets the total number of contacts matching the search criteria.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

