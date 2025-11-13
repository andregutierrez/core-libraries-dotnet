namespace People.Application.UseCases.Localities.Commands.DeleteLocality;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesDomain.Exceptions;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Localities.Services.Repositories;

/// <summary>
/// Handler for the DeleteLocalityCommand.
/// </summary>
public class DeleteLocalityCommandHandler : ICommandHandler<DeleteLocalityCommand>
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteLocalityCommandHandler"/>.
    /// </summary>
    /// <param name="localityRepository">The locality repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public DeleteLocalityCommandHandler(
        ILocalityRepository localityRepository,
        IUnitOfWork unitOfWork)
    {
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the DeleteLocalityCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the locality is not found.</exception>
    public async Task Handle(
        DeleteLocalityCommand request,
        CancellationToken cancellationToken)
    {
        var locality = await _localityRepository.GetAsync(
            new AlternateKey(request.LocalityKey),
            cancellationToken);

        await _localityRepository.DeleteAsync(locality, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

