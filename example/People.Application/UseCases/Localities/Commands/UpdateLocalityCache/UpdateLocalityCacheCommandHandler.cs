namespace People.Application.UseCases.Localities.Commands.UpdateLocalityCache;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesDomain.Exceptions;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Localities.Entities;
using People.Domain.Localities.Services.Repositories;

/// <summary>
/// Handler for the UpdateLocalityCacheCommand.
/// </summary>
public class UpdateLocalityCacheCommandHandler : ICommandHandler<UpdateLocalityCacheCommand>
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateLocalityCacheCommandHandler"/>.
    /// </summary>
    /// <param name="localityRepository">The locality repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdateLocalityCacheCommandHandler(
        ILocalityRepository localityRepository,
        IUnitOfWork unitOfWork)
    {
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the UpdateLocalityCacheCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the locality is not found.</exception>
    public async Task Handle(
        UpdateLocalityCacheCommand request,
        CancellationToken cancellationToken)
    {
        var locality = await _localityRepository.GetAsync(
            new AlternateKey(request.LocalityKey),
            cancellationToken);

        // UpdateCache is implemented in each derived type
        switch (locality)
        {
            case LocalityCity city:
                city.UpdateCache(request.Name, request.Metadata);
                break;
            case LocalityCountry country:
                country.UpdateCache(request.Name, request.Metadata);
                break;
            case LocalityState state:
                state.UpdateCache(request.Name, request.Metadata);
                break;
            case LocalityNeighborhood neighborhood:
                neighborhood.UpdateCache(request.Name, request.Metadata);
                break;
            case LocalityStreet street:
                street.UpdateCache(request.Name, request.Metadata);
                break;
            default:
                throw new InvalidOperationException($"Unsupported locality type: {locality.GetType().Name}");
        }

        await _localityRepository.UpdateAsync(locality, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

