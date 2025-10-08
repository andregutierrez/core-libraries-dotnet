namespace Core.Libraries.Domain.Entities.DomainEvents;

/// <summary>
/// Representa uma entidade que expõe eventos de domínio gerados durante seu ciclo de vida.
/// </summary>
/// <remarks>
/// Implementar esta interface permite que um aggregate root ou entidade publique eventos de domínio,
/// que podem ser tratados de forma assíncrona pela camada de domínio ou aplicação para disparar efeitos colaterais ou integrações.
/// </remarks>
public interface IHasDomainEvents
{
    /// <summary>
    /// Obtém a coleção de eventos de domínio gerados pela entidade.
    /// </summary>
    /// <value>
    /// Um <see cref="IDomainEventList"/> contendo eventos de domínio que ocorreram como resultado de operações de negócio.
    /// </value>
    IDomainEventList Events { get; }
}
