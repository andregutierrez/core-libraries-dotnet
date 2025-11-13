namespace People.Application.UseCases.Localities.Commands.ImportLocality;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to import a locality cache entry with a specific alternate key.
/// </summary>
public record ImportLocalityCommand : BaseCommand<ImportLocalityCommandResponse>
{
    /// <summary>
    /// Gets the locality's alternate key from the external system.
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
/// Response for the ImportLocalityCommand.
/// </summary>
public class ImportLocalityCommandResponse
{
    /// <summary>
    /// Gets the imported locality's alternate key.
    /// </summary>
    public Guid LocalityKey { get; init; }
}

