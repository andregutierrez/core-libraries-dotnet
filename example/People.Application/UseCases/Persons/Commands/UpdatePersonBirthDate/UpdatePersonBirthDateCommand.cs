namespace People.Application.UseCases.Persons.Commands.UpdatePersonBirthDate;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update a person's birth date.
/// </summary>
public class UpdatePersonBirthDateCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the person's birth date. Can be null to remove the birth date.
    /// </summary>
    public DateOnly? BirthDate { get; init; }
}

