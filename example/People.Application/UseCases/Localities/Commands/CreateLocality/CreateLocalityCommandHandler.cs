namespace People.Application.UseCases.Localities.Commands.CreateLocality;

using Core.LibrariesApplication.Commands;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Exceptions;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Localities.Entities;
using People.Domain.Localities.Services.Repositories;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Handler for the CreateLocalityCommand.
/// </summary>
public class CreateLocalityCommandHandler : ICommandHandler<CreateLocalityCommand, CreateLocalityCommandResponse>
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateLocalityCommandHandler"/>.
    /// </summary>
    /// <param name="localityRepository">The locality repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public CreateLocalityCommandHandler(
        ILocalityRepository localityRepository,
        IUnitOfWork unitOfWork)
    {
        _localityRepository = localityRepository ?? throw new ArgumentNullException(nameof(localityRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the CreateLocalityCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response containing the created locality's alternate key.</returns>
    /// <exception cref="ArgumentException">Thrown when the locality type is invalid.</exception>
    public async Task<CreateLocalityCommandResponse> Handle(
        CreateLocalityCommand request,
        CancellationToken cancellationToken)
    {
        var localityType = LocalityType.FromCode(request.LocalityTypeCode);
        if (localityType == null)
            throw new ArgumentException($"Invalid locality type code: {request.LocalityTypeCode}", nameof(request));

        var key = new AlternateKey(request.LocalityKey);

        // Check if locality already exists
        var existing = await _localityRepository.FindAsync(key, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException($"A locality with key {request.LocalityKey} already exists.");

        Locality locality = localityType.Code switch
        {
            1 => LocalityCity.Create(key, request.Name, request.Metadata),
            2 => LocalityCountry.Create(key, request.Name, request.Metadata),
            3 => LocalityState.Create(key, request.Name, request.Metadata),
            4 => LocalityNeighborhood.Create(key, request.Name, request.Metadata),
            5 => LocalityStreet.Create(key, request.Name, request.Metadata),
            _ => throw new ArgumentException($"Unsupported locality type code: {request.LocalityTypeCode}", nameof(request))
        };

        await _localityRepository.InsertAsync(locality, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreateLocalityCommandResponse
        {
            LocalityKey = locality.Key.Value
        };
    }
}

