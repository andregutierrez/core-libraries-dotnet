namespace People.Application.UseCases.Addresses.Commands.UpdateAddressNotes;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Addresses.Services.Repositories;

/// <summary>
/// Handler for the UpdateAddressNotesCommand.
/// </summary>
public class UpdateAddressNotesCommandHandler : ICommandHandler<UpdateAddressNotesCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateAddressNotesCommandHandler"/>.
    /// </summary>
    /// <param name="addressRepository">The address repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdateAddressNotesCommandHandler(
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the UpdateAddressNotesCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(
        UpdateAddressNotesCommand request,
        CancellationToken cancellationToken)
    {
        // Resolve AddressKey to Address entity using GetAsync
        var address = await _addressRepository.GetAsync(
            new AlternateKey(request.AddressKey),
            cancellationToken);

        // Update the notes using the domain method
        address.UpdateNotes(request.Notes);

        // Update the address in the repository
        await _addressRepository.UpdateAsync(address, cancellationToken);

        // Commit the transaction
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

