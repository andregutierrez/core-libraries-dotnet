namespace Core.Libraries.Domain.Entities.DomainEvents;

/// <summary>
/// Define um contrato para gerenciar eventos de domínio dentro de um agregado ou entidade.
/// </summary>
/// <remarks>
/// <para>
/// Esta interface fornece acesso a eventos de domínio que foram gerados durante o ciclo de vida da entidade,
/// permitindo coleta, inspeção e limpeza explícita de eventos para suportar padrões como
/// despacho de eventos ou transactional outbox.
/// </para>
/// <para>
/// O método <see cref="GetAll"/> nunca retorna <c>null</c>; sempre retorna um enumerable válido,
/// mesmo que vazio quando não há eventos registrados.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var agregado = new Pedido(pedidoId);
/// agregado.Confirmar(); // Registra eventos internamente
/// 
/// var eventos = agregado.Events;
/// if (eventos.Count > 0)
/// {
///     foreach (var evento in eventos.GetAll())
///     {
///         // Processar evento...
///     }
///     eventos.Clear(); // Limpar após processamento
/// }
/// </code>
/// </example>
public interface IDomainEventList
{
    /// <summary>
    /// Obtém o número de eventos de domínio registrados.
    /// </summary>
    /// <value>
    /// O número de eventos de domínio atualmente na lista. Sempre maior ou igual a zero.
    /// </value>
    int Count { get; }

    /// <summary>
    /// Limpa todos os eventos de domínio registrados neste agregado.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Tipicamente chamado após os eventos serem despachados para prevenir tratamento duplicado
    /// durante operações futuras de persistência.
    /// </para>
    /// <para>
    /// Após chamar este método, <see cref="Count"/> retornará zero e <see cref="GetAll"/> retornará
    /// um enumerable vazio.
    /// </para>
    /// </remarks>
    void Clear();

    /// <summary>
    /// Retorna todos os eventos de domínio atualmente associados ao agregado.
    /// </summary>
    /// <returns>
    /// Um <see cref="IEnumerable{T}"/> contendo as instâncias <see cref="DomainEvent"/> que foram geradas.
    /// Nunca retorna <c>null</c>; retorna um enumerable vazio quando não há eventos registrados.
    /// Os eventos são retornados na ordem em que foram registrados.
    /// </returns>
    IEnumerable<DomainEvent> GetAll();
}
