namespace Core.Libraries.Domain.Entities.DomainEvents;

/// <summary>
/// Representa uma entidade que expõe eventos de domínio gerados durante seu ciclo de vida.
/// </summary>
/// <remarks>
/// <para>
/// Implementar esta interface permite que um aggregate root ou entidade publique eventos de domínio,
/// que podem ser tratados de forma assíncrona pela camada de domínio ou aplicação para disparar efeitos colaterais ou integrações.
/// </para>
/// <para>
/// Esta interface é tipicamente implementada por Aggregate Roots para rastrear eventos que ocorrem
/// durante operações de negócio. Os eventos são coletados durante a execução e podem ser publicados
/// após a persistência bem-sucedida do agregado.
/// </para>
/// <para>
/// A propriedade <see cref="Events"/> nunca deve retornar <c>null</c>; deve sempre retornar uma
/// instância válida de <see cref="IDomainEventList"/>, mesmo que vazia.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class Pedido : Aggregate&lt;EntityId&gt;
/// {
///     public void Confirmar()
///     {
///         // Lógica de negócio...
///         RegisterEvent(new PedidoConfirmadoEvent(Id, DateTime.UtcNow));
///     }
///     
///     // A propriedade Events é acessível através da interface
///     // var eventos = ((IHasDomainEvents)pedido).Events;
/// }
/// </code>
/// </example>
public interface IHasDomainEvents
{
    /// <summary>
    /// Obtém a coleção de eventos de domínio gerados pela entidade.
    /// </summary>
    /// <value>
    /// Um <see cref="IDomainEventList"/> contendo eventos de domínio que ocorreram como resultado de operações de negócio.
    /// Esta propriedade nunca retorna <c>null</c>; sempre retorna uma instância válida, mesmo que vazia.
    /// </value>
    IDomainEventList Events { get; }
}
