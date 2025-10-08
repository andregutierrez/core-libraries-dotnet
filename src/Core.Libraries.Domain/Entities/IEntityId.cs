namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Interface marcadora para identificadores de entidade.
/// Todos os identificadores de entidade devem implementar esta interface para garantir que possam ser comparados.
/// </summary>
public interface IEntityId 
    : IComparable { }
