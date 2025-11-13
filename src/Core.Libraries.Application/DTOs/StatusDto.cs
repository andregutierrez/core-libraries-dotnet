namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents a basic status model with an activation flag.
/// </summary>
public record StatusDto
{
    /// <summary>
    /// Indicates whether the entity is active.
    /// </summary>
    public bool IsActive { get; init; }
}
