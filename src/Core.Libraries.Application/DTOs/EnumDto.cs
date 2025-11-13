namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents an enumeration value with its numeric code and human-readable description,
/// commonly used in API responses.
/// </summary>
public record EnumDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumDto"/> class with the specified code and description.
    /// </summary>
    /// <param name="code">The numeric code associated with the enumeration value.</param>
    /// <param name="description">The human-readable description of the enumeration value.</param>
    public EnumDto(int code, string description)
    {
        Code = code;
        Description = description;
    }

    /// <summary>
    /// Gets or sets the numeric code associated with the enumeration.
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// Gets or sets the human-readable description of the enumeration value.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}
