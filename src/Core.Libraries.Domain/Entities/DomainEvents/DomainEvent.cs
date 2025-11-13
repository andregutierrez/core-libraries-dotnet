using System.Threading;

namespace Core.Libraries.Domain.Entities.DomainEvents;

/// <summary>
/// Representa um evento de domínio que ocorreu no sistema.
/// </summary>
public class DomainEvent
{
    /// <summary>
    /// Variável estática para armazenar o último número gerado.
    /// </summary>
    private static long _last;

    /// <summary>
    /// Inicia uma nova instância da classe <see cref="DomainEvent"/>.
    /// </summary>
    /// <param name="entity">A entidade relacionada ao evento.</param>
    /// <param name="eventData">Os dados do evento de domínio.</param>
    /// <param name="eventOrder">A ordem do evento.</param>
    /// <exception cref="ArgumentNullException">Lançado quando entity ou eventData são null.</exception>
    public DomainEvent(object entity, object eventData, long eventOrder)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(eventData);

        Entity = entity;
        EventData = eventData;
        Order = eventOrder;
    }

    /// <summary>
    /// Obtém os dados do evento de domínio.
    /// </summary>
    public object EventData { get; }

    /// <summary>
    /// Obtém a entidade relacionada ao evento.
    /// </summary>
    public object Entity { get; }

    /// <summary>
    /// Obtém a ordem do evento.
    /// </summary>
    public long Order { get; }

    /// <summary>
    /// Cria um novo evento de domínio com os dados especificados e incrementa a ordem do evento.
    /// Este é o método recomendado para criar instâncias de <see cref="DomainEvent"/>.
    /// </summary>
    /// <param name="entity">A entidade relacionada ao evento.</param>
    /// <param name="eventData">Os dados do evento de domínio.</param>
    /// <returns>Uma nova instância da classe <see cref="DomainEvent"/>.</returns>
    /// <exception cref="ArgumentNullException">Lançado quando entity ou eventData são null.</exception>
    public static DomainEvent Create(object entity, object eventData)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(eventData);

        var order = Interlocked.Increment(ref _last);
        return new DomainEvent(entity, eventData, order);
    }
}
