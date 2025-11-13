namespace Core.LibrariesApplication.Commands;

public interface IHasPermissionContext
{
    PermissionContext? GetPermission();
}
