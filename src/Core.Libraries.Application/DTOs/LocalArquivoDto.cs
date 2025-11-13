namespace Core.LibrariesApplication.DTOs;

/// <summary>
/// Represents a file location reference with its type information.
/// Used to transfer file metadata and location between application layers.
/// </summary>
public record FileLocationDto
{
    /// <summary>
    /// Gets or sets the file type information (e.g., image, document, audio).
    /// </summary>
    public EnumDto? Type { get; init; }

    /// <summary>
    /// Gets or sets the file path or URI where the file is located.
    /// Can be a relative path, absolute path, or a full URI.
    /// </summary>
    public string Path { get; init; } = string.Empty;
}
