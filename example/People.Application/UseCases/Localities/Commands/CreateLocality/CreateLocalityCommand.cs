namespace People.Application.UseCases.Localities.Commands;

using Core.LibrariesApplication.Commands;

/// <summary>
/// Command to create a new locality cache entry.
/// </summary>
public class CreateLocalityCommand : BaseCommand<CreateLocalityCommandResponse>
{
    /// <summary>
    /// Gets the locality's alternate key from the locality microservice.
    /// </summary>
    public Guid LocalityKey { get; init; }

    /// <summary>
    /// Gets the type of locality (City = 1, Country = 2, State = 3, Neighborhood = 4, Street = 5).
    /// </summary>
    public int LocalityTypeCode { get; init; }

    /// <summary>
    /// Gets the locality name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets additional metadata from the locality service (JSON or serialized data). Optional.
    /// </summary>
    public string? Metadata { get; init; }
}

/// <summary>
/// Response for the CreateLocalityCommand.
/// </summary>
public class CreateLocalityCommandResponse
{
    /// <summary>
    /// Gets the created locality's alternate key.
    /// </summary>
    public Guid LocalityKey { get; init; }
}

