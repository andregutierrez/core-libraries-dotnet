namespace Core.LibrariesApplication;

public interface IPermissionContextAccessor
{
    void SetPermission(PermissionContext context);
    PermissionContext? GetPermission();
}
