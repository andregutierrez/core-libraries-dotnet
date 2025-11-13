namespace People.Application.UseCases.Persons.Commands;

using Core.LibrariesApplication.Commands;

/// <summary>
/// Command to deactivate a person (sets status to Inactive).
/// </summary>
public class DeactivatePersonCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets optional notes explaining the deactivation.
    /// </summary>
    public string? Notes { get; init; }
}

