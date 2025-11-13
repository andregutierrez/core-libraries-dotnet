namespace People.Application.UseCases.Persons.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to update a person's gender.
/// </summary>
public class UpdatePersonGenderCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the person's gender code. Can be null to remove the gender.
    /// </summary>
    public int? GenderCode { get; init; }
}

