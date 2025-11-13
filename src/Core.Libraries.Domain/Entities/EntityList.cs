using System.Linq.Expressions;

namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Classe base abstrata para coleções de entidades de domínio.
/// Fornece uma coleção fortemente tipada que implementa múltiplas interfaces de coleção
/// e suporta consultas LINQ através de IQueryable.
/// </summary>
/// <typeparam name="TEntity">O tipo de entidades na coleção.</typeparam>
public abstract class EntityList<TEntity> : ICollection<TEntity>, IReadOnlyCollection<TEntity>, IQueryable<TEntity>
{
    /// <summary>
    /// A lista subjacente que armazena as entidades.
    /// </summary>
    protected readonly List<TEntity> _list = new();

    /// <summary>
    /// Cache do IQueryable para evitar recriações desnecessárias.
    /// </summary>
    private IQueryable<TEntity>? _queryable;

    /// <summary>
    /// Obtém o número de entidades na coleção.
    /// </summary>
    public int Count => _list.Count;

    /// <summary>
    /// Obtém um valor que indica se a coleção é somente leitura.
    /// Esta coleção é sempre gravável, então retorna false.
    /// </summary>
    bool ICollection<TEntity>.IsReadOnly => false;

    /// <summary>
    /// Obtém o tipo dos elementos na coleção (implementação IQueryable).
    /// </summary>
    public Type ElementType => typeof(TEntity);

    /// <summary>
    /// Obtém a árvore de expressão associada à coleção (implementação IQueryable).
    /// </summary>
    public Expression Expression => Queryable.Expression;

    /// <summary>
    /// Obtém o provedor de consulta para a coleção (implementação IQueryable).
    /// </summary>
    public IQueryProvider Provider => Queryable.Provider;

    /// <summary>
    /// Obtém ou cria o IQueryable para a lista.
    /// </summary>
    private IQueryable<TEntity> Queryable => _queryable ??= _list.AsQueryable();

    /// <summary>
    /// Retorna um wrapper somente leitura ao redor da coleção.
    /// </summary>
    /// <returns>Uma coleção somente leitura contendo as mesmas entidades.</returns>
    public IReadOnlyCollection<TEntity> AsReadOnly()
        => _list.AsReadOnly();

    /// <summary>
    /// Adiciona uma entidade à coleção (implementação ICollection).
    /// </summary>
    /// <param name="item">A entidade a ser adicionada.</param>
    /// <exception cref="ArgumentNullException">Lançado quando item é null.</exception>
    void ICollection<TEntity>.Add(TEntity item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _list.Add(item);
        _queryable = null; // Limpa o cache do queryable quando há modificação
    }

    /// <summary>
    /// Remove todas as entidades da coleção (implementação ICollection).
    /// </summary>
    void ICollection<TEntity>.Clear()
    {
        _list.Clear();
        _queryable = null; // Limpa o cache do queryable
    }

    /// <summary>
    /// Determina se a coleção contém uma entidade específica (implementação ICollection).
    /// </summary>
    /// <param name="item">A entidade a ser localizada na coleção.</param>
    /// <returns>true se a entidade for encontrada; caso contrário, false.</returns>
    bool ICollection<TEntity>.Contains(TEntity item)
        => item is not null && _list.Contains(item);

    /// <summary>
    /// Copia as entidades da coleção para um array, começando em um índice específico do array (implementação ICollection).
    /// </summary>
    /// <param name="array">O array unidimensional que é o destino das entidades copiadas da coleção.</param>
    /// <param name="arrayIndex">O índice baseado em zero no array no qual a cópia começa.</param>
    /// <exception cref="ArgumentNullException">Lançado quando array é null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Lançado quando arrayIndex é menor que zero.</exception>
    void ICollection<TEntity>.CopyTo(TEntity[] array, int arrayIndex)
        => _list.CopyTo(array, arrayIndex);

    /// <summary>
    /// Retorna um enumerador que itera através da coleção (implementação IEnumerable).
    /// </summary>
    /// <returns>Um enumerador que pode ser usado para iterar através da coleção.</returns>
    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        => _list.GetEnumerator();

    /// <summary>
    /// Remove a primeira ocorrência de uma entidade específica da coleção (implementação ICollection).
    /// </summary>
    /// <param name="item">A entidade a ser removida da coleção.</param>
    /// <returns>true se a entidade foi removida com sucesso; caso contrário, false.</returns>
    bool ICollection<TEntity>.Remove(TEntity item)
    {
        if (item is null) return false;
        var removed = _list.Remove(item);
        if (removed)
            _queryable = null; // Limpa o cache do queryable quando há modificação
        return removed;
    }

    /// <summary>
    /// Retorna um enumerador que itera através da coleção (implementação IEnumerable).
    /// </summary>
    /// <returns>Um enumerador que pode ser usado para iterar através da coleção.</returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => _list.GetEnumerator();
}
