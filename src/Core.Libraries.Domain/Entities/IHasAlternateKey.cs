namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Fornece mecanismo para implementar chaves alternativas em entidades.
/// </summary>
/// <remarks>
/// <para>
/// Uma chave alternativa serve como um identificador único adicional para uma entidade,
/// utilizada para propósitos de identificação além da chave primária da entidade.
/// Esta interface garante que qualquer entidade que a implemente possua uma propriedade
/// de chave alternativa do tipo <see cref="AlternateKey"/>.
/// </para>
/// <para>
/// Chaves alternativas são especialmente úteis para:
/// - Integração entre sistemas e bounded contexts
/// - Referências externas em APIs públicas
/// - Auditoria e rastreamento com identificadores estáveis
/// - Migração de dados mantendo referências consistentes
/// </para>
/// <para>
/// A propriedade <see cref="Key"/> nunca deve retornar <c>null</c>; deve sempre retornar uma
/// instância válida de <see cref="AlternateKey"/>. Como <see cref="AlternateKey"/> valida que
/// o valor não seja <see cref="Guid.Empty"/>, a chave alternativa sempre terá um valor válido.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class Usuario : Entity&lt;EntityId&gt;, IHasAlternateKey
/// {
///     // protected set permite que ORMs definam o valor durante deserialização
///     public AlternateKey Key { get; protected set; }
///     public string Nome { get; set; }
///     
///     // Construtor protegido para ORM/serialização
///     protected Usuario() { }
///     
///     public Usuario(EntityId usuarioId, string nome) : base(usuarioId)
///     {
///         Nome = nome;
///         Key = AlternateKey.New(); // Gera chave alternativa automaticamente
///     }
///     
///     // A propriedade Key é acessível através da interface
///     // var chave = ((IHasAlternateKey)usuario).Key;
/// }
/// </code>
/// </example>
public interface IHasAlternateKey
{
    /// <summary>
    /// Obtém a chave alternativa da entidade.
    /// </summary>
    /// <value>
    /// A chave alternativa da entidade, do tipo <see cref="AlternateKey"/>.
    /// Esta propriedade nunca retorna <c>null</c>; sempre retorna uma instância válida.
    /// </value>
    AlternateKey Key { get; }
}
