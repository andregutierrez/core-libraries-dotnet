namespace Core.Libraries.Domain.Entities.Lifecycle;


public interface ISoftDeletable
{
    DeletionInfo Deletion { get; }
}


public interface ISoftDeletable<TUserId> : ISoftDeletable
{
    new DeletionInfo<TUserId> Deletion { get; }
}
