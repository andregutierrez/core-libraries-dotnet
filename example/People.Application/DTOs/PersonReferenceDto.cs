namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing a minimal reference to a person.
/// Used when an address needs to reference its owner person with minimal information.
/// </summary>
public class PersonReferenceDto
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }

    /// <summary>
    /// Gets the person's name.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}

