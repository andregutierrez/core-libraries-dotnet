namespace People.Application.UseCases.Persons.Commands.MarkPersonAsMerged;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to mark a person as merged (sets status to Merged).
/// </summary>
public class MarkPersonAsMergedCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets optional notes explaining the merge (e.g., target person key).
    /// </summary>
    public string? Notes { get; init; }
}

