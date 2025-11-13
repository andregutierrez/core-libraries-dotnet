namespace Core.LibrariesDomain.Services.Realtime;

/// <summary>
/// Defines a contract for sending real-time messages to clients.
/// </summary>
public interface IRealTimeChannel
{
    /// <summary>
    /// Sends a real-time message to all connected clients.
    /// </summary>
    /// <param name="messageType">The type of the message being sent.</param>
    /// <param name="payload">The payload content of the message.</param>
    /// <param name="cancellationToken">Optional token to cancel the operation.</param>
    Task SendAsync(
        RealTimeMessageType messageType,
        RealTimePayload payload,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a real-time message to a specific channel.
    /// </summary>
    /// <param name="targetChannel">The target channel identifier.</param>
    /// <param name="messageType">The type of the message being sent.</param>
    /// <param name="payload">The payload content of the message.</param>
    /// <param name="cancellationToken">Optional token to cancel the operation.</param>
    Task SendAsync(
        RealTimeChannelId targetChannel,
        RealTimeMessageType messageType,
        RealTimePayload payload,
        CancellationToken cancellationToken = default);
}