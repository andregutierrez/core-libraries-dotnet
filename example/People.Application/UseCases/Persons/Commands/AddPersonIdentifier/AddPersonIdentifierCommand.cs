namespace People.Application.UseCases.Persons.Commands;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to add an external system identifier to a person.
/// </summary>
public class AddPersonIdentifierCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the identifier type code.
    /// </summary>
    public int IdentifierTypeCode { get; init; }

    /// <summary>
    /// Gets the identifier value in the external system.
    /// </summary>
    public string ExternalId { get; init; } = string.Empty;
}

