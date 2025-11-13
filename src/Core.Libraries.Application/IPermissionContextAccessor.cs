namespace Core.Libraries.Application;

public interface IPermissionContextAccessor
{
    void SetPermission(PermissionContext context);
    PermissionContext? GetPermission();
}
