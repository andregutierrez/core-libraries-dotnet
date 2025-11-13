namespace People.Domain.Contacts.ValueObjects;

using Core.Libraries.Domain.ValueObjects;

/// <summary>
/// Represents the type of contact information.
/// </summary>
public class ContactType : Enumeration
{
    /// <summary>
    /// Email contact type.
    /// </summary>
    public static readonly ContactType Email = new(1, "Email");

    /// <summary>
    /// Phone contact type.
    /// </summary>
    public static readonly ContactType Phone = new(2, "Phone");

    /// <summary>
    /// Mobile phone contact type.
    /// </summary>
    public static readonly ContactType Mobile = new(3, "Mobile");

    /// <summary>
    /// WhatsApp contact type.
    /// </summary>
    public static readonly ContactType WhatsApp = new(4, "WhatsApp");

    /// <summary>
    /// Social media contact type.
    /// </summary>
    public static readonly ContactType SocialMedia = new(5, "Social Media");

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactType"/> class.
    /// </summary>
    /// <param name="code">The unique code for the contact type.</param>
    /// <param name="name">The name of the contact type.</param>
    private ContactType(int code, string name) : base(code, name) { }

    /// <summary>
    /// Gets a contact type by its code.
    /// </summary>
    /// <param name="code">The contact type code.</param>
    /// <returns>The contact type with the specified code, or <c>null</c> if not found.</returns>
    public static ContactType? FromCode(int code)
        => GetAll<ContactType>().FirstOrDefault(ct => ct.Code == code);

    /// <summary>
    /// Gets a contact type by its name (case-insensitive).
    /// </summary>
    /// <param name="name">The contact type name.</param>
    /// <returns>The contact type with the specified name, or <c>null</c> if not found.</returns>
    public static ContactType? FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return GetAll<ContactType>().FirstOrDefault(ct =>
            ct.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}

