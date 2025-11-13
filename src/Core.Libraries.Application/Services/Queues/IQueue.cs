using System;
namespace Core.LibrariesDomain.Services.Queues
{
    public interface IQueue<TMessage>
    {
        /// <summary>
        /// Enfileira uma mensagem para envio.
        /// </summary>
        /// <param name="message">Payload da mensagem.</param>
        /// <param name="options">Opções de entrega (atraso, cabeçalhos etc.).</param>
        /// <param name="cancellationToken">Token para cancelar a operação.</param>
        Task EnqueueAsync(
            string queueName,
            TMessage message,
            MessageOptions? options = default,
            CancellationToken cancellationToken = default);
    }

    public sealed record MessageOptions
    {
        /// <summary>
        /// Atraso antes de disponibilizar a mensagem na fila (opcional).
        /// </summary>
        public TimeSpan? Delay { get; init; }

        /// <summary>
        /// Cabeçalhos customizados para a mensagem (opcional).
        /// </summary>
        public IReadOnlyDictionary<string, string>? Headers { get; init; }

        public static readonly MessageOptions None = new();
    }
}

