namespace Core.Libraries.Application;

public record PermissionContext(
    string UserId,
    string[] Roles,
    string[] Permissions
);
