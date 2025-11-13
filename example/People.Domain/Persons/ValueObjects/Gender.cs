namespace People.Domain.Persons.ValueObjects;

using Core.Libraries.Domain.ValueObjects;

/// <summary>
/// Represents a person's gender.
/// </summary>
public class Gender : Enumeration
{
    /// <summary>
    /// Male gender.
    /// </summary>
    public static readonly Gender Male = new(1, "Male");

    /// <summary>
    /// Female gender.
    /// </summary>
    public static readonly Gender Female = new(2, "Female");

    /// <summary>
    /// Non-binary gender.
    /// </summary>
    public static readonly Gender NonBinary = new(3, "Non-Binary");

    /// <summary>
    /// Prefer not to say.
    /// </summary>
    public static readonly Gender PreferNotToSay = new(4, "Prefer Not to Say");

    /// <summary>
    /// Other gender.
    /// </summary>
    public static readonly Gender Other = new(5, "Other");

    /// <summary>
    /// Initializes a new instance of the <see cref="Gender"/> class.
    /// </summary>
    /// <param name="code">The unique code for the gender.</param>
    /// <param name="name">The name of the gender.</param>
    private Gender(int code, string name) : base(code, name) { }

    /// <summary>
    /// Gets a gender by its code.
    /// </summary>
    /// <param name="code">The gender code.</param>
    /// <returns>The gender with the specified code, or <c>null</c> if not found.</returns>
    public static Gender? FromCode(int code)
        => GetAll<Gender>().FirstOrDefault(g => g.Code == code);

    /// <summary>
    /// Gets a gender by its name (case-insensitive).
    /// </summary>
    /// <param name="name">The gender name.</param>
    /// <returns>The gender with the specified name, or <c>null</c> if not found.</returns>
    public static Gender? FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return GetAll<Gender>().FirstOrDefault(g =>
            g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}