namespace Core.Libraries.Domain.Entities.DomainEvents;

/// <summary>
/// Define um contrato para gerenciar eventos de domínio dentro de um agregado ou entidade.
/// </summary>
/// <remarks>
/// Esta interface fornece acesso a eventos de domínio que foram gerados durante o ciclo de vida da entidade,
/// permitindo coleta, inspeção e limpeza explícita de eventos para suportar padrões como
/// despacho de eventos ou transactional outbox.
/// </remarks>
public interface IDomainEventList
{
    /// <summary>
    /// Limpa todos os eventos de domínio registrados neste agregado.
    /// </summary>
    /// <remarks>
    /// Tipicamente chamado após os eventos serem despachados para prevenir tratamento duplicado durante operações futuras de persistência.
    /// </remarks>
    void Clear();

    /// <summary>
    /// Retorna todos os eventos de domínio atualmente associados ao agregado.
    /// </summary>
    /// <returns>
    /// Um <see cref="IEnumerable{T}"/> contendo as instâncias <see cref="DomainEvent"/> que foram geradas.
    /// </returns>
    IEnumerable<DomainEvent> GetAll();
}
