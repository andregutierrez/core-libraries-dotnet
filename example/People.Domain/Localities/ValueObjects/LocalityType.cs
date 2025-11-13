namespace People.Domain.Localities.ValueObjects;

using Core.Libraries.Domain.ValueObjects;

/// <summary>
/// Represents the type of a locality (City, Country, State, Neighborhood, Street).
/// </summary>
public class LocalityType : Enumeration
{
    /// <summary>
    /// Represents a city locality.
    /// </summary>
    public static readonly LocalityType City = new(1, "City");

    /// <summary>
    /// Represents a country locality.
    /// </summary>
    public static readonly LocalityType Country = new(2, "Country");

    /// <summary>
    /// Represents a state/province locality.
    /// </summary>
    public static readonly LocalityType State = new(3, "State");

    /// <summary>
    /// Represents a neighborhood/district locality.
    /// </summary>
    public static readonly LocalityType Neighborhood = new(4, "Neighborhood");

    /// <summary>
    /// Represents a street locality.
    /// </summary>
    public static readonly LocalityType Street = new(5, "Street");

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected LocalityType() : base(0, string.Empty) { }

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityType"/>.
    /// </summary>
    /// <param name="code">The unique code for this locality type.</param>
    /// <param name="name">The name of this locality type.</param>
    private LocalityType(int code, string name)
        : base(code, name)
    {
    }

    /// <summary>
    /// Gets a locality type by its code.
    /// </summary>
    /// <param name="code">The code of the locality type.</param>
    /// <returns>The locality type with the specified code, or <c>null</c> if not found.</returns>
    public static LocalityType? FromCode(int code)
        => GetAll<LocalityType>().FirstOrDefault(x => x.Code == code);

    /// <summary>
    /// Gets a locality type by its name.
    /// </summary>
    /// <param name="name">The name of the locality type.</param>
    /// <returns>The locality type with the specified name, or <c>null</c> if not found.</returns>
    public static LocalityType? FromName(string name)
        => GetAll<LocalityType>().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}

