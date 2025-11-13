namespace People.Domain.Addresses.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Addresses.Events;
using People.Domain.Addresses.ValueObjects;

/// <summary>
/// Represents an address for a person.
/// </summary>
public class Address : Aggregate<EntityId>, IHasAlternateKey
{
    /// <summary>
    /// Gets the alternate key for this address.
    /// Used for external references and cross-system integration.
    /// </summary>
    public AlternateKey Key { get; protected set; } = null!;

    /// <summary>
    /// Gets the person's entity ID this address belongs to.
    /// </summary>
    public EntityId PersonId { get; protected set; }

    /// <summary>
    /// Gets the type of address (Residential, Commercial, Delivery, Billing, etc.).
    /// </summary>
    public AddressType Type { get; protected set; } = null!;

    /// <summary>
    /// Gets the street locality entity ID.
    /// </summary>
    public EntityId StreetId { get; protected set; } = null!;

    /// <summary>
    /// Gets the address number, if any.
    /// </summary>
    public string? Number { get; protected set; }

    /// <summary>
    /// Gets the address complement (apartment, suite, etc.), if any.
    /// </summary>
    public string? Complement { get; protected set; }

    /// <summary>
    /// Gets the neighborhood/district locality entity ID, if any.
    /// </summary>
    public EntityId? NeighborhoodId { get; protected set; }

    /// <summary>
    /// Gets the city locality entity ID.
    /// </summary>
    public EntityId CityId { get; protected set; } = null!;

    /// <summary>
    /// Gets the state/province locality entity ID, if any.
    /// </summary>
    public EntityId? StateId { get; protected set; }

    /// <summary>
    /// Gets the country locality entity ID, if any.
    /// </summary>
    public EntityId? CountryId { get; protected set; }

    /// <summary>
    /// Gets the postal code locality entity ID, if any.
    /// </summary>
    public EntityId? PostalCodeId { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether this is the primary address.
    /// </summary>
    public bool IsPrimary { get; protected set; }

    /// <summary>
    /// Gets additional notes or context for this address.
    /// </summary>
    public string? Notes { get; protected set; }

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected Address()
    {
        Key = default!;
        PersonId = default!;
        Type = default!;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Address"/>.
    /// </summary>
    /// <param name="key">The alternate key for this address.</param>
    /// <param name="personId">The person's Id this address belongs to.</param>
    /// <param name="type">The type of address.</param>
    /// <param name="streetId">The street locality entity ID.</param>
    /// <param name="cityId">The city locality entity ID.</param>
    /// <param name="number">The address number, if any.</param>
    /// <param name="complement">The address complement, if any.</param>
    /// <param name="neighborhoodId">The neighborhood/district locality entity ID, if any.</param>
    /// <param name="stateId">The state/province locality entity ID, if any.</param>
    /// <param name="countryId">The country locality entity ID, if any.</param>
    /// <param name="postalCodeId">The postal code locality entity ID, if any.</param>
    /// <param name="isPrimary">Whether this is the primary address.</param>
    /// <param name="notes">Optional notes for this address.</param>
    /// <exception cref="ArgumentNullException">Thrown when key, personId, type, streetId, or cityId is null.</exception>
    private Address(
        AlternateKey key,
        EntityId personId,
        AddressType type,
        EntityId streetId,
        EntityId cityId,
        string? number = null,
        string? complement = null,
        EntityId? neighborhoodId = null,
        EntityId? stateId = null,
        EntityId? countryId = null,
        EntityId? postalCodeId = null,
        bool isPrimary = false,
        string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(streetId);
        ArgumentNullException.ThrowIfNull(cityId);

        Key = key;
        PersonId = personId;
        Type = type;
        StreetId = streetId;
        CityId = cityId;
        Number = number?.Trim();
        Complement = complement?.Trim();
        NeighborhoodId = neighborhoodId;
        StateId = stateId;
        CountryId = countryId;
        PostalCodeId = postalCodeId;
        IsPrimary = isPrimary;
        Notes = notes?.Trim();
    }

    /// <summary>
    /// Creates a new address.
    /// </summary>
    /// <param name="personId">The person's Id this address belongs to.</param>
    /// <param name="type">The type of address.</param>
    /// <param name="streetId">The street locality entity ID.</param>
    /// <param name="cityId">The city locality entity ID.</param>
    /// <param name="number">The address number, if any.</param>
    /// <param name="complement">The address complement, if any.</param>
    /// <param name="neighborhoodId">The neighborhood/district locality entity ID, if any.</param>
    /// <param name="stateId">The state/province locality entity ID, if any.</param>
    /// <param name="countryId">The country locality entity ID, if any.</param>
    /// <param name="postalCodeId">The postal code locality entity ID, if any.</param>
    /// <param name="isPrimary">Whether this is the primary address.</param>
    /// <param name="notes">Optional notes for this address.</param>
    /// <returns>A new instance of <see cref="Address"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when personId, type, streetId, or cityId is null.</exception>
    public static Address Create(
        EntityId personId,
        AddressType type,
        EntityId streetId,
        EntityId cityId,
        string? number = null,
        string? complement = null,
        EntityId? neighborhoodId = null,
        EntityId? stateId = null,
        EntityId? countryId = null,
        EntityId? postalCodeId = null,
        bool isPrimary = false,
        string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(streetId);
        ArgumentNullException.ThrowIfNull(cityId);

        var address = new Address(
            AlternateKey.New(),
            personId,
            type,
            streetId,
            cityId,
            number,
            complement,
            neighborhoodId,
            stateId,
            countryId,
            postalCodeId,
            isPrimary,
            notes);

        address.RegisterEvent(new AddressCreatedEvent(
            address.Key.Value,
            personId.Value,
            string.Empty, // Street name - should be resolved from locality service
            number,
            complement,
            null, // Neighborhood name - should be resolved from locality service
            string.Empty, // City name - should be resolved from locality service
            null, // State name - should be resolved from locality service
            null, // Country name - should be resolved from locality service
            null)); // Postal code name - should be resolved from locality service

        return address;
    }

    /// <summary>
    /// Imports an address from an external system with a specific alternate key.
    /// </summary>
    /// <param name="key">The alternate key from the external system. Cannot be null.</param>
    /// <param name="personId">The person's Id this address belongs to.</param>
    /// <param name="type">The type of address.</param>
    /// <param name="streetId">The street locality entity ID.</param>
    /// <param name="cityId">The city locality entity ID.</param>
    /// <param name="number">The address number, if any.</param>
    /// <param name="complement">The address complement, if any.</param>
    /// <param name="neighborhoodId">The neighborhood/district locality entity ID, if any.</param>
    /// <param name="stateId">The state/province locality entity ID, if any.</param>
    /// <param name="countryId">The country locality entity ID, if any.</param>
    /// <param name="postalCodeId">The postal code locality entity ID, if any.</param>
    /// <param name="isPrimary">Whether this is the primary address.</param>
    /// <param name="notes">Optional notes for this address.</param>
    /// <returns>A new instance of <see cref="Address"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key, personId, type, streetId, or cityId is null.</exception>
    public static Address Import(
        AlternateKey key,
        EntityId personId,
        AddressType type,
        EntityId streetId,
        EntityId cityId,
        string? number = null,
        string? complement = null,
        EntityId? neighborhoodId = null,
        EntityId? stateId = null,
        EntityId? countryId = null,
        EntityId? postalCodeId = null,
        bool isPrimary = false,
        string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(personId);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(streetId);
        ArgumentNullException.ThrowIfNull(cityId);

        var address = new Address(
            key,
            personId,
            type,
            streetId,
            cityId,
            number,
            complement,
            neighborhoodId,
            stateId,
            countryId,
            postalCodeId,
            isPrimary,
            notes);

        address.RegisterEvent(new AddressCreatedEvent(
            address.Key.Value,
            personId.Value,
            string.Empty, // Street name - should be resolved from locality service
            number,
            complement,
            null, // Neighborhood name - should be resolved from locality service
            string.Empty, // City name - should be resolved from locality service
            null, // State name - should be resolved from locality service
            null, // Country name - should be resolved from locality service
            null)); // Postal code name - should be resolved from locality service

        return address;
    }

    /// <summary>
    /// Updates the address information.
    /// </summary>
    /// <param name="streetId">The new street locality entity ID.</param>
    /// <param name="cityId">The new city locality entity ID.</param>
    /// <param name="number">The new address number, if any.</param>
    /// <param name="complement">The new address complement, if any.</param>
    /// <param name="neighborhoodId">The new neighborhood/district locality entity ID, if any.</param>
    /// <param name="stateId">The new state/province locality entity ID, if any.</param>
    /// <param name="countryId">The new country locality entity ID, if any.</param>
    /// <param name="postalCodeId">The new postal code locality entity ID, if any.</param>
    /// <exception cref="ArgumentNullException">Thrown when streetId or cityId is null.</exception>
    public void Update(
        EntityId streetId,
        EntityId cityId,
        string? number = null,
        string? complement = null,
        EntityId? neighborhoodId = null,
        EntityId? stateId = null,
        EntityId? countryId = null,
        EntityId? postalCodeId = null)
    {
        ArgumentNullException.ThrowIfNull(streetId);
        ArgumentNullException.ThrowIfNull(cityId);

        var previousNumber = Number;
        var previousComplement = Complement;

        StreetId = streetId;
        CityId = cityId;
        Number = number?.Trim();
        Complement = complement?.Trim();
        NeighborhoodId = neighborhoodId;
        StateId = stateId;
        CountryId = countryId;
        PostalCodeId = postalCodeId;

        RegisterEvent(new AddressUpdatedEvent(
            Key.Value,
            PersonId.Value,
            string.Empty, // Previous street name - should be resolved from locality service
            previousNumber,
            previousComplement,
            null, // Previous neighborhood name - should be resolved from locality service
            string.Empty, // Previous city name - should be resolved from locality service
            null, // Previous state name - should be resolved from locality service
            null, // Previous country name - should be resolved from locality service
            null, // Previous postal code name - should be resolved from locality service
            string.Empty, // New street name - should be resolved from locality service
            number,
            complement,
            null, // New neighborhood name - should be resolved from locality service
            string.Empty, // New city name - should be resolved from locality service
            null, // New state name - should be resolved from locality service
            null, // New country name - should be resolved from locality service
            null)); // New postal code name - should be resolved from locality service
    }

    /// <summary>
    /// Sets this address as the primary address.
    /// </summary>
    public void SetAsPrimary()
    {
        if (IsPrimary)
            return;

        IsPrimary = true;
        RegisterEvent(new AddressSetAsPrimaryEvent(Key.Value, PersonId.Value));
    }

    /// <summary>
    /// Removes the primary flag from this address.
    /// </summary>
    public void RemovePrimary()
    {
        if (!IsPrimary)
            return;

        IsPrimary = false;
        RegisterEvent(new AddressRemovedAsPrimaryEvent(Key.Value, PersonId.Value));
    }

    /// <summary>
    /// Updates the notes for this address.
    /// </summary>
    /// <param name="notes">The new notes. Can be null to remove notes.</param>
    public void UpdateNotes(string? notes)
    {
        Notes = notes?.Trim();
    }

    /// <summary>
    /// Updates the address type.
    /// </summary>
    /// <param name="type">The new address type. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
    public void UpdateType(AddressType type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Type = type;
    }

    /// <summary>
    /// Gets the full address as a formatted string.
    /// Note: When using locality entities, the names should be resolved from the locality service.
    /// This method returns a basic representation using entity IDs.
    /// </summary>
    public string GetFullAddress()
    {
        var parts = new List<string>();

        // Street
        parts.Add($"[Street: {StreetId.Value}]");

        if (!string.IsNullOrWhiteSpace(Number))
            parts.Add(Number);

        if (!string.IsNullOrWhiteSpace(Complement))
            parts.Add(Complement);

        // Neighborhood
        if (NeighborhoodId != null)
            parts.Add($"[Neighborhood: {NeighborhoodId.Value}]");

        // City
        parts.Add($"[City: {CityId.Value}]");

        // State
        if (StateId != null)
            parts.Add($"[State: {StateId.Value}]");

        // Postal Code
        if (PostalCodeId != null)
            parts.Add($"[PostalCode: {PostalCodeId.Value}]");

        // Country
        if (CountryId != null)
            parts.Add($"[Country: {CountryId.Value}]");

        return string.Join(", ", parts);
    }
}
