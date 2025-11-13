namespace Core.Libraries.Domain.Services.Repositories;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Begins a new database transaction if none exists.
    /// </summary>
    Task BeginAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Commits the current transaction after saving changes.
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Rolls back the current transaction, if one is active.
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken);

}
