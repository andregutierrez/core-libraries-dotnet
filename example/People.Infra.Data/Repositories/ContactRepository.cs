using Microsoft.EntityFrameworkCore;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesInfra.Data.Postgress.Repositories;
using People.Domain.Contacts.Entities;
using People.Domain.Contacts.Services.Repositories;
using People.Domain.Persons.Entities;
using People.Application.Services.Repositories;
using People.Application.DTOs;
using People.Infra.Data.Context;

namespace People.Infra.Data.Repositories;

/// <summary>
/// Repository implementation for managing contact entities.
/// Implements both domain repository and application read repository interfaces.
/// </summary>
public class ContactRepository : Repository<Contact, EntityId>, IContactRepository, IContactReadRepository
{
    private readonly PeopleDbContext _peopleDbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="ContactRepository"/>.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ContactRepository(DbContext dbContext)
        : base(dbContext)
    {
        _peopleDbContext = (PeopleDbContext)dbContext;
    }

    /// <inheritdoc />
    public async Task<ContactDto?> GetByKeyAsync(Guid contactKey, CancellationToken cancellationToken = default)
    {
        var result = await (from contact in _peopleDbContext.Contacts
                            join person in _peopleDbContext.Persons on contact.PersonId.Value equals person.Id.Value
                            where contact.Key.Value == contactKey
                            select new
                            {
                                Contact = contact,
                                Person = person
                            })
                           .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            return null;

        return MapToContactDto(result.Contact, result.Person);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ContactDto>> GetByPersonKeyAsync(Guid personKey, CancellationToken cancellationToken = default)
    {
        var result = await (from contact in _peopleDbContext.Contacts
                            join person in _peopleDbContext.Persons on contact.PersonId.Value equals person.Id.Value
                            where person.Key.Value == personKey
                            select new
                            {
                                Contact = contact,
                                Person = person
                            })
                           .ToListAsync(cancellationToken);

        return result.Select(r => MapToContactDto(r.Contact, r.Person)).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ContactDto>> GetByPersonKeyAndTypeAsync(Guid personKey, int contactTypeCode, CancellationToken cancellationToken = default)
    {
        var result = await (from contact in _peopleDbContext.Contacts
                            join person in _peopleDbContext.Persons on contact.PersonId.Value equals person.Id.Value
                            where person.Key.Value == personKey && contact.Type.Code == contactTypeCode
                            select new
                            {
                                Contact = contact,
                                Person = person
                            })
                           .ToListAsync(cancellationToken);

        return result.Select(r => MapToContactDto(r.Contact, r.Person)).ToList();
    }

    /// <inheritdoc />
    public async Task<ContactDto?> GetPrimaryByPersonKeyAndTypeAsync(Guid personKey, int contactTypeCode, CancellationToken cancellationToken = default)
    {
        var result = await (from contact in _peopleDbContext.Contacts
                            join person in _peopleDbContext.Persons on contact.PersonId.Value equals person.Id.Value
                            where person.Key.Value == personKey && contact.Type.Code == contactTypeCode && contact.IsPrimary
                            select new
                            {
                                Contact = contact,
                                Person = person
                            })
                           .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            return null;

        return MapToContactDto(result.Contact, result.Person);
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<ContactDto> Contacts, int TotalCount)> SearchAsync(
        Guid? personKey = null,
        int? contactTypeCode = null,
        bool? isPrimary = null,
        string? email = null,
        string? phoneNumber = null,
        int? socialMediaPlatformCode = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Build base query with join to Person
        var baseQuery = from contact in _peopleDbContext.Contacts
                        join person in _peopleDbContext.Persons on contact.PersonId.Value equals person.Id.Value
                        select new
                        {
                            Contact = contact,
                            Person = person
                        };

        // Apply filters
        if (personKey.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Person.Key.Value == personKey.Value);
        }

        if (contactTypeCode.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Contact.Type.Code == contactTypeCode.Value);
        }

        if (isPrimary.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Contact.IsPrimary == isPrimary.Value);
        }

        // Apply type-specific filters - materialize and filter in memory for type-specific properties
        // Note: EF Core has limitations with pattern matching on TPH hierarchies
        // We'll apply these filters after materialization
        var materializedQuery = baseQuery;

        // Materialize all matching contacts first
        var allContacts = await materializedQuery.ToListAsync(cancellationToken);

        // Apply type-specific filters in memory
        var filteredContacts = allContacts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(email))
        {
            filteredContacts = filteredContacts.Where(x =>
                x.Contact is EmailContact emailContact &&
                emailContact.Email.Value.Contains(email, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            filteredContacts = filteredContacts.Where(x =>
                x.Contact is PhoneContact phoneContact &&
                phoneContact.Phone.Value.Contains(phoneNumber, StringComparison.OrdinalIgnoreCase));
        }

        if (socialMediaPlatformCode.HasValue)
        {
            filteredContacts = filteredContacts.Where(x =>
                x.Contact is SocialMediaContact smContact &&
                smContact.SocialMedia.Platform.Code == socialMediaPlatformCode.Value);
        }

        var contactsList = filteredContacts.ToList();
        var totalCount = contactsList.Count;

        var contacts = contactsList
            .OrderBy(x => x.Contact.Id.Value)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = contacts.Select(x => MapToContactDto(x.Contact, x.Person)).ToList();

        return (result, totalCount);
    }

    /// <summary>
    /// Maps a Contact entity to a ContactDto.
    /// </summary>
    private static ContactDto MapToContactDto(Contact contact, Person person)
    {
        var dto = new ContactDto
        {
            ContactKey = contact.Key.Value,
            Person = new PersonReferenceDto
            {
                PersonKey = person.Key.Value,
                Name = person.Name.DisplayName
            },
            ContactTypeCode = contact.Type.Code,
            DisplayValue = contact.DisplayValue,
            IsPrimary = contact.IsPrimary,
            Notes = contact.Notes
        };

        // Map specific contact types
        switch (contact)
        {
            case EmailContact emailContact:
                dto = dto with
                {
                    Email = new EmailContactDto
                    {
                        Email = emailContact.Email.Value
                    }
                };
                break;

            case PhoneContact phoneContact:
                dto = dto with
                {
                    Phone = new PhoneContactDto
                    {
                        PhoneNumber = phoneContact.Phone.Value,
                        CountryCode = phoneContact.Phone.Formatted // Using Formatted as fallback for CountryCode
                    }
                };
                break;

            case SocialMediaContact socialMediaContact:
                dto = dto with
                {
                    SocialMedia = new SocialMediaContactDto
                    {
                        PlatformCode = socialMediaContact.SocialMedia.Platform.Code,
                        Username = socialMediaContact.SocialMedia.Username,
                        ProfileUrl = socialMediaContact.SocialMedia.ProfileUrl
                    }
                };
                break;
        }

        return dto;
    }
}

