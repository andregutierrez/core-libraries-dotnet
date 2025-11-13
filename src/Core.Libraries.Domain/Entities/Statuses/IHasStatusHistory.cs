namespace Core.Libraries.Domain.Entities.Statuses;

public interface IHasStatusHistory<TStatus>
    where TStatus : Enum
{
    /// <summary>
    /// Returns the collection as a read-only list.
    /// </summary>
    /// <returns>A read-only view of the collection.</returns>
    IStatusHistory<TStatus> StatusHistory { get; }
}