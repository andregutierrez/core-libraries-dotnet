namespace People.Domain.Addresses.ValueObjects;

using Core.Libraries.Domain.ValueObjects;

/// <summary>
/// Represents the type of address (Residential, Commercial, Delivery, Billing, etc.).
/// </summary>
public class AddressType : Enumeration
{
    /// <summary>
    /// Residential address (home address).
    /// </summary>
    public static readonly AddressType Residential = new(1, "Residential");

    /// <summary>
    /// Commercial address (business address).
    /// </summary>
    public static readonly AddressType Commercial = new(2, "Commercial");

    /// <summary>
    /// Delivery address (for shipping).
    /// </summary>
    public static readonly AddressType Delivery = new(3, "Delivery");

    /// <summary>
    /// Billing address (for invoices).
    /// </summary>
    public static readonly AddressType Billing = new(4, "Billing");

    /// <summary>
    /// Other type of address.
    /// </summary>
    public static readonly AddressType Other = new(5, "Other");

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected AddressType() : base(0, string.Empty) { }

    /// <summary>
    /// Initializes a new instance of <see cref="AddressType"/>.
    /// </summary>
    /// <param name="code">The unique code for this address type.</param>
    /// <param name="name">The name of this address type.</param>
    private AddressType(int code, string name)
        : base(code, name)
    {
    }

    /// <summary>
    /// Gets an address type by its code.
    /// </summary>
    /// <param name="code">The code of the address type.</param>
    /// <returns>The address type with the specified code, or <c>null</c> if not found.</returns>
    public static AddressType? FromCode(int code)
        => GetAll<AddressType>().FirstOrDefault(t => t.Code == code);

    /// <summary>
    /// Gets an address type by its name.
    /// </summary>
    /// <param name="name">The name of the address type.</param>
    /// <returns>The address type with the specified name, or <c>null</c> if not found.</returns>
    public static AddressType? FromName(string name)
        => GetAll<AddressType>().FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}

