using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Abstract base class for domain entity statuses, 
/// encapsulating an alternate key, creation timestamp, status type, notes, and current flag.
/// </summary>
/// <typeparam name="TStatus">
/// Enum type representing the specific status categories 
/// (e.g. <c>MessageStatusType</c>, <c>ExecutionStatusType</c>).
/// </typeparam>
public abstract class Status<TType> : Entity<StatusId>, IStatus<TType>, IHasAlternateKey
    where TType : Enum
{
    /// <summary>
    /// Protected parameterless constructor for ORM and deserialization scenarios.
    /// </summary>
    protected Status()
    {
        Key = default!;
        CreatedAt = default!;
        Type = default!;
        Notes = default!;
        IsCurrent = default!;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Status{TStatus}"/> 
    /// with all fields specified.
    /// </summary>
    /// <param name="key">
    /// Alternate GUID key used for external references or secondary indexes.
    /// </param>
    /// <param name="createdAt">
    /// Timestamp indicating when this status record was created.
    /// </param>
    /// <param name="type">
    /// Value of the <typeparamref name="TStatus"/> enum indicating the status category.
    /// </param>
    /// <param name="notes">
    /// Free-text notes or context associated with this status.
    /// </param>
    protected Status(Guid key, DateTime createdAt, TType type, string notes)
    {
        Key = key;
        CreatedAt = createdAt;
        Type = type;
        Notes = notes;
        IsCurrent = true;
    }

    /// <summary>
    /// Gets the alternate GUID key for this status, 
    /// used for external references or secondary indexing.
    /// </summary>
    public AlternateKey Key { get; protected set; }

    /// <summary>
    /// Gets the UTC timestamp when this status record was created.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Gets or sets the category of this status, 
    /// as defined by the <typeparamref name="TStatus"/> enum.
    /// </summary>
    public TType Type { get; set; }

    /// <summary>
    /// Gets or sets additional notes or context for this status.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether this status is the current active status.
    /// </summary>
    public bool IsCurrent { get; protected set; }

    /// <summary>
    /// Marks this status record as no longer current.
    /// </summary>
    public void Deactivate()
        => IsCurrent = false;
}
