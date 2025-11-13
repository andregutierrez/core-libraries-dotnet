using Core.Libraries.Application;

namespace Core.Libraries.Application.Commands;

public interface IHasPermissionContext
{
    PermissionContext? GetPermission();
}
