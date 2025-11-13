namespace Core.Libraries.Domain.Entities.Lifecycle;


public class AuditInfo
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected AuditInfo(DateTime createdAt)
    {
        CreatedAt = createdAt;
        UpdatedAt = null;
    }

    public static AuditInfo Create()
    {
        return new AuditInfo(DateTime.UtcNow);
    }
    public void MarkAsModified()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

public class AuditInfo<TUserId> : AuditInfo
{
    public TUserId? CreatedBy { get; protected set; }

    public TUserId? UpdatedBy { get; protected set; }

    public AuditInfo(DateTime createdAt, TUserId? createdBy) : base(createdAt)
    {
        CreatedBy = createdBy;
        UpdatedBy = default;
    }

    public void MarkAsModified(TUserId? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public static AuditInfo Create<T>(T? createdBy)
        => new AuditInfo<T>(DateTime.UtcNow, createdBy);

}


