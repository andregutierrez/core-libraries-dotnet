namespace People.Application.UseCases.Localities.Commands.UpdateLocalityCache;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update a locality cache entry.
/// </summary>
public class UpdateLocalityCacheCommand : BaseCommand
{
    /// <summary>
    /// Gets the locality's alternate key.
    /// </summary>
    public Guid LocalityKey { get; init; }

    /// <summary>
    /// Gets the updated locality name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets additional metadata from the locality service (JSON or serialized data). Optional.
    /// </summary>
    public string? Metadata { get; init; }
}

