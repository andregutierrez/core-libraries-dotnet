namespace People.Application.UseCases.Addresses.Queries.SearchAddresses;

using System.Linq.Expressions;
using System.Reflection;
using Core.Libraries.Application.Queries;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Application.DTOs;
using People.Domain.Addresses.Entities;
using People.Domain.Addresses.Services.Repositories;
using People.Domain.Persons.Services.Repositories;
using People.Domain.Localities.Services.Repositories;
using People.Domain.Localities.Entities;
using Core.LibrariesDomain.Services.Repositories;

/// <summary>
/// Handler for the SearchAddressesQuery.
/// </summary>
public class SearchAddressesQueryHandler : IQueryHandler<SearchAddressesQuery, SearchAddressesQueryResponse>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ILocalityRepository _localityRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="SearchAddressesQueryHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="localityRepository">The locality repository.</param>
    public SearchAddressesQueryHandler(
        IAddressRepository addressRepository,
        IPersonRepository personRepository,
        ILocalityRepository localityRepository)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
    }

    /// <summary>
    /// Handles the SearchAddressesQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The paginated collection of summarized addresses matching the search criteria.</returns>
    public async Task<SearchAddressesQueryResponse> Handle(
        SearchAddressesQuery request,
        CancellationToken cancellationToken)
    {
        // Build filter conditions
        EntityId? personId = null;
        if (request.PersonKey.HasValue)
        {
            var person = await _personRepository.GetAsync(
                new AlternateKey(request.PersonKey.Value),
                cancellationToken);
            personId = person.Id;
        }

        People.Domain.Addresses.ValueObjects.AddressType? addressType = null;
        if (request.AddressTypeCode.HasValue)
        {
            addressType = People.Domain.Addresses.ValueObjects.AddressType.FromCode(request.AddressTypeCode.Value);
        }

        EntityId? cityId = null;
        if (request.CityKey.HasValue)
        {
            var city = await _localityRepository.GetAsync(
                new AlternateKey(request.CityKey.Value),
                cancellationToken);
            cityId = city.Id;
        }

        EntityId? stateId = null;
        if (request.StateKey.HasValue)
        {
            var state = await _localityRepository.GetAsync(
                new AlternateKey(request.StateKey.Value),
                cancellationToken);
            stateId = state.Id;
        }

        EntityId? countryId = null;
        if (request.CountryKey.HasValue)
        {
            var country = await _localityRepository.GetAsync(
                new AlternateKey(request.CountryKey.Value),
                cancellationToken);
            countryId = country.Id;
        }

        // Build filter expression
        Expression<Func<Address, bool>> predicate = a =>
            (!personId.HasValue || a.PersonId == personId.Value) &&
            (addressType == null || a.Type == addressType) &&
            (!request.IsPrimary.HasValue || a.IsPrimary == request.IsPrimary.Value) &&
            (!cityId.HasValue || a.CityId == cityId.Value) &&
            (!stateId.HasValue || a.StateId == stateId.Value) &&
            (!countryId.HasValue || a.CountryId == countryId.Value);

        // Get all matching addresses (for total count)
        var allAddresses = await _addressRepository.SearchAsync(
            a => a.Id,
            predicate,
            cancellationToken);

        var totalCount = allAddresses.Count();

        // Apply pagination
        var pagination = new PaginationOptions(request.PageNumber, request.PageSize);
        var pagedAddresses = await _addressRepository.SearchAsync(
            a => a.Id,
            predicate,
            pagination,
            cancellationToken);

        // Map to summary DTOs (minimal information for search results)
        var summaryDtos = new List<AddressSummaryDto>();

        foreach (var address in pagedAddresses)
        {
            // For search results, we only need minimal locality information with names
            var city = await _localityRepository.GetAsync(address.CityId, cancellationToken) as LocalityCity;

            var state = address.StateId.HasValue
                ? await _localityRepository.GetAsync(address.StateId.Value, cancellationToken) as LocalityState
                : null;

            var country = address.CountryId.HasValue
                ? await _localityRepository.GetAsync(address.CountryId.Value, cancellationToken) as LocalityCountry
                : null;

            // Resolve PersonId to PersonKey
            var person = await _personRepository.GetAsync(address.PersonId, cancellationToken);

            summaryDtos.Add(new AddressSummaryDto
            {
                AddressKey = address.Key.Value,
                PersonKey = person.Key.Value,
                AddressTypeCode = address.Type.Code,
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
                IsPrimary = address.IsPrimary
            });
        }

        return new SearchAddressesQueryResponse
        {
            Addresses = summaryDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

