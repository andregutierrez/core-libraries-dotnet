namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents a filter criteria for entity identifiers.
/// Used to filter entities by identifier type and value.
/// </summary>
public record IdentifierFilterDto
{
    /// <summary>
    /// Gets or sets the identifier type code (e.g., CPF, CNPJ, Email).
    /// Corresponds to the <see cref="IdentifierType"/> enumeration value.
    /// </summary>
    public int? Type { get; init; }

    /// <summary>
    /// Gets or sets the identifier value to filter by.
    /// The format depends on the identifier type (e.g., "123.456.789-00" for CPF).
    /// </summary>
    public string? Value { get; init; }
}
