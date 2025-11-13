using System.Diagnostics;

namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents a data transfer object for entity identifiers.
/// Used for transferring entity identifiers between application layers or services.
/// </summary>
[DebuggerDisplay("{Value}")]
public record EntityIdDto
{
    /// <summary>
    /// Gets or sets the integer value of the entity identifier.
    /// Defaults to 0 if not initialized.
    /// </summary>
    public int Value { get; init; }
}
