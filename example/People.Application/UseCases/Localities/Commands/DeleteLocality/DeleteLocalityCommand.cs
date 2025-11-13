namespace People.Application.UseCases.Localities.Commands.DeleteLocality;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to delete a locality cache entry.
/// </summary>
public class DeleteLocalityCommand : BaseCommand
{
    /// <summary>
    /// Gets the locality's alternate key.
    /// </summary>
    public Guid LocalityKey { get; init; }
}

