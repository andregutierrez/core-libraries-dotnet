namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Defines the abstraction for a status history in domain entities, providing methods to 
/// query the active status and retrieve entries by type.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the status entity, derived from <see cref="Status{TStatus}"/>.
/// </typeparam>
/// <typeparam name="TStatus">
/// Enum type representing the possible status values.
/// </typeparam>
public interface IStatusHistory<TStatus> : IReadOnlyCollection<TStatus> 
{
    /// <summary>
    /// Retrieves all status records that match the specified type.
    /// </summary>
    /// <param name="type">The enum value to filter by.</param>
    /// <returns>
    /// A sequence of status entities where <see cref="Status{TStatus}.Type"/>
    /// matches the specified <paramref name="type"/>.
    /// </returns>
    IEnumerable<TStatus> GetByType<TType>(TType type);

    /// <summary>
    /// Retrieves the currently active status record.
    /// </summary>
    /// <returns>
    /// An instance of <typeparamref name="TEntity"/> with <c>Active == true</c>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if there is not exactly one active status.
    /// </exception>
    TStatus GetCurrent();
}
