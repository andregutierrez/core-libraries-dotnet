namespace People.Application.UseCases.Persons.Commands.ImportPerson;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to import a person from an external system with a specific alternate key.
/// </summary>
public record ImportPersonCommand : BaseCommand<ImportPersonCommandResponse>
{
    /// <summary>
    /// Gets the person's alternate key from the external system.
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

    /// <summary>
    /// Gets the person's birth date, if any.
    /// </summary>
    public DateOnly? BirthDate { get; init; }

    /// <summary>
    /// Gets the person's gender code, if any.
    /// </summary>
    public int? GenderCode { get; init; }
}

/// <summary>
/// Response for the ImportPersonCommand.
/// </summary>
public class ImportPersonCommandResponse
{
    /// <summary>
    /// Gets the imported person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }
}

