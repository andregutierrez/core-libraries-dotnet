namespace People.Domain.Localities.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Represents a cached locality information from the locality microservice.
/// This aggregate serves as a fast cache within the People domain and is not intended for relational queries.
/// </summary>
public abstract class Locality : Aggregate<EntityId>, IHasAlternateKey
{
    /// <summary>
    /// Gets the alternate key for this locality cache.
    /// This key comes from the locality microservice.
    /// </summary>
    public AlternateKey Key { get; protected set; } = null!;

    /// <summary>
    /// Gets the type of this locality (City, Country, State, Neighborhood, Street).
    /// </summary>
    public LocalityType Type { get; protected set; } = null!;

    /// <summary>
    /// Gets additional metadata from the locality service (JSON or serialized data).
    /// </summary>
    public string? Metadata { get; protected set; }

    /// <summary>
    /// Gets the timestamp when this cache entry was created or last updated.
    /// </summary>
    public DateTime CachedAt { get; protected set; }

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected Locality()
    {
        Key = default!;
        Type = default!;
        CachedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Locality"/>.
    /// </summary>
    /// <param name="type">The type of this locality.</param>
    /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
    protected Locality(LocalityType type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Type = type;
        CachedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets a value indicating whether this cache entry is stale (older than specified days).
    /// </summary>
    /// <param name="maxAgeDays">Maximum age in days before considering stale. Defaults to 30 days.</param>
    /// <returns><c>true</c> if the cache is stale; otherwise, <c>false</c>.</returns>
    public bool IsStale(int maxAgeDays = 30)
    {
        var age = DateTime.UtcNow - CachedAt;
        return age.TotalDays > maxAgeDays;
    }
}

