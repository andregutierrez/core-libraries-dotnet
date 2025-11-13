using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Classe base abstrata que define a estrutura para identificações de sistemas externos.
/// Fornece suporte para chaves alternativas e categorização tipada de identificações.
/// </summary>
/// <remarks>
/// <para>
/// Esta classe é projetada para permitir que entidades do domínio sejam identificadas em sistemas externos,
/// facilitando o mapeamento DEPara (Data Exchange Para) entre diferentes plataformas e sistemas.
/// </para>
/// <para>
/// Classes derivadas devem implementar propriedades específicas para armazenar os dados de identificação
/// do sistema externo (ex: IDs, tokens, chaves de API, etc.).
/// </para>
/// <para>
/// Exemplo de uso:
/// <code>
/// public class OpenAIIdentifier : Identifier
/// {
///     public string OpenAIId { get; private set; }
///     
///     public OpenAIIdentifier(Guid key, string openAIId)
///         : base(key, IdentifierType.OpenAIPlatform)
///     {
///         OpenAIId = openAIId;
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public abstract class Identifier : Entity<IdentifierId>, IHasAlternateKey, IIdentifier
{
    /// <summary>
    /// Construtor protegido para uso por frameworks ORM ou serialização.
    /// </summary>
    protected Identifier()
    {
        Key = default!;
        Type = default!;
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Identifier"/>
    /// com a chave alternativa e o tipo de identificação fornecidos.
    /// </summary>
    /// <param name="key">
    /// A chave alternativa globalmente única usada para referenciar esta identificação externamente.
    /// Esta chave permite o mapeamento DEPara entre sistemas.
    /// </param>
    /// <param name="type">
    /// A classificação do sistema ou plataforma externa (ex: OpenAI, Salesforce, Microsoft Graph).
    /// Define em qual sistema externo esta identificação é válida.
    /// </param>
    /// <remarks>
    /// Este construtor cria uma identificação com ID interno padrão (0), útil para novas identificações
    /// que ainda não foram persistidas no banco de dados.
    /// </remarks>
    protected Identifier(Guid key, IdentifierType type)
        : base(new IdentifierId(0))
    {
        Key = key;
        Type = type;
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Identifier"/>
    /// com o identificador interno, chave alternativa e tipo de identificação fornecidos.
    /// </summary>
    /// <param name="id">
    /// O identificador único interno para esta instância de identificação.
    /// Usado para persistência e referência dentro do sistema.
    /// </param>
    /// <param name="key">
    /// A chave alternativa globalmente única usada para referenciar esta identificação externamente.
    /// Esta chave permite o mapeamento DEPara entre sistemas.
    /// </param>
    /// <param name="type">
    /// A classificação do sistema ou plataforma externa (ex: OpenAI, Salesforce, Microsoft Graph).
    /// Define em qual sistema externo esta identificação é válida.
    /// </param>
    /// <remarks>
    /// Use este construtor quando a identificação já possui um ID interno persistido,
    /// como ao carregar dados do banco de dados ou ao atualizar uma identificação existente.
    /// </remarks>
    protected Identifier(IdentifierId id, Guid key, IdentifierType type)
        : base(id)
    {
        Key = key;
        Type = type;
    }

    /// <summary>
    /// Obtém a chave alternativa (GUID) para esta identificação.
    /// </summary>
    /// <value>
    /// A chave alternativa que pode ser usada para referências externas, integrações ou indexação.
    /// Esta chave é essencial para o mapeamento DEPara, permitindo rastrear a mesma entidade
    /// em diferentes sistemas externos.
    /// </value>
    /// <remarks>
    /// A chave alternativa é diferente do ID interno da entidade e serve especificamente
    /// para integração com sistemas externos e mapeamento DEPara.
    /// </remarks>
    public AlternateKey Key { get; protected set; }

    /// <summary>
    /// Obtém o tipo ou classificação da identificação externa.
    /// </summary>
    /// <value>
    /// O tipo que identifica em qual sistema ou plataforma externa esta identificação é válida.
    /// Exemplos: OpenAI Platform, Salesforce Platform, Microsoft Graph, etc.
    /// </value>
    /// <remarks>
    /// O tipo permite que uma mesma entidade tenha múltiplas identificações para diferentes sistemas,
    /// facilitando integrações com múltiplas plataformas simultaneamente.
    /// </remarks>
    public IdentifierType Type { get; protected set; }
}
