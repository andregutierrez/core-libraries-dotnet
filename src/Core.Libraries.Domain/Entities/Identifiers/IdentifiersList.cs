using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Representa uma coleção genérica de identificações do tipo <typeparamref name="TIdentifier"/>.
/// Usada para armazenar múltiplos identificadores de sistemas externos para uma entidade, habilitando mapeamento DEPara.
/// </summary>
/// <typeparam name="TIdentifier">
/// O tipo de identificador que implementa <see cref="IIdentifier"/> armazenado nesta coleção.
/// Deve ser uma classe concreta que herda de <see cref="Identifier"/>.
/// </typeparam>
/// <remarks>
/// <para>
/// Esta classe permite que uma entidade mantenha múltiplas identificações para diferentes sistemas externos,
/// facilitando integrações e mapeamento DEPara (Data Exchange Para) entre plataformas.
/// </para>
/// <para>
/// Exemplo de uso:
/// <code>
/// public class User : Entity&lt;EntityId&gt;, IHasIdentifiers&lt;IdentifiersList&lt;OpenAIIdentifier&gt;&gt;
/// {
///     private readonly IdentifiersList&lt;OpenAIIdentifier&gt; _identifiers = new();
///     public IdentifiersList&lt;OpenAIIdentifier&gt; Identifiers => _identifiers;
///     
///     public void AddOpenAIIdentifier(string openAIId)
///     {
///         var identifier = new OpenAIIdentifier(AlternateKey.New(), openAIId);
///         _identifiers.Add(identifier);
///     }
///     
///     public OpenAIIdentifier? GetOpenAIIdentifier()
///     {
///         return _identifiers.GetByType(IdentifierType.OpenAIPlatform);
///     }
/// }
/// </code>
/// </para>
/// <para>
/// A coleção herda de <see cref="EntityList{TEntity}"/>, fornecendo funcionalidades como:
/// - Consultas LINQ
/// - Enumeração
/// - Contagem de itens
/// - Verificação de existência
/// </para>
/// </remarks>
public class IdentifiersList<TIdentifier> : EntityList<TIdentifier>, IIdentifiersList<TIdentifier>
    where TIdentifier : IIdentifier
{
    /// <summary>
    /// Adiciona um identificador à coleção.
    /// </summary>
    /// <param name="identifier">
    /// O identificador a ser adicionado. Não pode ser <c>null</c>.
    /// </param>
    /// <remarks>
    /// <para>
    /// Este método adiciona o identificador à coleção interna, permitindo que a entidade
    /// mantenha referências a múltiplos sistemas externos.
    /// </para>
    /// <para>
    /// O método é virtual para permitir que classes derivadas adicionem validações ou
    /// comportamentos específicos ao adicionar identificadores (ex: garantir unicidade por tipo).
    /// </para>
    /// </remarks>
    public virtual void Add(TIdentifier identifier)
    {
        _list.Add(identifier);
    }

    /// <summary>
    /// Obtém o primeiro identificador do tipo especificado.
    /// </summary>
    /// <param name="type">O tipo de identificação a ser buscado.</param>
    /// <returns>
    /// O primeiro identificador encontrado do tipo especificado, ou <c>null</c> se nenhum for encontrado.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Este método é útil para DEPara quando você precisa encontrar o identificador de um sistema específico.
    /// </para>
    /// <para>
    /// Exemplo:
    /// <code>
    /// var openAIIdentifier = identifiers.GetByType(IdentifierType.OpenAIPlatform);
    /// if (openAIIdentifier != null)
    /// {
    ///     // Usar identificador para integração
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public TIdentifier? GetByType(IdentifierType type)
    {
        return _list.FirstOrDefault(i => i.Type.Code == type.Code);
    }

    /// <summary>
    /// Verifica se existe um identificador do tipo especificado na coleção.
    /// </summary>
    /// <param name="type">O tipo de identificação a ser verificado.</param>
    /// <returns>
    /// <c>true</c> se existir pelo menos um identificador do tipo especificado; caso contrário, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Útil para verificar se uma entidade já possui identificação em um sistema externo antes de adicionar uma nova.
    /// </para>
    /// <para>
    /// Exemplo:
    /// <code>
    /// if (!identifiers.HasType(IdentifierType.OpenAIPlatform))
    /// {
    ///     identifiers.Add(new OpenAIIdentifier(AlternateKey.New(), openAIId));
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public bool HasType(IdentifierType type)
    {
        return _list.Any(i => i.Type.Code == type.Code);
    }

    /// <summary>
    /// Remove o primeiro identificador do tipo especificado da coleção.
    /// </summary>
    /// <param name="type">O tipo de identificação a ser removido.</param>
    /// <returns>
    /// <c>true</c> se um identificador foi removido; caso contrário, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Útil para remover identificações quando uma integração é desativada ou quando
    /// uma identificação precisa ser substituída.
    /// </para>
    /// <para>
    /// Exemplo:
    /// <code>
    /// // Remover identificação OpenAI quando integração é desativada
    /// identifiers.RemoveByType(IdentifierType.OpenAIPlatform);
    /// </code>
    /// </para>
    /// </remarks>
    public bool RemoveByType(IdentifierType type)
    {
        var identifier = GetByType(type);
        if (identifier == null)
            return false;

        return _list.Remove(identifier);
    }

    /// <summary>
    /// Obtém um identificador pela sua chave alternativa.
    /// </summary>
    /// <param name="key">A chave alternativa do identificador a ser buscado.</param>
    /// <returns>
    /// O identificador encontrado com a chave alternativa especificada, ou <c>null</c> se nenhum for encontrado.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Útil para buscar identificadores quando você tem a chave alternativa (GUID),
    /// especialmente em cenários de repositório ou quando trabalha com referências externas.
    /// </para>
    /// <para>
    /// Exemplo:
    /// <code>
    /// var identifier = identifiers.GetByKey(alternateKey);
    /// if (identifier != null)
    /// {
    ///     // Processar identificador
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public TIdentifier? GetByKey(AlternateKey key)
    {
        return _list.FirstOrDefault(i => i.Key.Value == key.Value);
    }

    /// <summary>
    /// Remove um identificador pela sua chave alternativa.
    /// </summary>
    /// <param name="key">A chave alternativa do identificador a ser removido.</param>
    /// <returns>
    /// <c>true</c> se um identificador foi removido; caso contrário, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Útil para remover identificações específicas quando você tem a chave alternativa (GUID),
    /// especialmente em cenários onde múltiplos identificadores do mesmo tipo podem existir
    /// e você precisa remover um específico.
    /// </para>
    /// <para>
    /// Exemplo:
    /// <code>
    /// // Remover identificação específica pela chave alternativa
    /// var removed = identifiers.RemoveByKey(alternateKey);
    /// if (removed)
    /// {
    ///     // Identificador removido com sucesso
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public bool RemoveByKey(AlternateKey key)
    {
        var identifier = GetByKey(key);
        if (identifier == null)
            return false;

        return _list.Remove(identifier);
    }

    /// <summary>
    /// Obtém todos os identificadores do tipo especificado.
    /// </summary>
    /// <param name="type">O tipo de identificação a ser filtrado.</param>
    /// <returns>
    /// Uma coleção enumerável contendo todos os identificadores do tipo especificado.
    /// Retorna uma coleção vazia se nenhum identificador do tipo for encontrado.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Útil quando uma entidade pode ter múltiplos identificadores do mesmo tipo
    /// (embora isso seja incomum, pode acontecer em cenários específicos).
    /// </para>
    /// <para>
    /// Exemplo:
    /// <code>
    /// var allOpenAIIdentifiers = identifiers.GetAllByType(IdentifierType.OpenAIPlatform);
    /// foreach (var identifier in allOpenAIIdentifiers)
    /// {
    ///     // Processar cada identificador
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public IEnumerable<TIdentifier> GetAllByType(IdentifierType type)
    {
        return _list.Where(i => i.Type.Code == type.Code);
    }
}
