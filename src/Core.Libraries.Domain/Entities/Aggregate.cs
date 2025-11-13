using Core.Libraries.Domain.Entities.DomainEvents;

namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Classe base abstrata para agregados de domínio.
/// Um agregado é um cluster de objetos de domínio que são tratados como uma unidade para fins de mudança de dados.
/// Implementa o padrão Aggregate Root do Domain-Driven Design (DDD).
/// </summary>
/// <typeparam name="TEntityId">O tipo do identificador do agregado, deve implementar <see cref="IEntityId"/>.</typeparam>
public abstract class Aggregate<TEntityId> : Entity<TEntityId>, IHasDomainEvents
    where TEntityId : IEntityId
{
    /// <summary>
    /// Lista interna de eventos de domínio associados a este agregado.
    /// </summary>
    private readonly DomainEventList _domainEvents = new();

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Aggregate{TEntityId}"/>.
    /// Este construtor é tipicamente usado por frameworks ORM ou serialização.
    /// </summary>
    protected Aggregate() { }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Aggregate{TEntityId}"/> com um identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único para este agregado.</param>
    protected Aggregate(TEntityId id) : base(id) { }

    /// <summary>
    /// Registra um evento de domínio para ser processado posteriormente.
    /// Os eventos são coletados durante a execução de operações de domínio e podem ser
    /// publicados após a persistência bem-sucedida do agregado.
    /// </summary>
    /// <param name="eventData">Os dados do evento a ser registrado.</param>
    /// <exception cref="ArgumentNullException">Lançado quando eventData é null.</exception>
    public void RegisterEvent(object eventData)
    {
        ArgumentNullException.ThrowIfNull(eventData);
        _domainEvents.RegisterEvent(this, eventData);
    }

    /// <summary>
    /// Obtém a lista de eventos de domínio associados a este agregado.
    /// </summary>
    IDomainEventList IHasDomainEvents.Events => _domainEvents;
}
