namespace People.Domain.Persons.Entities;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Persons.Entities.Identifiers;
using People.Domain.Persons.Entities.Statuses;
using People.Domain.Persons.Events;
using People.Domain.Persons.ValueObjects;

/// <summary>
/// Represents a person in the system.
/// </summary>
public class Person : Aggregate<EntityId>, IHasAlternateKey, IHasIdentifiers<IdentifiersList<PersonIdentifier>>
{
    private readonly IdentifiersList<PersonIdentifier> _identifiers = new();
    private readonly PersonStatusHistory _statusHistory = new();

    /// <summary>
    /// Gets the person's full name.
    /// </summary>
    public PersonName Name { get; protected set; } = null!;

    /// <summary>
    /// Gets the person's birth date (birthday).
    /// All birthday-related operations are available through this Value Object.
    /// </summary>
    public BirthDate? BirthDate { get; protected set; }

    /// <summary>
    /// Gets the person's gender.
    /// </summary>
    public Gender? Gender { get; protected set; }

    /// <summary>
    /// Gets the alternate key for this person.
    /// Used for external references and cross-system integration.
    /// </summary>
    public AlternateKey Key { get; protected set; } = null!;

    /// <summary>
    /// Gets the collection of external system identifiers for this person.
    /// Used for mapping persons between different systems (DEPara).
    /// </summary>
    public IdentifiersList<PersonIdentifier> Identifiers => _identifiers;

    /// <summary>
    /// Gets the status history for this person.
    /// </summary>
    public PersonStatusHistory StatusHistory => _statusHistory;

    /// <summary>
    /// Gets the current status type of this person.
    /// </summary>
    public PersonStatusType? CurrentStatus => _statusHistory.GetCurrentStatusType();

    /// <summary>
    /// Gets a value indicating whether this person is currently active.
    /// </summary>
    public bool IsActive => _statusHistory.IsActive();

    /// <summary>
    /// Gets a value indicating whether this person is currently inactive.
    /// </summary>
    public bool IsInactive => _statusHistory.IsInactive();

    /// <summary>
    /// Gets a value indicating whether this person has been merged.
    /// </summary>
    public bool IsMerged => _statusHistory.IsMerged();

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected Person()
    {
        Key = default!;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Person"/>.
    /// This constructor is private to enforce the use of the <see cref="Create"/> factory method.
    /// </summary>
    /// <param name="key">The alternate key for this person.</param>
    /// <param name="name">The person's name.</param>
    /// <param name="birthDate">The person's birth date. Optional.</param>
    /// <param name="gender">The person's gender. Optional.</param>
    /// <exception cref="ArgumentNullException">Thrown when key or name is null.</exception>
    private Person(AlternateKey key, PersonName name, BirthDate? birthDate = null, Gender? gender = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(name);
        Key = key;
        Name = name;
        BirthDate = birthDate;
        Gender = gender;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Person"/>.
    /// This is the factory method for creating new person entities.
    /// The entity ID will be set by the persistence layer (EF Core), and a new alternate key is automatically generated.
    /// </summary>
    /// <param name="name">The person's name. Cannot be null.</param>
    /// <param name="birthDate">The person's birth date. Optional.</param>
    /// <param name="gender">The person's gender. Optional.</param>
    /// <returns>A new instance of <see cref="Person"/> with an automatically generated alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when name is null.</exception>
    public static Person Create(PersonName name, BirthDate? birthDate = null, Gender? gender = null)
    {
        ArgumentNullException.ThrowIfNull(name);

        var person = new Person(AlternateKey.New(), name, birthDate, gender);

        // Initialize with Active status
        var initialStatus = new PersonStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            PersonStatusType.Active,
            "Person created"
        );
        person._statusHistory.Add(initialStatus);

        person.RegisterEvent(new PersonCreatedEvent(person.Key.Value, person.Name, person.BirthDate, person.Gender));
        return person;
    }

    /// <summary>
    /// Imports a person from an external system with a specific alternate key.
    /// This method is used when importing data from external systems where the alternate key must be preserved.
    /// </summary>
    /// <param name="key">The alternate key from the external system. Cannot be null.</param>
    /// <param name="name">The person's name. Cannot be null.</param>
    /// <param name="birthDate">The person's birth date. Optional.</param>
    /// <param name="gender">The person's gender. Optional.</param>
    /// <returns>A new instance of <see cref="Person"/> with the specified alternate key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key or name is null.</exception>
    public static Person Import(AlternateKey key, PersonName name, BirthDate? birthDate = null, Gender? gender = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(name);

        var person = new Person(key, name, birthDate, gender);

        // Initialize with Active status
        var initialStatus = new PersonStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            PersonStatusType.Active,
            "Person imported"
        );
        person._statusHistory.Add(initialStatus);

        person.RegisterEvent(new PersonCreatedEvent(person.Key.Value, person.Name, person.BirthDate, person.Gender));
        return person;
    }

    /// <summary>
    /// Adds an external system identifier for this person.
    /// </summary>
    /// <param name="type">The type of external system.</param>
    /// <param name="externalId">The identifier value in the external system.</param>
    /// <exception cref="ArgumentException">Thrown when externalId is null or whitespace.</exception>
    public void AddIdentifier(IdentifierType type, string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("External ID cannot be null or whitespace.", nameof(externalId));

        var identifier = new PersonIdentifier(AlternateKey.New(), type, externalId);
        _identifiers.Add(identifier);
        RegisterEvent(new PersonIdentifierAddedEvent(Key.Value, type, externalId));
    }

    /// <summary>
    /// Gets an identifier by its type.
    /// </summary>
    /// <param name="type">The type of external system.</param>
    /// <returns>The identifier for the specified type, or <c>null</c> if not found.</returns>
    public PersonIdentifier? GetIdentifier(IdentifierType type)
        => _identifiers.GetByType(type);

    /// <summary>
    /// Removes an identifier by its type.
    /// </summary>
    /// <param name="type">The type of external system.</param>
    /// <returns><c>true</c> if the identifier was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveIdentifier(IdentifierType type)
    {
        var identifier = GetIdentifier(type);
        if (identifier == null)
            return false;

        var externalId = identifier.ExternalId;
        var removed = _identifiers.RemoveByType(type);

        if (removed)
        {
            RegisterEvent(new PersonIdentifierRemovedEvent(Key.Value, type, externalId));
        }

        return removed;
    }

    /// <summary>
    /// Updates the person's name.
    /// </summary>
    /// <param name="name">The new name. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when name is null.</exception>
    public void UpdateName(PersonName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var previousName = Name;
        Name = name;

        RegisterEvent(new PersonNameUpdatedEvent(Key.Value, previousName, name));
    }

    /// <summary>
    /// Updates the person's gender.
    /// </summary>
    /// <param name="gender">The new gender. Can be null to remove the gender.</param>
    public void UpdateGender(Gender? gender)
    {
        var previousGender = Gender;
        Gender = gender;

        RegisterEvent(new PersonGenderUpdatedEvent(Key.Value, previousGender, gender));
    }

    /// <summary>
    /// Updates the person's birth date.
    /// </summary>
    /// <param name="birthDate">The new birth date. Can be null to remove the birth date.</param>
    public void UpdateBirthDate(BirthDate? birthDate)
    {
        var previousBirthDate = BirthDate;
        BirthDate = birthDate;

        RegisterEvent(new PersonBirthDateUpdatedEvent(Key.Value, previousBirthDate, birthDate));
    }

    /// <summary>
    /// Changes the person's status.
    /// </summary>
    /// <param name="newStatus">The new status type.</param>
    /// <param name="notes">Optional notes explaining the status change.</param>
    /// <exception cref="ArgumentNullException">Thrown when newStatus is not a valid enum value.</exception>
    public void ChangeStatus(PersonStatusType newStatus, string notes = "")
    {
        var previousStatus = _statusHistory.GetCurrentStatusType();

        var status = new PersonStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            newStatus,
            notes
        );

        _statusHistory.Add(status);

        RegisterEvent(new PersonStatusChangedEvent(Key.Value, previousStatus, newStatus, notes));
    }

    /// <summary>
    /// Activates the person (sets status to Active).
    /// </summary>
    /// <param name="notes">Optional notes explaining the activation.</param>
    public void Activate(string notes = "Person activated")
        => ChangeStatus(PersonStatusType.Active, notes);

    /// <summary>
    /// Deactivates the person (sets status to Inactive).
    /// </summary>
    /// <param name="notes">Optional notes explaining the deactivation.</param>
    public void Deactivate(string notes = "Person deactivated")
        => ChangeStatus(PersonStatusType.Inactive, notes);

    /// <summary>
    /// Marks the person as merged (sets status to Merged).
    /// </summary>
    /// <param name="notes">Optional notes explaining the merge (e.g., target person key).</param>
    public void MarkAsMerged(string notes = "Person merged")
        => ChangeStatus(PersonStatusType.Merged, notes);
}
