namespace Core.Libraries.Domain.Entities.AlternateKeys;

/// <summary>
/// Representa uma chave alternativa globalmente única usada para identificação de entidades entre sistemas.
/// Este objeto de valor encapsula um <see cref="Guid"/> para garantir type safety e clareza semântica,
/// particularmente em contextos envolvendo referências externas ou identificadores não primários.
/// </summary>
public readonly struct AlternateKey : IEquatable<AlternateKey>
{
    /// <summary>
    /// Obtém o valor subjacente <see cref="Guid"/> que representa a chave alternativa.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Inicializa uma nova instância da struct <see cref="AlternateKey"/> usando o <see cref="Guid"/> fornecido.
    /// </summary>
    /// <param name="value">O valor GUID para encapsular como chave alternativa.</param>
    /// <exception cref="ArgumentException">Lançada se o valor fornecido for <see cref="Guid.Empty"/>.</exception>
    public AlternateKey(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Chave alternativa não pode ser vazia", nameof(value));

        Value = value;
    }

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
    public bool Equals(AlternateKey other) => Value.Equals(other.Value);

    /// <summary>
    /// Determina se a instância atual é igual a outro objeto.
    /// </summary>
    /// <param name="obj">O objeto para comparar.</param>
    /// <returns><c>true</c> se o objeto for uma <see cref="AlternateKey"/> com o mesmo valor; caso contrário, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is AlternateKey other && Equals(other);

    /// <summary>
    /// Retorna o código hash para a chave alternativa atual.
    /// </summary>
    /// <returns>O código hash do valor GUID subjacente.</returns>
    public override int GetHashCode() => Value.GetHashCode();
}
