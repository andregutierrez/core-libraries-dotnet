namespace People.Domain.Addresses.Events;

/// <summary>
/// Domain event raised when an address is updated.
/// </summary>
public record AddressUpdatedEvent
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
    /// Gets the previous street address line.
    /// </summary>
    public string PreviousStreet { get; init; } = string.Empty;

    /// <summary>
    /// Gets the previous address number, if any.
    /// </summary>
    public string? PreviousNumber { get; init; }

    /// <summary>
    /// Gets the previous address complement, if any.
    /// </summary>
    public string? PreviousComplement { get; init; }

    /// <summary>
    /// Gets the previous neighborhood/district, if any.
    /// </summary>
    public string? PreviousNeighborhood { get; init; }

    /// <summary>
    /// Gets the previous city name.
    /// </summary>
    public string PreviousCity { get; init; } = string.Empty;

    /// <summary>
    /// Gets the previous state/province, if any.
    /// </summary>
    public string? PreviousState { get; init; }

    /// <summary>
    /// Gets the previous country name, if any.
    /// </summary>
    public string? PreviousCountry { get; init; }

    /// <summary>
    /// Gets the previous postal code, if any.
    /// </summary>
    public string? PreviousPostalCode { get; init; }

    /// <summary>
    /// Gets the new street address line.
    /// </summary>
    public string NewStreet { get; init; } = string.Empty;

    /// <summary>
    /// Gets the new address number, if any.
    /// </summary>
    public string? NewNumber { get; init; }

    /// <summary>
    /// Gets the new address complement, if any.
    /// </summary>
    public string? NewComplement { get; init; }

    /// <summary>
    /// Gets the new neighborhood/district, if any.
    /// </summary>
    public string? NewNeighborhood { get; init; }

    /// <summary>
    /// Gets the new city name.
    /// </summary>
    public string NewCity { get; init; } = string.Empty;

    /// <summary>
    /// Gets the new state/province, if any.
    /// </summary>
    public string? NewState { get; init; }

    /// <summary>
    /// Gets the new country name, if any.
    /// </summary>
    public string? NewCountry { get; init; }

    /// <summary>
    /// Gets the new postal code, if any.
    /// </summary>
    public string? NewPostalCode { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="AddressUpdatedEvent"/>.
    /// </summary>
    public AddressUpdatedEvent(
        Guid addressKey,
        int personId,
        string previousStreet,
        string? previousNumber,
        string? previousComplement,
        string? previousNeighborhood,
        string previousCity,
        string? previousState,
        string? previousCountry,
        string? previousPostalCode,
        string newStreet,
        string? newNumber,
        string? newComplement,
        string? newNeighborhood,
        string newCity,
        string? newState,
        string? newCountry,
        string? newPostalCode)
    {
        AddressKey = addressKey;
        PersonId = personId;
        PreviousStreet = previousStreet;
        PreviousNumber = previousNumber;
        PreviousComplement = previousComplement;
        PreviousNeighborhood = previousNeighborhood;
        PreviousCity = previousCity;
        PreviousState = previousState;
        PreviousCountry = previousCountry;
        PreviousPostalCode = previousPostalCode;
        NewStreet = newStreet;
        NewNumber = newNumber;
        NewComplement = newComplement;
        NewNeighborhood = newNeighborhood;
        NewCity = newCity;
        NewState = newState;
        NewCountry = newCountry;
        NewPostalCode = newPostalCode;
    }
}

