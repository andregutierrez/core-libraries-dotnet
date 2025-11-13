namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Objeto de valor que representa um identificador de entidade fortemente tipado.
/// </summary>
public record EntityId : IEquatable<EntityId>, IComparable<EntityId>, IEntityId
{
    /// <summary>
    /// Obtém o valor subjacente do identificador.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Cria uma nova instância de <see cref="EntityId"/> com um valor especificado.
    /// </summary>
    /// <param name="value">O valor do identificador. Deve ser maior ou igual a zero (zero indica entidade transitória).</param>
    /// <exception cref="ArgumentOutOfRangeException">Lançado quando o valor é menor que zero.</exception>
    public EntityId(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Entity ID cannot be negative.");

        Value = value;
    }

    /// <summary>
    /// Construtor protegido para serialização ou frameworks ORM.
    /// </summary>
    protected EntityId() => Value = default;

    /// <inheritdoc />
    public int CompareTo(EntityId? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is not EntityId other)
            throw new ArgumentException($"Cannot compare {GetType().Name} with object of type {obj.GetType().Name}.", nameof(obj));

        return CompareTo(other);
    }

    /// <inheritdoc />
    public virtual bool Equals(EntityId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override int GetHashCode()
        => Value.GetHashCode();

    /// <inheritdoc />
    public override string ToString()
        => Value.ToString();
}
