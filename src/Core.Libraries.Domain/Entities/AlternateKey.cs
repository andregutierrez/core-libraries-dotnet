namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Representa uma chave alternativa globalmente única usada para identificação de entidades entre sistemas.
/// Este objeto de valor encapsula um <see cref="Guid"/> para garantir type safety e clareza semântica,
/// particularmente em contextos envolvendo referências externas ou identificadores não primários.
/// </summary>
public record AlternateKey : IEquatable<AlternateKey>
{
    /// <summary>
    /// Obtém o valor subjacente <see cref="Guid"/> que representa a chave alternativa.
    /// </summary>
    public Guid Value { get; init; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AlternateKey"/> usando o <see cref="Guid"/> fornecido.
    /// </summary>
    /// <param name="value">O valor GUID para encapsular como chave alternativa.</param>
    /// <exception cref="ArgumentException">Lançada se o valor fornecido for <see cref="Guid.Empty"/>.</exception>
    public AlternateKey(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Alternate key cannot be empty.", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Construtor protegido para serialização ou frameworks ORM.
    /// </summary>
    protected AlternateKey() => Value = default;

    /// <summary>
    /// Gera uma nova instância de <see cref="AlternateKey"/> com um GUID recém-criado.
    /// </summary>
    /// <returns>Uma nova instância <see cref="AlternateKey"/> com um identificador único.</returns>
    public static AlternateKey New() => new AlternateKey(Guid.NewGuid());

    /// <summary>
    /// Habilita conversão implícita de <see cref="AlternateKey"/> para <see cref="Guid"/>.
    /// </summary>
    /// <param name="key">A <see cref="AlternateKey"/> para converter.</param>
    public static implicit operator Guid(AlternateKey key) => key.Value;

    /// <summary>
    /// Habilita conversão implícita de <see cref="Guid"/> para <see cref="AlternateKey"/>.
    /// </summary>
    /// <param name="guid">O <see cref="Guid"/> para encapsular como <see cref="AlternateKey"/>.</param>
    /// <exception cref="ArgumentException">Lançada se o valor fornecido for <see cref="Guid.Empty"/>.</exception>
    public static implicit operator AlternateKey(Guid guid) => new AlternateKey(guid);

    /// <summary>
    /// Retorna uma representação em string da chave alternativa.
    /// </summary>
    /// <returns>A representação em string do GUID subjacente.</returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Determina se a instância atual é igual a outra <see cref="AlternateKey"/>.
    /// </summary>
    /// <param name="other">A <see cref="AlternateKey"/> para comparar.</param>
    /// <returns><c>true</c> se ambas as instâncias forem iguais; caso contrário, <c>false</c>.</returns>
    public virtual bool Equals(AlternateKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    /// <summary>
    /// Retorna o código hash para a chave alternativa atual.
    /// </summary>
    /// <returns>O código hash do valor GUID subjacente.</returns>
    public override int GetHashCode() => Value.GetHashCode();
}
