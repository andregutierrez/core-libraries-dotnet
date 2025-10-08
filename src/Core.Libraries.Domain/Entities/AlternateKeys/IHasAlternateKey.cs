namespace Core.Libraries.Domain.Entities.AlternateKeys;

/// <summary>
/// Fornece mecanismo implementar chaves alternativas em entidades.
/// </summary>
/// <remarks>
/// Uma chave alternativa serve como um identificador único adicional para uma entidade,
/// utilizada para propósitos de identificação além da chave primária da entidade.
/// A interface garante que qualquer entidade que a implemente possua uma propriedade de chave alternativa do tipo Guid.
/// </remarks>
public interface IHasAlternateKey
{
    /// <summary>
    /// Obtém a chave alternativa da entidade.
    /// </summary>
    /// <value>
    /// A chave alternativa, representada como um Guid
    /// </value>
    AlternateKey Key { get; }
}
