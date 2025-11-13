using Microsoft.EntityFrameworkCore;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Addresses.Entities;
using People.Domain.Addresses.Services.Repositories;
using People.Domain.Persons.Services.Repositories;
using People.Domain.Localities.Services.Repositories;
using People.Domain.Localities.Entities;
using People.Domain.Persons.Entities;
using People.Application.Services.Repositories;
using People.Application.DTOs;
using People.Infra.Data.Context;
using Core.Libraries.Infra.Data.Repositories;

namespace People.Infra.Data.Repositories;

/// <summary>
/// Repository implementation for managing address entities.
/// Implements both domain repository and application read repository interfaces.
/// </summary>
public class AddressRepository : Repository<Address, EntityId>, IAddressRepository, IAddressReadRepository
{
    private readonly PeopleDbContext _peopleDbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="AddressRepository"/>.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public AddressRepository(DbContext dbContext)
        : base(dbContext)
    {
        _peopleDbContext = (PeopleDbContext)dbContext;
    }

    /// <inheritdoc />
    public async Task<AddressDetailDto?> GetDetailByKeyAsync(Guid addressKey, CancellationToken cancellationToken = default)
    {
        var result = await (from address in _peopleDbContext.Addresses
                            join person in _peopleDbContext.Persons on address.PersonId.Value equals person.Id.Value
                            join street in _peopleDbContext.Localities.OfType<LocalityStreet>() on address.StreetId.Value equals street.Id.Value
                            join city in _peopleDbContext.Localities.OfType<LocalityCity>() on address.CityId.Value equals city.Id.Value
                            from neighborhood in _peopleDbContext.Localities.OfType<LocalityNeighborhood>()
                                .Where(n => address.NeighborhoodId != null && n.Id.Value == address.NeighborhoodId.Value)
                                .DefaultIfEmpty()
                            from state in _peopleDbContext.Localities.OfType<LocalityState>()
                                .Where(s => address.StateId != null && s.Id.Value == address.StateId.Value)
                                .DefaultIfEmpty()
                            from country in _peopleDbContext.Localities.OfType<LocalityCountry>()
                                .Where(c => address.CountryId != null && c.Id.Value == address.CountryId.Value)
                                .DefaultIfEmpty()
                            from postalCode in _peopleDbContext.Localities
                                .Where(pc => address.PostalCodeId != null && pc.Id.Value == address.PostalCodeId.Value)
                                .DefaultIfEmpty()
                            where address.Key.Value == addressKey
                            select new
                            {
                                Address = address,
                                Person = person,
                                Street = street,
                                City = city,
                                Neighborhood = neighborhood,
                                State = state,
                                Country = country,
                                PostalCode = postalCode
                            })
                           .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            return null;

        return new AddressDetailDto
        {
            AddressKey = result.Address.Key.Value,
            Person = new PersonReferenceDto
            {
                PersonKey = result.Person.Key.Value,
                Name = result.Person.Name.DisplayName
            },
            AddressTypeCode = result.Address.Type.Code,
            Street = new LocalityReferenceDto
            {
                LocalityKey = result.Street.Key.Value,
                Name = result.Street.Name
            },
            Number = result.Address.Number,
            Complement = result.Address.Complement,
            Neighborhood = result.Neighborhood != null ? new LocalityReferenceDto
            {
                LocalityKey = result.Neighborhood.Key.Value,
                Name = result.Neighborhood.Name
            } : null,
            City = new LocalityReferenceDto
            {
                LocalityKey = result.City.Key.Value,
                Name = result.City.Name
            },
            State = result.State != null ? new LocalityReferenceDto
            {
                LocalityKey = result.State.Key.Value,
                Name = result.State.Name
            } : null,
            Country = result.Country != null ? new LocalityReferenceDto
            {
                LocalityKey = result.Country.Key.Value,
                Name = result.Country.Name
            } : null,
            PostalCode = result.PostalCode != null ? new LocalityReferenceDto
            {
                LocalityKey = result.PostalCode.Key.Value,
                Name = GetLocalityName(result.PostalCode)
            } : null,
            IsPrimary = result.Address.IsPrimary,
            Notes = result.Address.Notes
        };
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<AddressDetailDto>> GetDetailsByPersonKeyAsync(Guid personKey, CancellationToken cancellationToken = default)
    {
        var result = await (from address in _peopleDbContext.Addresses
                            join person in _peopleDbContext.Persons on address.PersonId.Value equals person.Id.Value
                            join street in _peopleDbContext.Localities.OfType<LocalityStreet>() on address.StreetId.Value equals street.Id.Value
                            join city in _peopleDbContext.Localities.OfType<LocalityCity>() on address.CityId.Value equals city.Id.Value
                            from neighborhood in _peopleDbContext.Localities.OfType<LocalityNeighborhood>()
                                .Where(n => address.NeighborhoodId != null && n.Id.Value == address.NeighborhoodId.Value)
                                .DefaultIfEmpty()
                            from state in _peopleDbContext.Localities.OfType<LocalityState>()
                                .Where(s => address.StateId != null && s.Id.Value == address.StateId.Value)
                                .DefaultIfEmpty()
                            from country in _peopleDbContext.Localities.OfType<LocalityCountry>()
                                .Where(c => address.CountryId != null && c.Id.Value == address.CountryId.Value)
                                .DefaultIfEmpty()
                            from postalCode in _peopleDbContext.Localities
                                .Where(pc => address.PostalCodeId != null && pc.Id.Value == address.PostalCodeId.Value)
                                .DefaultIfEmpty()
                            where person.Key.Value == personKey
                            select new
                            {
                                Address = address,
                                Person = person,
                                Street = street,
                                City = city,
                                Neighborhood = neighborhood,
                                State = state,
                                Country = country,
                                PostalCode = postalCode
                            })
                           .ToListAsync(cancellationToken);

        return result.Select(r => new AddressDetailDto
        {
            AddressKey = r.Address.Key.Value,
            Person = new PersonReferenceDto
            {
                PersonKey = r.Person.Key.Value,
                Name = r.Person.Name.DisplayName
            },
            AddressTypeCode = r.Address.Type.Code,
            Street = new LocalityReferenceDto
            {
                LocalityKey = r.Street.Key.Value,
                Name = r.Street.Name
            },
            Number = r.Address.Number,
            Complement = r.Address.Complement,
            Neighborhood = r.Neighborhood != null ? new LocalityReferenceDto
            {
                LocalityKey = r.Neighborhood.Key.Value,
                Name = r.Neighborhood.Name
            } : null,
            City = new LocalityReferenceDto
            {
                LocalityKey = r.City.Key.Value,
                Name = r.City.Name
            },
            State = r.State != null ? new LocalityReferenceDto
            {
                LocalityKey = r.State.Key.Value,
                Name = r.State.Name
            } : null,
            Country = r.Country != null ? new LocalityReferenceDto
            {
                LocalityKey = r.Country.Key.Value,
                Name = r.Country.Name
            } : null,
            PostalCode = r.PostalCode != null ? new LocalityReferenceDto
            {
                LocalityKey = r.PostalCode.Key.Value,
                Name = GetLocalityName(r.PostalCode)
            } : null,
            IsPrimary = r.Address.IsPrimary,
            Notes = r.Address.Notes
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<AddressDetailDto?> GetPrimaryDetailByPersonKeyAsync(Guid personKey, CancellationToken cancellationToken = default)
    {
        var result = await (from address in _peopleDbContext.Addresses
                            join person in _peopleDbContext.Persons on address.PersonId.Value equals person.Id.Value
                            join street in _peopleDbContext.Localities.OfType<LocalityStreet>() on address.StreetId.Value equals street.Id.Value
                            join city in _peopleDbContext.Localities.OfType<LocalityCity>() on address.CityId.Value equals city.Id.Value
                            from neighborhood in _peopleDbContext.Localities.OfType<LocalityNeighborhood>()
                                .Where(n => address.NeighborhoodId != null && n.Id.Value == address.NeighborhoodId.Value)
                                .DefaultIfEmpty()
                            from state in _peopleDbContext.Localities.OfType<LocalityState>()
                                .Where(s => address.StateId != null && s.Id.Value == address.StateId.Value)
                                .DefaultIfEmpty()
                            from country in _peopleDbContext.Localities.OfType<LocalityCountry>()
                                .Where(c => address.CountryId != null && c.Id.Value == address.CountryId.Value)
                                .DefaultIfEmpty()
                            from postalCode in _peopleDbContext.Localities
                                .Where(pc => address.PostalCodeId != null && pc.Id.Value == address.PostalCodeId.Value)
                                .DefaultIfEmpty()
                            where person.Key.Value == personKey && address.IsPrimary
                            select new
                            {
                                Address = address,
                                Person = person,
                                Street = street,
                                City = city,
                                Neighborhood = neighborhood,
                                State = state,
                                Country = country,
                                PostalCode = postalCode
                            })
                           .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            return null;

        return new AddressDetailDto
        {
            AddressKey = result.Address.Key.Value,
            Person = new PersonReferenceDto
            {
                PersonKey = result.Person.Key.Value,
                Name = result.Person.Name.DisplayName
            },
            AddressTypeCode = result.Address.Type.Code,
            Street = new LocalityReferenceDto
            {
                LocalityKey = result.Street.Key.Value,
                Name = result.Street.Name
            },
            Number = result.Address.Number,
            Complement = result.Address.Complement,
            Neighborhood = result.Neighborhood != null ? new LocalityReferenceDto
            {
                LocalityKey = result.Neighborhood.Key.Value,
                Name = result.Neighborhood.Name
            } : null,
            City = new LocalityReferenceDto
            {
                LocalityKey = result.City.Key.Value,
                Name = result.City.Name
            },
            State = result.State != null ? new LocalityReferenceDto
            {
                LocalityKey = result.State.Key.Value,
                Name = result.State.Name
            } : null,
            Country = result.Country != null ? new LocalityReferenceDto
            {
                LocalityKey = result.Country.Key.Value,
                Name = result.Country.Name
            } : null,
            PostalCode = result.PostalCode != null ? new LocalityReferenceDto
            {
                LocalityKey = result.PostalCode.Key.Value,
                Name = GetLocalityName(result.PostalCode)
            } : null,
            IsPrimary = result.Address.IsPrimary,
            Notes = result.Address.Notes
        };
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<AddressSummaryDto> Addresses, int TotalCount)> SearchSummariesAsync(
        Guid? personKey = null,
        int? addressTypeCode = null,
        bool? isPrimary = null,
        Guid? cityKey = null,
        Guid? stateKey = null,
        Guid? countryKey = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Build base query with joins
        var baseQuery = from address in _peopleDbContext.Addresses
                        join person in _peopleDbContext.Persons on address.PersonId.Value equals person.Id.Value
                        join city in _peopleDbContext.Localities.OfType<LocalityCity>() on address.CityId.Value equals city.Id.Value
                        from state in _peopleDbContext.Localities.OfType<LocalityState>()
                            .Where(s => address.StateId != null && s.Id.Value == address.StateId.Value)
                            .DefaultIfEmpty()
                        from country in _peopleDbContext.Localities.OfType<LocalityCountry>()
                            .Where(c => address.CountryId != null && c.Id.Value == address.CountryId.Value)
                            .DefaultIfEmpty()
                        select new
                        {
                            Address = address,
                            Person = person,
                            City = city,
                            State = state,
                            Country = country
                        };

        // Apply filters
        if (personKey.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Person.Key.Value == personKey.Value);
        }

        if (addressTypeCode.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Address.Type.Code == addressTypeCode.Value);
        }

        if (isPrimary.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Address.IsPrimary == isPrimary.Value);
        }

        if (cityKey.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.City.Key.Value == cityKey.Value);
        }

        if (stateKey.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.State != null && x.State.Key.Value == stateKey.Value);
        }

        if (countryKey.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Country != null && x.Country.Key.Value == countryKey.Value);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var addresses = await baseQuery
            .OrderBy(x => x.Address.Id.Value)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = addresses.Select(x => new AddressSummaryDto
        {
            AddressKey = x.Address.Key.Value,
            PersonKey = x.Person.Key.Value,
            AddressTypeCode = x.Address.Type.Code,
            City = new LocalityReferenceDto
            {
                LocalityKey = x.City.Key.Value,
                Name = x.City.Name
            },
            State = x.State != null ? new LocalityReferenceDto
            {
                LocalityKey = x.State.Key.Value,
                Name = x.State.Name
            } : null,
            Country = x.Country != null ? new LocalityReferenceDto
            {
                LocalityKey = x.Country.Key.Value,
                Name = x.Country.Name
            } : null,
            IsPrimary = x.Address.IsPrimary
        }).ToList();

        return (result, totalCount);
    }

    /// <summary>
    /// Gets the name from a locality entity using reflection if needed.
    /// </summary>
    private static string GetLocalityName(Locality locality)
    {
        // Try to get Name property via reflection
        var nameProperty = locality.GetType().GetProperty("Name");
        if (nameProperty != null && nameProperty.GetValue(locality) is string name)
        {
            return name;
        }

        // Fallback to ToString
        return locality.ToString() ?? string.Empty;
    }
}

