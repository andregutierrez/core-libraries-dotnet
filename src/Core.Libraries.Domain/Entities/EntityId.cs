namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Objeto de valor genérico que representa um identificador de entidade fortemente tipado.
/// </summary>
/// <typeparam name="T">O tipo subjacente do identificador (ex: int, Guid).</typeparam>
public record EntityId<T> : IEquatable<EntityId<T>>, IComparable<EntityId<T>>, IEntityId
    where T : IComparable, IEquatable<T>
{
    /// <summary>
    /// Obtém o valor subjacente do identificador.
    /// </summary>
    public T Value { get; protected set; }

    /// <summary>
    /// Cria uma nova instância de <see cref="EntityId{T}"/> com um valor especificado.
    /// </summary>
    /// <param name="value">O valor do identificador.</param>
    public EntityId(T value) => Value = value;

    /// <summary>
    /// Construtor protegido para serialização ou frameworks ORM.
    /// </summary>
    protected EntityId() => Value = default!;

    /// <inheritdoc />
    public int CompareTo(EntityId<T>? other)
    {
        if (other is null) return 1;
        return Comparer<T>.Default.Compare(Value, other.Value);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is not EntityId<T> other)
            throw new ArgumentException($"Cannot compare {GetType().Name} with object of type {obj.GetType().Name}.");

        return CompareTo(other);
    }

    /// <inheritdoc />
    public virtual bool Equals(EntityId<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    /// <inheritdoc />
    public override int GetHashCode()
        => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc />
    public override string ToString()
        => Value?.ToString() ?? string.Empty;
}
