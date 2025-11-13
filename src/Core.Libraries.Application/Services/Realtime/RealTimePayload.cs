namespace Core.Libraries.Application.Services.Realtime;

/// <summary>
/// Represents the payload to be delivered in a real-time message,
/// typically containing serialized data or a resource identifier.
/// </summary>
public record RealTimePayload(string Data);
