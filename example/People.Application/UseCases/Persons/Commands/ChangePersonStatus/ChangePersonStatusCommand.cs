namespace People.Application.UseCases.Persons.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to change a person's status.
/// </summary>
public class ChangePersonStatusCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the new status type (Active = 1, Inactive = 2, Merged = 3).
    /// </summary>
    public int StatusType { get; init; }

    /// <summary>
    /// Gets optional notes explaining the status change.
    /// </summary>
    public string? Notes { get; init; }
}

