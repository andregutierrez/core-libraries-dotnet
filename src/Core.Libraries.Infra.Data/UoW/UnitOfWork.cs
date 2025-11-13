using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Core.Libraries.Domain.Services.Repositories;

namespace Core.Libraries.Infra.Data.UoW;

/// <summary>
/// Implements the Unit of Work pattern using EF Core.
/// Manages transaction boundaries and ensures consistent SaveChanges/Commit behavior.
/// </summary>
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DbContext _dbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Begins a new database transaction if none exists.
    /// </summary>
    public async Task BeginAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            return;

        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction after saving changes.
    /// </summary>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction started.");

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        await DisposeAsync();
    }

    /// <summary>
    /// Rolls back the current transaction, if one is active.
    /// </summary>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await DisposeAsync();
    }

    /// <summary>
    /// Disposes the transaction and DbContext resources.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _dbContext.Dispose();
    }

    /// <summary>
    /// Internal helper to dispose the current transaction and set it to null.
    /// </summary>
    private async Task DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
