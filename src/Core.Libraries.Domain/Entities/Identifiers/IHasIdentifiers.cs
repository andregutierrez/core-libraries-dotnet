namespace Core.Libraries.Domain.Entities.Identifiers
{
    /// <summary>
    /// Interface que representa uma coleção somente leitura de entidades de identificação.
    /// Permite que entidades exponham seus identificadores de sistemas externos para mapeamento DEPara.
    /// </summary>
    /// <typeparam name="TIdentifierList">
    /// O tipo da coleção de identificadores. Deve implementar <see cref="IReadOnlyCollection{T}"/> de <see cref="IIdentifier"/>.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// Esta interface é implementada por entidades que precisam manter identificações em sistemas externos,
    /// facilitando o mapeamento DEPara (Data Exchange Para) entre diferentes plataformas.
    /// </para>
    /// <para>
    /// Exemplo de uso:
    /// <code>
    /// public class User : Entity&lt;EntityId&gt;, IHasIdentifiers&lt;IdentifiersList&lt;OpenAIIdentifier&gt;&gt;
    /// {
    ///     private readonly IdentifiersList&lt;OpenAIIdentifier&gt; _identifiers = new();
    ///     public IdentifiersList&lt;OpenAIIdentifier&gt; Identifiers => _identifiers;
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// Ao implementar esta interface, a entidade pode:
    /// - Armazenar múltiplas identificações para diferentes sistemas externos
    /// - Facilitar a busca e mapeamento de entidades entre sistemas
    /// - Manter rastreabilidade de identificações externas
    /// </para>
    /// </remarks>
    public interface IHasIdentifiers<TIdentifierList>
        where TIdentifierList : IReadOnlyCollection<IIdentifier>
    {
        /// <summary>
        /// Obtém a coleção de identificadores como uma lista somente leitura.
        /// </summary>
        /// <value>
        /// Uma visualização somente leitura da coleção de identificadores.
        /// Permite acesso seguro aos identificadores sem permitir modificações diretas.
        /// </value>
        /// <remarks>
        /// <para>
        /// A coleção retornada é somente leitura para garantir que modificações sejam feitas
        /// através de métodos específicos da entidade, mantendo a integridade do modelo de domínio.
        /// </para>
        /// <para>
        /// Para adicionar ou remover identificadores, use os métodos apropriados da entidade
        /// que implementa esta interface, não manipule diretamente esta coleção.
        /// </para>
        /// </remarks>
        public TIdentifierList Identifiers { get; }
    }
}
