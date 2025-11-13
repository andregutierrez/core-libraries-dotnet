namespace People.Application.UseCases.Addresses.Commands.UpdateAddress;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Addresses.Entities;
using People.Domain.Addresses.Services.Repositories;
using People.Domain.Localities.Services.Repositories;

/// <summary>
/// Handler for the UpdateAddressCommand.
/// </summary>
public class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly ILocalityRepository _localityRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateAddressCommandHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="localityRepository">The locality repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdateAddressCommandHandler(
        IAddressRepository addressRepository,
        ILocalityRepository localityRepository,
        IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the UpdateAddressCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(
        UpdateAddressCommand request,
        CancellationToken cancellationToken)
    {
        // Resolve AddressKey to Address entity using GetAsync
        var address = await _addressRepository.GetAsync(
            new AlternateKey(request.AddressKey),
            cancellationToken);

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

        // Update the address using the domain method
        address.Update(
            streetId,
            cityId,
            request.Number,
            request.Complement,
            neighborhoodId,
            stateId,
            countryId,
            postalCodeId);

        // Update the address in the repository
        await _addressRepository.UpdateAsync(address, cancellationToken);

        // Commit the transaction
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

