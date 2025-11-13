namespace People.Domain.Localities.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Represents a cached neighborhood/district information from the locality microservice.
/// This aggregate serves as a fast cache within the People domain and is not intended for relational queries.
/// </summary>
public class LocalityNeighborhood : Locality
{
    /// <summary>
    /// Gets the neighborhood/district name.
    /// </summary>
    public string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected LocalityNeighborhood() { }

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityNeighborhood"/>.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The neighborhood/district name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
    private LocalityNeighborhood(AlternateKey key, string name, string? metadata = null)
        : base(LocalityType.Neighborhood)
    {
        ArgumentNullException.ThrowIfNull(key);
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Neighborhood name cannot be null or whitespace.", nameof(name));

        Key = key;
        Name = name.Trim();
        Metadata = metadata;
    }

    /// <summary>
    /// Creates a new neighborhood/district locality cache entry from the locality microservice.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The neighborhood/district name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityNeighborhood"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityNeighborhood Create(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityNeighborhood(key, name, metadata);
    }

    /// <summary>
    /// Imports a neighborhood/district locality with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system.</param>
    /// <param name="name">The neighborhood/district name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityNeighborhood"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityNeighborhood Import(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityNeighborhood(key, name, metadata);
    }

    /// <summary>
    /// Updates the cached neighborhood/district information.
    /// </summary>
    /// <param name="name">The neighborhood/district name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    public void UpdateCache(string name, string? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Neighborhood name cannot be null or whitespace.", nameof(name));

        Name = name.Trim();
        Metadata = metadata;
        CachedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Returns the neighborhood/district name as a string.
    /// </summary>
    public override string ToString() => Name;
}

