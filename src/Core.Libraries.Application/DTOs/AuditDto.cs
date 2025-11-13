namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents audit information for data transfer objects, including timestamps for creation and last modification.
/// </summary>
public record AuditDto
{
    /// <summary>
    /// The UTC timestamp when the entity was created.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    /// The UTC timestamp when the entity was last modified.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; init; }
}
