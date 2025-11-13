namespace Core.Libraries.Domain.Entities.Lifecycle;

public interface IAuditable
{
    AuditInfo Audit { get; }
}

public interface IAuditable<TUserId> : IAuditable
{
    new AuditInfo<TUserId> Audit { get; }

}