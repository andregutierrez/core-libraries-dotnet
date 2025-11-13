namespace Core.Libraries.Application.Services.Realtime;

/// <summary>
/// Represents the type of a real-time message to be published through the channel,
/// such as notifications or domain events.
/// </summary>
public record RealTimeMessageType(string Value);
