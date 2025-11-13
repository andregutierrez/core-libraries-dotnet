namespace People.Domain.Localities.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Represents a cached country information from the locality microservice.
/// This aggregate serves as a fast cache within the People domain and is not intended for relational queries.
/// </summary>
public class LocalityCountry : Locality
{
    /// <summary>
    /// Gets the country name.
    /// </summary>
    public string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected LocalityCountry() { }

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityCountry"/>.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The country name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
    private LocalityCountry(AlternateKey key, string name, string? metadata = null)
        : base(LocalityType.Country)
    {
        ArgumentNullException.ThrowIfNull(key);
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name cannot be null or whitespace.", nameof(name));

        Key = key;
        Name = name.Trim();
        Metadata = metadata;
    }

    /// <summary>
    /// Creates a new country locality cache entry from the locality microservice.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The country name.</param>
    /// <param name="code">An optional country code (e.g., ISO 3166-1 alpha-2: "BR", "US").</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityCountry"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityCountry Create(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityCountry(key, name, metadata);
    }

    /// <summary>
    /// Imports a country locality with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system.</param>
    /// <param name="name">The country name.</param>
    /// <param name="code">An optional country code (e.g., ISO 3166-1 alpha-2: "BR", "US").</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityCountry"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityCountry Import(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityCountry(key, name, metadata);
    }

    /// <summary>
    /// Updates the cached country information.
    /// </summary>
    /// <param name="name">The country name.</param>
    /// <param name="code">An optional country code.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    public void UpdateCache(string name, string? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name cannot be null or whitespace.", nameof(name));

        Name = name.Trim();
        Metadata = metadata;
        CachedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Returns the country name as a string.
    /// </summary>
    public override string ToString() => Name;
}
