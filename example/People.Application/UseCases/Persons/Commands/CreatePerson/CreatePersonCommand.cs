namespace People.Application.UseCases.Persons.Commands.CreatePerson;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to create a new person.
/// </summary>
public record CreatePersonCommand : BaseCommand<CreatePersonCommandResponse>
{
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
/// Response for the CreatePersonCommand.
/// </summary>
public class CreatePersonCommandResponse
{
    /// <summary>
    /// Gets the created person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }
}

