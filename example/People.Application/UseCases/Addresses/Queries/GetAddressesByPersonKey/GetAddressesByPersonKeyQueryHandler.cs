namespace People.Application.UseCases.Addresses.Queries.GetAddressesByPersonKey;

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
/// Handler for the GetAddressesByPersonKeyQuery.
/// </summary>
public class GetAddressesByPersonKeyQueryHandler : IQueryHandler<GetAddressesByPersonKeyQuery, GetAddressesByPersonKeyQueryResponse>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ILocalityRepository _localityRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="GetAddressesByPersonKeyQueryHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="localityRepository">The locality repository.</param>
    public GetAddressesByPersonKeyQueryHandler(
        IAddressRepository addressRepository,
        IPersonRepository personRepository,
        ILocalityRepository localityRepository)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
    }

    /// <summary>
    /// Handles the GetAddressesByPersonKeyQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of detailed addresses for the person.</returns>
    public async Task<GetAddressesByPersonKeyQueryResponse> Handle(
        GetAddressesByPersonKeyQuery request,
        CancellationToken cancellationToken)
    {
        // Resolve PersonKey to PersonId
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        // Search addresses by PersonId
        var addresses = await _addressRepository.SearchAsync(
            a => a.Id,
            a => a.PersonId == person.Id,
            cancellationToken);

        var addressDtos = new List<AddressDetailDto>();

        foreach (var address in addresses)
        {
            // Resolve locality EntityIds to Keys and Names
            var street = await _localityRepository.GetAsync(address.StreetId, cancellationToken) as LocalityStreet;
            var city = await _localityRepository.GetAsync(address.CityId, cancellationToken) as LocalityCity;

            var neighborhood = address.NeighborhoodId.HasValue
                ? await _localityRepository.GetAsync(address.NeighborhoodId.Value, cancellationToken) as LocalityNeighborhood
                : null;

            var state = address.StateId.HasValue
                ? await _localityRepository.GetAsync(address.StateId.Value, cancellationToken) as LocalityState
                : null;

            var country = address.CountryId.HasValue
                ? await _localityRepository.GetAsync(address.CountryId.Value, cancellationToken) as LocalityCountry
                : null;

            var postalCode = address.PostalCodeId.HasValue
                ? await _localityRepository.GetAsync(address.PostalCodeId.Value, cancellationToken)
                : null;

            addressDtos.Add(new AddressDetailDto
            {
                AddressKey = address.Key.Value,
                Person = new PersonReferenceDto
                {
                    PersonKey = person.Key.Value
                },
                AddressTypeCode = address.Type.Code,
                Street = new LocalityReferenceDto
                {
                    LocalityKey = street!.Key.Value,
                    Name = street.Name
                },
                Number = address.Number,
                Complement = address.Complement,
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
                IsPrimary = address.IsPrimary,
                Notes = address.Notes
            });
        }

        return new GetAddressesByPersonKeyQueryResponse
        {
            Addresses = addressDtos
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

