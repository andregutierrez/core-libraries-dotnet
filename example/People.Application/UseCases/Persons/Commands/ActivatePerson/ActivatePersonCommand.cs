namespace People.Application.UseCases.Persons.Commands;

using Core.LibrariesApplication.Commands;

/// <summary>
/// Command to activate a person (sets status to Active).
/// </summary>
public class ActivatePersonCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets optional notes explaining the activation.
    /// </summary>
    public string? Notes { get; init; }
}

