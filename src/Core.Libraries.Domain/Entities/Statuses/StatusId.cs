using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Represents the identifier of a status within the domain,
/// extending <see cref="EntityId{T}"/> with an underlying <see cref="int"/> value.
/// </summary>
public sealed record StatusId : EntityId
{
    /// <summary>
    /// Initializes a new instance of <see cref="StatusId"/> with the given value.
    /// </summary>
    /// <param name="entityId">
    /// Integer value that uniquely identifies the status.
    /// </param>
    public StatusId(int entityId)
        : base(entityId) { }

    /// <summary>
    /// Implicitly converts an <see cref="int"/> into a <see cref="StatusId"/>,
    /// enabling direct assignment from integer literals.
    /// </summary>
    /// <param name="id">The integer value to convert.</param>
    public static implicit operator StatusId(int id)
        => new StatusId(id);

    /// <summary>
    /// Implicitly converts a <see cref="StatusId"/> into an <see cref="int"/>,
    /// returning the internal identifier value.
    /// </summary>
    /// <param name="entityId">The <see cref="StatusId"/> instance to convert.</param>
    public static implicit operator int(StatusId entityId)
        => entityId.Value;
}
