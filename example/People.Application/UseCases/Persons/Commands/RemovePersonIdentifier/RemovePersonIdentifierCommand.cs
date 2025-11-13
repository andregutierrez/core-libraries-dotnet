namespace People.Application.UseCases.Persons.Commands.RemovePersonIdentifier;

using Core.Libraries.Application.Commands;

/// <summary>
/// Command to remove an external system identifier from a person.
/// </summary>
public record RemovePersonIdentifierCommand : BaseCommand
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the identifier type code.
    /// </summary>
    public int IdentifierTypeCode { get; init; }
}

