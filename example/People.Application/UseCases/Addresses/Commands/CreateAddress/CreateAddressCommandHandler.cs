namespace People.Application.UseCases.Addresses.Commands.CreateAddress;

using Core.LibrariesApplication.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Exceptions;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Addresses.Entities;
using People.Domain.Addresses.Services.Repositories;
using People.Domain.Addresses.ValueObjects;
using People.Domain.Persons.Services.Repositories;
using People.Domain.Localities.Services.Repositories;

/// <summary>
/// Handler for the CreateAddressCommand.
/// </summary>
public class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand, CreateAddressCommandResponse>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ILocalityRepository _localityRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateAddressCommandHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="localityRepository">The locality repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public CreateAddressCommandHandler(
        IAddressRepository addressRepository,
        IPersonRepository personRepository,
        ILocalityRepository localityRepository,
        IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the CreateAddressCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response containing the created address's alternate key.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    /// <exception cref="ArgumentException">Thrown when the address type is invalid.</exception>
    public async Task<CreateAddressCommandResponse> Handle(
        CreateAddressCommand request,
        CancellationToken cancellationToken)
    {
        // Resolve PersonKey to EntityId using GetAsync
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);
        var personId = person.Id;

        // Resolve AddressType from code
        var addressType = AddressType.FromCode(request.AddressTypeCode);
        if (addressType == null)
            throw new ArgumentException($"Invalid address type code: {request.AddressTypeCode}", nameof(request));

        // Resolve locality keys to EntityIds using GetAsync
        var street = await _localityRepository.GetAsync(new AlternateKey(request.StreetKey), cancellationToken);
        var streetId = street.Id;

        var city = await _localityRepository.GetAsync(new AlternateKey(request.CityKey), cancellationToken);
        var cityId = city.Id;

        EntityId? neighborhoodId = null;
        if (request.NeighborhoodKey.HasValue)
        {
            var neighborhood = await _localityRepository.GetAsync(new AlternateKey(request.NeighborhoodKey.Value), cancellationToken);
            neighborhoodId = neighborhood.Id;
        }

        EntityId? stateId = null;
        if (request.StateKey.HasValue)
        {
            var state = await _localityRepository.GetAsync(new AlternateKey(request.StateKey.Value), cancellationToken);
            stateId = state.Id;
        }

        EntityId? countryId = null;
        if (request.CountryKey.HasValue)
        {
            var country = await _localityRepository.GetAsync(new AlternateKey(request.CountryKey.Value), cancellationToken);
            countryId = country.Id;
        }

        EntityId? postalCodeId = null;
        if (request.PostalCodeKey.HasValue)
        {
            var postalCode = await _localityRepository.GetAsync(new AlternateKey(request.PostalCodeKey.Value), cancellationToken);
            postalCodeId = postalCode.Id;
        }

        // Create the address using the domain factory method
        var address = Address.Create(
            personId,
            addressType,
            streetId,
            cityId,
            request.Number,
            request.Complement,
            neighborhoodId,
            stateId,
            countryId,
            postalCodeId,
            request.IsPrimary,
            request.Notes);

        // Insert the address
        await _addressRepository.InsertAsync(address, cancellationToken);

        // Commit the transaction
        await _unitOfWork.CommitAsync(cancellationToken);

        // Return the response with the alternate key
        return new CreateAddressCommandResponse
        {
            AddressKey = address.Key.Value
        };
    }
}

