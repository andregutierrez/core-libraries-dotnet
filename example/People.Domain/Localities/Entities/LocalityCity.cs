namespace People.Domain.Localities.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Represents a cached city information from the locality microservice.
/// This aggregate serves as a fast cache within the People domain and is not intended for relational queries.
/// </summary>
public class LocalityCity : Locality
{
    /// <summary>
    /// Gets the city name.
    /// </summary>
    public string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected LocalityCity() { }

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityCity"/>.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The city name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
    private LocalityCity(AlternateKey key, string name, string? metadata = null)
        : base(LocalityType.City)
    {
        ArgumentNullException.ThrowIfNull(key);
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("City name cannot be null or whitespace.", nameof(name));

        Key = key;
        Name = name.Trim();
        Metadata = metadata;
    }

    /// <summary>
    /// Creates a new city locality cache entry from the locality microservice.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The city name.</param>
    /// <param name="code">An optional city code (e.g., IBGE code).</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityCity"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityCity Create(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityCity(key, name, metadata);
    }

    /// <summary>
    /// Imports a city locality with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system.</param>
    /// <param name="name">The city name.</param>
    /// <param name="code">An optional city code (e.g., IBGE code).</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityCity"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityCity Import(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityCity(key, name, metadata);
    }

    /// <summary>
    /// Updates the cached city information.
    /// </summary>
    /// <param name="name">The city name.</param>
    /// <param name="code">An optional city code.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    public void UpdateCache(string name, string? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("City name cannot be null or whitespace.", nameof(name));

        Name = name.Trim();
        Metadata = metadata;
        CachedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Returns the city name as a string.
    /// </summary>
    public override string ToString() => Name;
}
