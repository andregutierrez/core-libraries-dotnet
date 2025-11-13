namespace Core.LibrariesApplication;

public record ClientMetadata(
    string IpAddress,
    string? UserAgent,
    string? Origin,
    string? DeviceId,
    string? SessionId
);
