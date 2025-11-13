namespace People.Domain.Addresses.Events;

/// <summary>
/// Domain event raised when an address is set as primary.
/// </summary>
public record AddressSetAsPrimaryEvent
{
    /// <summary>
    /// Gets the address's alternate key.
    /// </summary>
    public Guid AddressKey { get; init; }

    /// <summary>
    /// Gets the person's entity ID this address belongs to.
    /// </summary>
    public int PersonId { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="AddressSetAsPrimaryEvent"/>.
    /// </summary>
    public AddressSetAsPrimaryEvent(Guid addressKey, int personId)
    {
        AddressKey = addressKey;
        PersonId = personId;
    }
}

