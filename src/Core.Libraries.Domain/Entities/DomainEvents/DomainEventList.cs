namespace Core.Libraries.Domain.Entities.DomainEvents;

/// <summary>
/// Implementação padrão de <see cref="IDomainEventList"/> usada para gerenciar e coletar eventos de domínio gerados por um agregado.
/// </summary>
/// <remarks>
/// Esta classe encapsula a coleção interna de eventos e fornece métodos para registrar, recuperar
/// e limpar eventos de domínio que devem ser processados pela camada de domínio ou aplicação.
/// </remarks>
public class DomainEventList : EntityList<DomainEvent>, IDomainEventList
{
    /// <summary>
    /// Registra um novo evento de domínio para ser publicado posteriormente.
    /// </summary>
    /// <param name="eventData">
    /// Os dados do evento que descrevem a ocorrência do domínio. 
    /// Será encapsulado em um <see cref="DomainEvent"/> e rastreado na lista de eventos.
    /// </param>
    public void RegisterEvent(object eventData)
        => _list.Add(DomainEvent.Create(this, eventData));

    /// <summary>
    /// Limpa todos os eventos de domínio registrados neste agregado.
    /// </summary>
    /// <remarks>
    /// Isso é tipicamente invocado após todos os eventos terem sido despachados para garantir que o agregado esteja limpo para operações futuras.
    /// </remarks>
    public void Clear()
        => _list.Clear();

    /// <summary>
    /// Retorna todos os eventos de domínio atualmente associados ao agregado.
    /// </summary>
    /// <returns>
    /// Um <see cref="IEnumerable{T}"/> de instâncias <see cref="DomainEvent"/> que foram registradas.
    /// </returns>
    public IEnumerable<DomainEvent> GetAll()
        => _list.AsEnumerable();
}
