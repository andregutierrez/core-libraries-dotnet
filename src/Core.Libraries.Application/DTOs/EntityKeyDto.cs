using System.Diagnostics;
using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Application.DTOs;

/// <summary>
/// Represents a strongly-typed GUID used as a cross-system entity reference key.
/// </summary>
[DebuggerDisplay("{Value}")]
public readonly record struct EntityKeyDto : IEquatable<EntityKeyDto>
{
    /// <summary>
    /// Gets or sets the actual GUID key value.
    /// </summary>
    public Guid Value { get; init; }

    /// <summary>
    /// Implicitly converts a <see cref="Guid"/> to an <see cref="EntityKeyDto"/>.
    /// </summary>
    /// <param name="value">The GUID key value.</param>
    public static implicit operator EntityKeyDto(Guid value)
        => new() { Value = value };

    /// <summary>
    /// Implicitly converts a <see cref="AlternateKey"/> to an <see cref="EntityKeyDto"/>.
    /// </summary>
    /// <param name="value">The AlternateKey value.</param>
    public static implicit operator EntityKeyDto(AlternateKey alternateKey)
        => new() { Value = alternateKey.Value };

    /// <summary>
    /// Implicitly converts an <see cref="EntityKeyDto"/> to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="key">The entity key DTO.</param>
    public static implicit operator Guid(EntityKeyDto key) => key.Value;

    /// <inheritdoc />
    public override string ToString() => Value.ToString();

}
