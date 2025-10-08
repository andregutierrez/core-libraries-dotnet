namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Classe base abstrata para todas as entidades de domínio no sistema.
/// Fornece funcionalidade comum para identificação, comparação e igualdade de entidades.
/// </summary>
/// <typeparam name="TKey">O tipo do identificador da entidade, deve implementar <see cref="IEntityId"/>.</typeparam>
public abstract class Entity<TKey> : IComparable<Entity<TKey>>
    where TKey : IEntityId
{
    /// <summary>
    /// O identificador único para esta entidade.
    /// </summary>
    protected TKey _id = default!;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Entity{TKey}"/>.
    /// Este construtor é tipicamente usado por frameworks ORM ou serialização.
    /// </summary>
    protected Entity() 
        => _id = default!;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Entity{TKey}"/> com um identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único para esta entidade.</param>
    protected Entity(TKey id) 
        => _id = id;

    /// <summary>
    /// Obtém o identificador único para esta entidade.
    /// </summary>
    public TKey Id 
        => _id;

    /// <summary>
    /// Obtém um valor que indica se esta entidade é transitória (ainda não persistida).
    /// Uma entidade é considerada transitória se seu identificador for o valor padrão.
    /// </summary>
    protected bool IsTransient =>
        EqualityComparer<TKey>.Default.Equals(Id, default);


    /// <summary>
    /// Compara esta entidade com outra entidade para fins de ordenação.
    /// Entidades transitórias são consideradas menores que entidades persistidas.
    /// </summary>
    /// <param name="other">A entidade para comparar com esta entidade.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa das entidades sendo comparadas.
    /// Retorna 1 se other for null, 0 se ambas forem transitórias, -1 se esta for transitória,
    /// 1 se other for transitória, ou o resultado da comparação dos identificadores.
    /// </returns>
    public int CompareTo(Entity<TKey>? other)
    {
        if (other is null)
            return 1;

        if (IsTransient && other.IsTransient)
            return 0;

        if (IsTransient)
            return -1;
        if (other.IsTransient)
            return 1;

        return Id.CompareTo(other.Id);
    }

    /// <summary>
    /// Determina se o objeto especificado é igual a esta entidade.
    /// Duas entidades são consideradas iguais se tiverem o mesmo tipo e o mesmo identificador.
    /// Entidades transitórias nunca são iguais a outras entidades.
    /// </summary>
    /// <param name="obj">O objeto para comparar com esta entidade.</param>
    /// <returns>true se o objeto especificado for igual a esta entidade; caso contrário, false.</returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is not Entity<TKey> other)
            return false;

        if (IsTransient || other.IsTransient)
            return false;

        return Id.Equals(other.Id);
    }

    /// <summary>
    /// Retorna um código hash para esta entidade.
    /// Para entidades transitórias, retorna o código hash base.
    /// Para entidades persistidas, retorna o código hash do identificador.
    /// </summary>
    /// <returns>Um código hash para esta entidade.</returns>
    public override int GetHashCode()
        => IsTransient
            ? base.GetHashCode()
            : Id.GetHashCode();
   
    /// <summary>
    /// Retorna uma representação em string desta entidade.
    /// </summary>
    /// <returns>Uma string que representa esta entidade no formato "NomeDoTipo [Id=identificador]".</returns>
    public override string ToString()
        => $"{GetType().Name} [Id={Id}]";
}

