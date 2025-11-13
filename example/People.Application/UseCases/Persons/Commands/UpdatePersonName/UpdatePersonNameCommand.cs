namespace People.Application.UseCases.Persons.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update a person's name.
/// </summary>
public class UpdatePersonNameCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the person's first name.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the person's last name.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the person's middle name, if any.
    /// </summary>
    public string? MiddleName { get; init; }

    /// <summary>
    /// Gets the person's social name (nome social), if any.
    /// </summary>
    public string? SocialName { get; init; }
}

