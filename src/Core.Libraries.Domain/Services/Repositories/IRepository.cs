using System.Linq.Expressions;
using Core.Libraries.Domain.Entities;


namespace Core.LibrariesDomain.Services.Repositories;

/// <summary>
/// Interface for a generic repository that manages operations for entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public interface IRepository<TEntity, TKey>
    where TKey : IEntityId
    where TEntity : Entity<TKey>
{
    /// <summary>
    /// Retrieves an entity by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to the entity instance.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// Thrown if the entity is not found.
    /// </exception>
    /// <remarks>
    /// This method throws if the entity is not found. Use <c>FindAsync</c> to return <c>null</c> instead.
    /// </remarks>
    Task<TEntity> GetAsync(TKey id);

    /// <summary>
    /// Retrieves an entity using its globally unique alternate key.
    /// </summary>
    /// <param name="alternateKey">The alternate key used to identify the entity externally.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to the entity instance.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// Thrown if the entity is not found.
    /// </exception>
    /// <remarks>
    /// This is useful when referencing entities via external systems or APIs using a GUID-based key.
    /// </remarks>
    Task<TEntity> GetAsync(AlternateKey alternateKey);

    /// <summary>
    /// Retrieves an entity by its primary key including related navigation properties.
    /// </summary>
    /// <param name="id">The primary key of the entity.</param>
    /// <param name="propertySelectors">Navigation properties to include in the query (e.g., lazy-loaded relationships).</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to the entity instance with specified related data.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// Thrown if the entity is not found.
    /// </exception>
    /// <example>
    /// <code>
    /// var order = await repository.GetAsync(orderId, x => x.Items, x => x.Customer);
    /// </code>
    /// </example>
    Task<TEntity> GetAsync(TKey id, params Expression<Func<TEntity, object?>>[] propertySelectors);

    /// <summary>
    /// Retrieves an entity by its alternate key including related navigation properties.
    /// </summary>
    /// <param name="alternateKey">The alternate key used to identify the entity externally.</param>
    /// <param name="propertySelectors">Navigation properties to include in the query.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to the entity instance with specified related data.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// Thrown if the entity is not found.
    /// </exception>
    /// <remarks>
    /// Use this overload when eager loading specific relationships is required for external lookups.
    /// </remarks>
    Task<TEntity> GetAsync(AlternateKey alternateKey, params Expression<Func<TEntity, object?>>[] propertySelectors);


    /// <summary>
    /// Asynchronously searches for an entity using its primary key.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains the entity instance if found; otherwise, <c>null</c>.
    /// </returns>
    /// <typeparamref name="TKey"/>
    /// <remarks>
    /// This method performs a lookup using the primary key value without tracking.
    /// </remarks>
    /// <example>
    /// <code>
    /// var entity = await repository.FindAsync(entityId);
    /// if (entity != null)
    /// {
    ///     // entity found
    /// }
    /// </code>
    /// </example>
    Task<TEntity?> FindAsync(TKey id);

    /// <summary>
    /// Asynchronously searches for an entity using an alternate key (external/global identifier).
    /// </summary>
    /// <param name="alternateKey">The alternate key representing the external or globally unique identifier.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains the entity instance if found; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Use this method when working with external systems or alternate indexing strategies.
    /// </remarks>
    /// <example>
    /// <code>
    /// var entity = await repository.FindAsync(new AlternateKey(externalGuid));
    /// </code>
    /// </example>
    Task<TEntity?> FindAsync(AlternateKey alternateKey);

    /// <summary>
    /// Asynchronously searches for an entity using its primary key, including related navigation properties.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="propertySelectors">Expressions to specify which related properties to include in the result.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains the entity instance with the specified related data if found; otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var entity = await repository.FindAsync(id, e => e.RelatedEntity, e => e.Metadata);
    /// </code>
    /// </example>
    Task<TEntity?> FindAsync(TKey id, params Expression<Func<TEntity, object?>>[] propertySelectors);

    /// <summary>
    /// Asynchronously searches for an entity using an alternate key, including related navigation properties.
    /// </summary>
    /// <param name="alternateKey">The alternate key representing the external or globally unique identifier.</param>
    /// <param name="propertySelectors">Expressions to specify which related properties to include in the result.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains the entity instance with the specified related data if found; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Useful for scenarios where eager loading of related data is necessary.
    /// </remarks>
    /// <example>
    /// <code>
    /// var entity = await repository.FindAsync(_alternateKey, e => e.Tags, e => e.AuditTrail);
    /// </code>
    /// </example>
    Task<TEntity?> FindAsync(AlternateKey alternateKey, params Expression<Func<TEntity, object?>>[] propertySelectors);


    /// <summary>
    /// Asynchronously retrieves all entities from the repository.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a collection of all entities.
    /// </returns>
    /// <example>
    /// <code>
    /// var allEntities = await repository.SearchAsync();
    /// </code>
    /// </example>
    Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy);

    /// <summary>
    /// Asynchronously retrieves all entities that match the specified filter.
    /// </summary>
    /// <param name="predicate">An expression used to filter entities.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a collection of entities matching the filter condition.
    /// </returns>
    /// <example>
    /// <code>
    /// var filtered = await repository.SearchAsync(e => e.IsActive);
    /// </code>
    /// </example>
    Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves a paginated list of entities that match the specified filter.
    /// </summary>
    /// <param name="predicate">An expression used to filter entities.</param>
    /// <param name="pagination">Pagination options including page number and page size.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a paginated collection of entities matching the filter condition.
    /// </returns>
    /// <example>
    /// <code>
    /// var paged = await repository.SearchAsync(e => e.Type == "internal", new PaginationOptions(2, 20));
    /// </code>
    /// </example>
    Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, PaginationOptions pagination);

    /// <summary>
    /// Asynchronously retrieves a paginated list of entities that match the specified filter and includes related navigation properties.
    /// </summary>
    /// <param name="predicate">An expression used to filter entities.</param>
    /// <param name="pagination">Pagination options including page number and page size.</param>
    /// <param name="propertySelectors">Expressions to specify which related properties to include.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a paginated collection of entities with related data.
    /// </returns>
    /// <remarks>
    /// This method supports eager loading and is useful for UI projections with complex graphs.
    /// </remarks>
    /// <example>
    /// <code>
    /// var pagedWithDetails = await repository.SearchAsync(e => e.Status == Status.Active, new PaginationOptions(1, 10), e => e.Profile, e => e.Logs);
    /// </code>
    /// </example>
    Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, PaginationOptions pagination, params Expression<Func<TEntity, object?>>[] propertySelectors);

    /// <summary>
    /// Asynchronously retrieves all entities that match the specified filter and includes related navigation properties.
    /// </summary>
    /// <param name="predicate">An expression used to filter entities.</param>
    /// <param name="propertySelectors">Expressions to specify which related properties to include.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a collection of entities matching the filter with related data included.
    /// </returns>
    /// <example>
    /// <code>
    /// var entitiesWithDetails = await repository.SearchAsync(e => e.IsDeleted == false, e => e.AuditTrail, e => e.Metadata);
    /// </code>
    /// </example>
    Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] propertySelectors);


    /// <summary>
    /// Asynchronously inserts a new entity into the repository.
    /// </summary>
    /// <param name="entity">The entity instance to be inserted.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This operation persists the entity into the underlying data store but does not commit the transaction automatically.
    /// Use <c>IUnitOfWork.CommitAsync()</c> to finalize the changes.
    /// </remarks>
    /// <example>
    /// <code>
    /// await repository.InsertAsync(new Customer("Alice"));
    /// </code>
    /// </example>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// Asynchronously inserts multiple new entities into the repository.
    /// </summary>
    /// <param name="entities">A collection of entities to be inserted.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Useful for bulk-insert operations. Does not commit automatically—pair with unit of work pattern for transaction scope.
    /// </remarks>
    /// <example>
    /// <code>
    /// await repository.InsertAsync(new[] { new Product("P1"), new Product("P2") });
    /// </code>
    /// </example>
    Task InsertAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Asynchronously updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity instance with updated values.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Assumes the entity is already tracked or properly attached to the context. 
    /// If not, you may need to explicitly attach it before calling this method.
    /// </remarks>
    /// <example>
    /// <code>
    /// customer.Name = "Updated Name";
    /// await repository.UpdateAsync(customer);
    /// </code>
    /// </example>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Asynchronously updates a collection of entities in the repository.
    /// </summary>
    /// <param name="entities">A collection of entities with updated values.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Efficient for batch updates. Does not commit automatically.
    /// </remarks>
    /// <example>
    /// <code>
    /// foreach (var item in orders)
    ///     item.Status = Status.Shipped;
    /// await repository.UpdateAsync(orders);
    /// </code>
    /// </example>
    Task UpdateAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Asynchronously marks an entity for deletion from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Depending on implementation, this may either physically remove the entity or set a soft-delete flag.
    /// </remarks>
    /// <example>
    /// <code>
    /// await repository.DeleteAsync(order);
    /// </code>
    /// </example>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Asynchronously marks multiple entities for deletion from the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Use with caution in production scenarios where cascade deletes or referential integrity applies.
    /// </remarks>
    /// <example>
    /// <code>
    /// await repository.DeleteAsync(expiredSessions);
    /// </code>
    /// </example>
    Task DeleteAsync(IEnumerable<TEntity> entities);


    /// <summary>
    /// Asynchronously checks whether an entity with the specified primary key exists in the repository.
    /// </summary>
    /// <param name="id">The primary key identifier of the entity.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> whose result is <c>true</c> if the entity exists; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method performs a lightweight check without loading the full entity data from the underlying data store.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool exists = await repository.ExistsAsync(new CustomerId(123));
    /// </code>
    /// </example>
    Task<bool> ExistsAsync(TKey id);

    /// <summary>
    /// Asynchronously checks whether an entity with the specified alternate key exists in the repository.
    /// </summary>
    /// <param name="alternateKey">The alternate (external) key of the entity.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> whose result is <c>true</c> if the entity exists; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Alternate keys are often used for external integrations or UUID-based lookups where a primary key is not exposed.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool exists = await repository.ExistsAsync(new AlternateKey(Guid.Parse("fc7b...")));
    /// </code>
    /// </example>
    Task<bool> ExistsAsync(AlternateKey alternateKey);

    /// <summary>
    /// Asynchronously checks whether any entity matches the specified predicate.
    /// </summary>
    /// <param name="predicate">A LINQ expression that defines the match condition for the entity.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> whose result is <c>true</c> if any entity matches the condition; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Use this method to validate the existence of entities based on domain rules or filters.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool emailExists = await repository.ExistsAsync(u => u.Email == "user@domain.com");
    /// </code>
    /// </example>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Asynchronously counts the number of entities in the repository that satisfy the given condition.
    /// </summary>
    /// <param name="predicate">
    /// A LINQ expression that defines the condition to be matched. 
    /// Only entities matching this condition will be counted.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> whose result is the number of entities that match the specified predicate.
    /// </returns>
    /// <remarks>
    /// This method is optimized to run a count query directly on the data source without loading entity data into memory.
    /// It is suitable for pagination, validation rules, and reporting metrics.
    /// </remarks>
    /// <example>
    /// <code>
    /// int activeUsers = await repository.CountAsync(user => user.IsActive);
    /// </code>
    /// </example>
    /// <seealso cref="SearchAsync"/>
    /// <seealso cref="ExistsAsync(Expression{Func{TEntity, bool}})"/>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    /// Returns a queryable interface over the entity set, enabling deferred execution and further composition using LINQ.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{TEntity}"/> that represents the entity collection in the current repository context.
    /// </returns>
    /// <remarks>
    /// This method does not automatically include related data. It is useful when custom queries, projections, or filters are needed.
    /// </remarks>
    /// <example>
    /// <code>
    /// var recentItems = repository.GetQueryable()
    ///     .Where(e => e.CreatedAt > DateTime.UtcNow.AddDays(-30))
    ///     .OrderByDescending(e => e.CreatedAt);
    /// </code>
    /// </example>
    /// <seealso cref="WithDetails(Expression{Func{TEntity, object}}[])"/>
    IQueryable<TEntity> GetQueryable();


    /// <summary>
    /// Returns a queryable interface over the entity set with the specified related entities eagerly loaded.
    /// </summary>
    /// <param name="propertySelectors">
    /// A set of expressions indicating the navigation properties to include in the query (e.g., <c>x => x.RelatedEntity</c>).
    /// </param>
    /// <returns>
    /// An <see cref="IQueryable{TEntity}"/> that includes the specified navigation properties.
    /// </returns>
    /// <remarks>
    /// This method is useful when you need to retrieve an entity along with its related data to avoid the N+1 query problem.
    /// </remarks>
    /// <example>
    /// <code>
    /// var orders = repository.WithDetails(o => o.Customer, o => o.Items)
    ///     .Where(o => o.Status == OrderStatus.Processing)
    ///     .ToList();
    /// </code>
    /// </example>
    /// <seealso cref="GetQueryable"/>
    IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object?>>[] propertySelectors);

}
