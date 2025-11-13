namespace Core.Libraries.Domain.Entities.Lifecycle;

public class DeletionInfo
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public DeletionInfo() : base() { }

    public void MarkAsDeleted()
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }

    public void Restore()
    {
        if (IsDeleted)
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}

public class DeletionInfo<TUserId> : DeletionInfo
{
    public TUserId? DeletedBy { get; protected set; }

    public void MarkAsDeleted(TUserId? deletedBy = default)
    {
        base.MarkAsDeleted();
        DeletedBy = deletedBy;

    }

    public void Restore(TUserId? deletedBy = default)
    {
        if (IsDeleted)
        {
            base.Restore();
            DeletedBy = deletedBy;
        }
    }
}
