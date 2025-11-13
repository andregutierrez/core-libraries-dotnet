namespace People.Application.UseCases.Localities.Commands.ImportLocality;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Localities.Entities;
using People.Domain.Localities.Services.Repositories;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Handler for the ImportLocalityCommand.
/// </summary>
public class ImportLocalityCommandHandler : ICommandHandler<ImportLocalityCommand, ImportLocalityCommandResponse>
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="ImportLocalityCommandHandler"/>.
    /// </summary>
    /// <param name="localityRepository">The locality repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public ImportLocalityCommandHandler(
        ILocalityRepository localityRepository,
        IUnitOfWork unitOfWork)
    {
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the ImportLocalityCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response containing the imported locality's alternate key.</returns>
    public async Task<ImportLocalityCommandResponse> Handle(
        ImportLocalityCommand request,
        CancellationToken cancellationToken)
    {
        var localityType = LocalityType.FromCode(request.LocalityTypeCode);
        if (localityType == null)
            throw new ArgumentException($"Invalid locality type code: {request.LocalityTypeCode}", nameof(request));

        var key = new AlternateKey(request.LocalityKey);

        // Check if locality already exists
        var existing = await _localityRepository.FindAsync(key, cancellationToken);
        if (existing != null)
        {
            // Update existing cache entry
            switch (existing)
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
                    throw new InvalidOperationException($"Unsupported locality type: {existing.GetType().Name}");
            }

            await _localityRepository.UpdateAsync(existing, cancellationToken);
        }
        else
        {
            // Create new cache entry
            Locality locality = localityType.Code switch
            {
                1 => LocalityCity.Import(key, request.Name, request.Metadata),
                2 => LocalityCountry.Import(key, request.Name, request.Metadata),
                3 => LocalityState.Import(key, request.Name, request.Metadata),
                4 => LocalityNeighborhood.Import(key, request.Name, request.Metadata),
                5 => LocalityStreet.Import(key, request.Name, request.Metadata),
                _ => throw new ArgumentException($"Unsupported locality type code: {request.LocalityTypeCode}", nameof(request))
            };

            await _localityRepository.InsertAsync(locality, cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return new ImportLocalityCommandResponse
        {
            LocalityKey = request.LocalityKey
        };
    }
}

