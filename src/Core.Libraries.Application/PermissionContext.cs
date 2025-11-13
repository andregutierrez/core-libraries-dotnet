namespace Core.LibrariesApplication;

public record PermissionContext(
    string UserId,
    string[] Roles,
    string[] Permissions
);
