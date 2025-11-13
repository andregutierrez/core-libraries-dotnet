namespace People.Domain.Addresses.Events;

/// <summary>
/// Domain event raised when an address is removed as primary.
/// </summary>
public record AddressRemovedAsPrimaryEvent
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
    /// Initializes a new instance of <see cref="AddressRemovedAsPrimaryEvent"/>.
    /// </summary>
    public AddressRemovedAsPrimaryEvent(Guid addressKey, int personId)
    {
        AddressKey = addressKey;
        PersonId = personId;
    }
}

