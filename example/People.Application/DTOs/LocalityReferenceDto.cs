namespace People.Application.DTOs;

/// <summary>
/// Data transfer object representing a reference to a locality with its key and name.
/// Used when an address needs to reference a locality with minimal information.
/// </summary>
public class LocalityReferenceDto
{
    /// <summary>
    /// Gets the locality's alternate key.
    /// </summary>
    public Guid LocalityKey { get; init; }

    /// <summary>
    /// Gets the locality name.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}

