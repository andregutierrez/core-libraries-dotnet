using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;
using Core.Libraries.Domain.Exceptions;
using Core.Libraries.Domain.Services.Repositories;

namespace Core.Libraries.Infra.Data.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TKey : IEntityId
    where TEntity : Entity<TKey>
{
    private readonly DbContext _dbContext;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected DbContext Context => _dbContext;

    public async Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<TEntity, object?>>[] propertySelectors)
        => await WithDetails(propertySelectors)
            .Where(o => o.Id.Equals(id))
            .SingleOrDefaultAsync();

    public async Task<TEntity?> FindAsync(Guid alternateKey, CancellationToken cancellationToken, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        if (typeof(IHasAlternateKey).IsAssignableFrom(typeof(TEntity)))
        {
            return await WithDetails(propertySelectors)
                .Where(e => ((IHasAlternateKey)e).Key.Value == alternateKey)
                .ApplySoftDeleteFilter()
                .SingleOrDefaultAsync();
        }
        throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} does not implement IHasAlternateKey.");
    }


    public async Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        => await _dbContext
            .Set<TEntity>()
            .Where(predicate)
            .ApplySoftDeleteFilter()
            .SingleOrDefaultAsync();

    public async Task<TEntity?> FindAsync(TKey id)
    => await _dbContext
        .Set<TEntity>()
        .Where(e => e.Id.Equals(id))
        .ApplySoftDeleteFilter()
        .SingleOrDefaultAsync();

    public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<TEntity, object?>>[] propertySelectors)
        => await FindAsync(id, cancellationToken, propertySelectors)
            ?? throw new EntityNotFoundException(typeof(TEntity).Name, new { id });

    public async Task<TEntity> GetAsync(Guid key, CancellationToken cancellationToken, params Expression<Func<TEntity, object?>>[] propertySelectors)
        => await FindAsync(key, propertySelectors)
            ?? throw new EntityNotFoundException(typeof(TEntity).Name, new { key });

    public async Task<TKey> GetIdAsync(CancellationToken cancellationToken, Guid alternateKey)
    {
        if (!typeof(IHasAlternateKey).IsAssignableFrom(typeof(TEntity)))
            throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} does not implement IHasAlternateKey.");

        return await _dbContext
            .Set<TEntity>()
            .Where(e => ((IHasAlternateKey)e).Key.Value == alternateKey)
            .ApplySoftDeleteFilter()
            .Select(e => e.Id)
            .SingleAsync();
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool includeDetails = true)
        => await FindAsync(cancellationToken, predicate)
            ?? throw new EntityNotFoundException(typeof(TEntity).Name, new { predicate });

    public virtual async Task<TEntity> GetAsync(TKey id)
        => await FindAsync(id)
            ?? throw new EntityNotFoundException(typeof(TEntity).Name, new { id });

    public virtual async Task<TEntity> GetAsync(Guid key)
        => await FindAsync(key)
            ?? throw new EntityNotFoundException(typeof(TEntity).Name, new { key });

    public async Task<TEntity?> FindAsync(Guid key)
    {
        if (typeof(IHasAlternateKey).IsAssignableFrom(typeof(TEntity)))
        {
            return await _dbContext
                .Set<TEntity>()
                .Where(e => ((IHasAlternateKey)e).Key.Value == key)
                .ApplySoftDeleteFilter()
                .SingleOrDefaultAsync();
        }
        throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} does not implement IHasAlternateKey.");
    }

    public async Task<IEnumerable<TEntity>> GetListAsync()
        => await _dbContext
            .Set<TEntity>()
            .ApplySoftDeleteFilter()
            .ToListAsync();

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
         => await _dbContext
            .Set<TEntity>()
            .Where(predicate)
            .ApplySoftDeleteFilter()
            .ToListAsync();

    public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting)
        => await _dbContext
            .Set<TEntity>()
            .ApplySoftDeleteFilter()
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();

    public IQueryable<TEntity> GetQueryable()
        => _dbContext
            .Set<TEntity>()
            .ApplySoftDeleteFilter()
            .AsQueryable();

    public IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object?>>[] propertySelectors)
        => IncludeDetails(GetQueryable(), propertySelectors);

    public async Task<bool> ExistsAsync(TKey id)
        => await GetQueryable()
            .Where(o => o.Id.Equals(id))
            .ApplySoftDeleteFilter()
            .AsNoTracking()
            .AnyAsync();

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        => await GetQueryable()
            .Where(predicate)
            .ApplySoftDeleteFilter()
            .AsNoTracking()
            .AnyAsync();

    public async Task InsertAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        var groupsValidatedHashEntites = entities
            .GroupBy(e => e.GetType());

        await _dbContext.Set<TEntity>().AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        if (entity is IAuditable auditableEntity)
            auditableEntity?.Audit?.MarkAsModified();

        _dbContext.Attach(entity);
        _dbContext.Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            if (entity is IAuditable auditableEntity)
                auditableEntity?.Audit?.MarkAsModified();

        _dbContext.Set<TEntity>().UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        if (entity is ISoftDeletable softDeleteEntity)
        {
            softDeleteEntity?.Deletion?.MarkAsDeleted();
            _dbContext
                .Set<TEntity>()
                .Update(entity);
        }
        else
        {
            _dbContext
                .Set<TEntity>()
                .Remove(entity);
        }
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        var entitiesList = entities.ToList();
        var softDeleteEntities = entitiesList.OfType<ISoftDeletable>().Cast<TEntity>().ToList();
        var hardDeleteEntities = entitiesList.Except(softDeleteEntities).ToList();

        foreach (var entity in softDeleteEntities)
        {
            if (entity == null) continue;

            if (entity is IAuditable auditableEntity)
                auditableEntity.Audit?.MarkAsModified();

            if (entity is ISoftDeletable softDeleteEntity)
                softDeleteEntity.Deletion?.MarkAsDeleted();

            _dbContext.Set<TEntity>().Update(entity);
        }

        if (hardDeleteEntities.Any())
        {
            _dbContext.Set<TEntity>().RemoveRange(hardDeleteEntities);
        }
    }

    private static IQueryable<TEntity> IncludeDetails(IQueryable<TEntity> query, Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        if (propertySelectors != null)
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
        }

        return query;
    }

    public Task<TEntity> GetAsync(AlternateKey alternateKey)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(AlternateKey alternateKey, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAsync(AlternateKey alternateKey)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAsync(AlternateKey alternateKey, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, PaginationOptions pagination)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, PaginationOptions pagination, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(AlternateKey alternateKey)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(AlternateKey alternateKey, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(AlternateKey alternateKey, CancellationToken cancellationToken, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAsync(AlternateKey alternateKey, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAsync(AlternateKey alternateKey, CancellationToken cancellationToken, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, PaginationOptions pagination, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, PaginationOptions pagination, CancellationToken cancellationToken, params Expression<Func<TEntity, object?>>[] propertySelectors)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(AlternateKey alternateKey, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplySoftDeleteFilter<TEntity>(this IQueryable<TEntity> query)
        where TEntity : class
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var statusProperty = Expression.Property(parameter, nameof(ISoftDeletable.Deletion.IsDeleted));
            //var ativoProperty = Expression.Property(statusProperty, nameof(SimpleStatus.Ativo));
            //var ativoCondition = Expression.Equal(ativoProperty, Expression.Constant(true));
            //var lambda = Expression.Lambda<Func<TEntity, bool>>(ativoCondition, parameter);

            //return query.Where(lambda);
        }

        return query;
    }
}
