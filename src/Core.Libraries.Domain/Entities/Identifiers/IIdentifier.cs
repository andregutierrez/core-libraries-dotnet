using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Interface marcadora para identificadores de nível de domínio.
/// </summary>
/// <remarks>
/// <para>
/// Esta interface destina-se a ser implementada por tipos que atuam como identificadores fortemente tipados,
/// como objetos de valor representando chaves únicas (ex: GUIDs, chaves compostas ou structs personalizados).
/// </para>
/// <para>
/// Promove type safety e clareza semântica dentro do modelo de domínio, garantindo que apenas
/// tipos que realmente representam identificações externas possam ser usados em coleções de identificadores.
/// </para>
/// <para>
/// Qualquer classe que implemente esta interface pode ser armazenada em uma <see cref="IdentifiersList{TIdentifier}"/>,
/// permitindo que entidades mantenham múltiplas identificações para diferentes sistemas externos.
/// </para>
/// </remarks>
public interface IIdentifier
{
    /// <summary>
    /// Obtém a chave alternativa (GUID) para esta identificação.
    /// </summary>
    /// <value>
    /// A chave alternativa que pode ser usada para referências externas, integrações ou indexação.
    /// Essencial para o mapeamento DEPara entre sistemas.
    /// </value>
    public AlternateKey Key { get; }

    /// <summary>
    /// Obtém o tipo ou classificação da identificação externa.
    /// </summary>
    /// <value>
    /// O tipo que identifica em qual sistema ou plataforma externa esta identificação é válida.
    /// Permite categorizar e filtrar identificações por sistema externo.
    /// </value>
    public IdentifierType Type { get; }
}