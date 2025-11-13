namespace People.Application.UseCases.Addresses.Commands.SetAddressAsPrimary;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Addresses.Services.Repositories;

/// <summary>
/// Handler for the SetAddressAsPrimaryCommand.
/// </summary>
public class SetAddressAsPrimaryCommandHandler : ICommandHandler<SetAddressAsPrimaryCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="SetAddressAsPrimaryCommandHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public SetAddressAsPrimaryCommandHandler(
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the SetAddressAsPrimaryCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(
        SetAddressAsPrimaryCommand request,
        CancellationToken cancellationToken)
    {
        // Resolve AddressKey to Address entity using GetAsync
        var address = await _addressRepository.GetAsync(
            new AlternateKey(request.AddressKey),
            cancellationToken);

        // Set as primary using the domain method
        address.SetAsPrimary();

        // Update the address in the repository
        await _addressRepository.UpdateAsync(address, cancellationToken);

        // Commit the transaction
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

