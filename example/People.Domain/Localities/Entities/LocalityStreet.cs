namespace People.Domain.Localities.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Represents a cached street information from the locality microservice.
/// This aggregate serves as a fast cache within the People domain and is not intended for relational queries.
/// </summary>
public class LocalityStreet : Locality
{
    /// <summary>
    /// Gets the street name.
    /// </summary>
    public string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected LocalityStreet() { }

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityStreet"/>.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The street name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
    private LocalityStreet(AlternateKey key, string name, string? metadata = null)
        : base(LocalityType.Street)
    {
        ArgumentNullException.ThrowIfNull(key);
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Street name cannot be null or whitespace.", nameof(name));

        Key = key;
        Name = name.Trim();
        Metadata = metadata;
    }

    /// <summary>
    /// Creates a new street locality cache entry from the locality microservice.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The street name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityStreet"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityStreet Create(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityStreet(key, name, metadata);
    }

    /// <summary>
    /// Imports a street locality with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system.</param>
    /// <param name="name">The street name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityStreet"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityStreet Import(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityStreet(key, name, metadata);
    }

    /// <summary>
    /// Updates the cached street information.
    /// </summary>
    /// <param name="name">The street name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    public void UpdateCache(string name, string? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Street name cannot be null or whitespace.", nameof(name));

        Name = name.Trim();
        Metadata = metadata;
        CachedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Returns the street name as a string.
    /// </summary>
    public override string ToString() => Name;
}

