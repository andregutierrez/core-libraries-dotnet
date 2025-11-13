namespace Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Representa uma coleção somente leitura de identificadores de domínio associados a uma entidade ou objeto de valor.
/// </summary>
/// <typeparam name="TIdentifier">
/// O tipo de identificador que implementa <see cref="IIdentifier"/>.
/// </typeparam>
/// <remarks>
/// <para>
/// Esta interface estende <see cref="IReadOnlyCollection{T}"/> para fornecer uma coleção fortemente tipada
/// de instâncias de <see cref="IIdentifier"/>. É comumente usada para modelar identidade composta,
/// vinculação de entidades ou referência multi-chave na camada de domínio.
/// </para>
/// <para>
/// A interface garante que apenas tipos que implementam <see cref="IIdentifier"/> possam ser armazenados,
/// promovendo type safety e prevenindo erros de programação.
/// </para>
/// <para>
/// Implementada por <see cref="IdentifiersList{TIdentifier}"/>, que fornece a implementação concreta
/// para armazenar e gerenciar identificadores de sistemas externos.
/// </para>
/// <para>
/// Esta interface é usada principalmente como tipo de retorno em <see cref="IHasIdentifiers{TIdentifierList}"/>,
/// permitindo que entidades exponham suas identificações externas de forma segura e tipada.
/// </para>
/// </remarks>
public interface IIdentifiersList<TIdentifier> : IReadOnlyCollection<TIdentifier>
    where TIdentifier : IIdentifier { }