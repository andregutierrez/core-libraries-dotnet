namespace People.Application.UseCases.Contacts.Queries;

using Core.Libraries.Application.Queries;

/// <summary>
/// Query to get all contacts of a specific type for a person.
/// </summary>
public class GetContactsByPersonKeyAndTypeQuery : IQuery<GetContactsByPersonKeyAndTypeQueryResponse>
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the type of contact (Email = 1, Phone = 2, Mobile = 3, WhatsApp = 4, SocialMedia = 5).
    /// </summary>
    public int ContactTypeId { get; init; }
}

/// <summary>
/// Response for the GetContactsByPersonKeyAndTypeQuery.
/// </summary>
public class GetContactsByPersonKeyAndTypeQueryResponse
{
    /// <summary>
    /// Gets the collection of contacts.
    /// </summary>
    public IReadOnlyList<ContactDto> Contacts { get; init; } = Array.Empty<ContactDto>();
}

