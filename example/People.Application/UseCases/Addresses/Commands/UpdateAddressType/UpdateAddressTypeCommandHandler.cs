namespace People.Application.UseCases.Addresses.Commands.UpdateAddressType;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Addresses.Entities;
using People.Domain.Addresses.Services.Repositories;
using People.Domain.Addresses.ValueObjects;

/// <summary>
/// Handler for the UpdateAddressTypeCommand.
/// </summary>
public class UpdateAddressTypeCommandHandler : ICommandHandler<UpdateAddressTypeCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateAddressTypeCommandHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdateAddressTypeCommandHandler(
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the UpdateAddressTypeCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the address type is invalid.</exception>
    public async Task Handle(
        UpdateAddressTypeCommand request,
        CancellationToken cancellationToken)
    {
        // Resolve AddressKey to Address entity using GetAsync
        var address = await _addressRepository.GetAsync(
            new AlternateKey(request.AddressKey),
            cancellationToken);

        // Resolve AddressType from code
        var addressType = AddressType.FromCode(request.AddressTypeCode);
        if (addressType == null)
            throw new ArgumentException($"Invalid address type code: {request.AddressTypeCode}", nameof(request));

        // Update the address type using the domain method
        address.UpdateType(addressType);

        // Update the address in the repository
        await _addressRepository.UpdateAsync(address, cancellationToken);

        // Commit the transaction
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

