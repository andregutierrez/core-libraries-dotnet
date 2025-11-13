namespace People.Domain.Localities.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Localities.ValueObjects;

/// <summary>
/// Represents a cached state/province information from the locality microservice.
/// This aggregate serves as a fast cache within the People domain and is not intended for relational queries.
/// </summary>
public class LocalityState : Locality
{
    /// <summary>
    /// Gets the state/province name.
    /// </summary>
    public string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected LocalityState() { }

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityState"/>.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The state/province name.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
    private LocalityState(AlternateKey key, string name, string? metadata = null)
        : base(LocalityType.State)
    {
        ArgumentNullException.ThrowIfNull(key);
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("State name cannot be null or whitespace.", nameof(name));

        Key = key;
        Name = name.Trim();
        Metadata = metadata;
    }

    /// <summary>
    /// Creates a new state/province locality cache entry from the locality microservice.
    /// </summary>
    /// <param name="key">The alternate key from the locality microservice.</param>
    /// <param name="name">The state/province name.</param>
    /// <param name="code">An optional state code (e.g., "SP", "CA").</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityState"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityState Create(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityState(key, name, metadata);
    }

    /// <summary>
    /// Imports a state/province locality with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system.</param>
    /// <param name="name">The state/province name.</param>
    /// <param name="code">An optional state code (e.g., "SP", "CA").</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    /// <returns>A new instance of <see cref="LocalityState"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
    public static LocalityState Import(AlternateKey key, string name, string? metadata = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        return new LocalityState(key, name, metadata);
    }

    /// <summary>
    /// Updates the cached state/province information.
    /// </summary>
    /// <param name="name">The state/province name.</param>
    /// <param name="code">An optional state code.</param>
    /// <param name="metadata">Additional metadata. Optional.</param>
    public void UpdateCache(string name, string? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("State name cannot be null or whitespace.", nameof(name));

        Name = name.Trim();
        Metadata = metadata;
        CachedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Returns the state/province name as a string.
    /// </summary>
    public override string ToString() => Name;
}
