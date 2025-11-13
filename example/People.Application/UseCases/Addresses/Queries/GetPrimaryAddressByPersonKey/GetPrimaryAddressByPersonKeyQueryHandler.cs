namespace People.Application.UseCases.Addresses.Queries.GetPrimaryAddressByPersonKey;

using System.Reflection;
using Core.Libraries.Application.Queries;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Application.DTOs;
using People.Domain.Addresses.Services.Repositories;
using People.Domain.Persons.Services.Repositories;
using People.Domain.Localities.Services.Repositories;
using People.Domain.Localities.Entities;

/// <summary>
/// Handler for the GetPrimaryAddressByPersonKeyQuery.
/// </summary>
public class GetPrimaryAddressByPersonKeyQueryHandler : IQueryHandler<GetPrimaryAddressByPersonKeyQuery, GetPrimaryAddressByPersonKeyQueryResponse?>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ILocalityRepository _localityRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="GetPrimaryAddressByPersonKeyQueryHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="localityRepository">The locality repository.</param>
    public GetPrimaryAddressByPersonKeyQueryHandler(
        IAddressRepository addressRepository,
        IPersonRepository personRepository,
        ILocalityRepository localityRepository)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
    }

    /// <summary>
    /// Handles the GetPrimaryAddressByPersonKeyQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed primary address information, or null if not found.</returns>
    public async Task<GetPrimaryAddressByPersonKeyQueryResponse?> Handle(
        GetPrimaryAddressByPersonKeyQuery request,
        CancellationToken cancellationToken)
    {
        // Resolve PersonKey to PersonId
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        // Search for primary address by PersonId
        var addresses = await _addressRepository.SearchAsync(
            a => a.Id,
            a => a.PersonId == person.Id && a.IsPrimary,
            cancellationToken);

        var primaryAddress = addresses.FirstOrDefault();
        if (primaryAddress == null)
            return null;

        // Resolve locality EntityIds to Keys and Names
        var street = await _localityRepository.GetAsync(primaryAddress.StreetId, cancellationToken) as LocalityStreet;
        var city = await _localityRepository.GetAsync(primaryAddress.CityId, cancellationToken) as LocalityCity;

        var neighborhood = primaryAddress.NeighborhoodId.HasValue
            ? await _localityRepository.GetAsync(primaryAddress.NeighborhoodId.Value, cancellationToken) as LocalityNeighborhood
            : null;

        var state = primaryAddress.StateId.HasValue
            ? await _localityRepository.GetAsync(primaryAddress.StateId.Value, cancellationToken) as LocalityState
            : null;

        var country = primaryAddress.CountryId.HasValue
            ? await _localityRepository.GetAsync(primaryAddress.CountryId.Value, cancellationToken) as LocalityCountry
            : null;

        var postalCode = primaryAddress.PostalCodeId.HasValue
            ? await _localityRepository.GetAsync(primaryAddress.PostalCodeId.Value, cancellationToken)
            : null;

        return new GetPrimaryAddressByPersonKeyQueryResponse
        {
            Address = new AddressDetailDto
            {
                AddressKey = primaryAddress.Key.Value,
                Person = new PersonReferenceDto
                {
                    PersonKey = person.Key.Value
                },
                AddressTypeCode = primaryAddress.Type.Code,
                Street = new LocalityReferenceDto
                {
                    LocalityKey = street!.Key.Value,
                    Name = street.Name
                },
                Number = primaryAddress.Number,
                Complement = primaryAddress.Complement,
                Neighborhood = neighborhood != null ? new LocalityReferenceDto
                {
                    LocalityKey = neighborhood.Key.Value,
                    Name = neighborhood.Name
                } : null,
                City = new LocalityReferenceDto
                {
                    LocalityKey = city!.Key.Value,
                    Name = city.Name
                },
                State = state != null ? new LocalityReferenceDto
                {
                    LocalityKey = state.Key.Value,
                    Name = state.Name
                } : null,
                Country = country != null ? new LocalityReferenceDto
                {
                    LocalityKey = country.Key.Value,
                    Name = country.Name
                } : null,
                PostalCode = postalCode != null ? new LocalityReferenceDto
                {
                    LocalityKey = postalCode.Key.Value,
                    Name = GetLocalityName(postalCode)
                } : null,
                IsPrimary = primaryAddress.IsPrimary,
                Notes = primaryAddress.Notes
            }
        };
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
        return locality.ToString();
    }
}

