namespace Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Representa o tipo de identificação de sistema externo associada a uma entidade.
/// Este padrão permite comportamento mais rico do que enums nativos.
/// </summary>
/// <remarks>
/// <para>
/// Esta classe utiliza o padrão "Type Object" que permite adicionar comportamento e metadados
/// aos tipos de identificação, ao invés de usar enums simples que são limitados em funcionalidade.
/// </para>
/// <para>
/// Cada tipo de identificação possui um código numérico único e um nome descritivo,
/// facilitando a categorização e busca de identificações por sistema externo.
/// </para>
/// <para>
/// Para adicionar um novo tipo de sistema externo, crie uma nova constante estática nesta classe
/// e adicione-a ao método <see cref="List()"/>.
/// </para>
/// </remarks>
public class IdentifierType
{
    /// <summary>
    /// Tipo de identificação para a plataforma OpenAI.
    /// </summary>
    public static readonly IdentifierType OpenAIPlatform = new(1, "OpenAI Platform");

    /// <summary>
    /// Obtém o identificador numérico único do tipo.
    /// </summary>
    /// <value>
    /// O código numérico que identifica exclusivamente este tipo de sistema externo.
    /// Usado para persistência e comparações eficientes.
    /// </value>
    public int Code { get; }

    /// <summary>
    /// Obtém o nome de exibição do tipo.
    /// </summary>
    /// <value>
    /// O nome descritivo do tipo de sistema externo (ex: "OpenAI Platform", "Salesforce Platform").
    /// Usado para exibição em interfaces e logs.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Construtor privado sem parâmetros para uso por frameworks ORM ou serialização.
    /// </summary>
    private IdentifierType()
    {
        Code = 0;
        Name = string.Empty;
    }

    /// <summary>
    /// Construtor privado que inicializa uma nova instância do tipo de identificação.
    /// </summary>
    /// <param name="code">O código numérico único do tipo.</param>
    /// <param name="name">O nome descritivo do tipo.</param>
    private IdentifierType(int code, string name)
    {
        Code = code;
        Name = name;
    }

    /// <summary>
    /// Retorna a representação em string deste tipo.
    /// </summary>
    /// <returns>O nome do tipo de identificação.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Obtém todos os tipos de identificação predefinidos.
    /// </summary>
    /// <returns>
    /// Uma coleção enumerável contendo todos os tipos de identificação disponíveis no sistema.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Este método retorna todos os tipos de sistemas externos suportados.
    /// Para adicionar um novo tipo, crie uma constante estática e inclua-a neste array.
    /// </para>
    /// <para>
    /// Exemplo:
    /// <code>
    /// public static readonly IdentifierType SalesforcePlatform = new(2, "Salesforce Platform");
    /// 
    /// public static IEnumerable&lt;IdentifierType&gt; List() =>
    ///     new[] { OpenAIPlatform, SalesforcePlatform };
    /// </code>
    /// </para>
    /// </remarks>
    public static IEnumerable<IdentifierType> List() =>
        new[] { OpenAIPlatform /*, MicrosoftGraph */ };

    /// <summary>
    /// Encontra um tipo pelo seu código numérico.
    /// </summary>
    /// <param name="code">O código numérico do tipo a ser encontrado.</param>
    /// <returns>O tipo de identificação correspondente ao código fornecido.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Lançada quando o código fornecido não corresponde a nenhum tipo conhecido.
    /// </exception>
    /// <remarks>
    /// Útil para deserialização de dados ou busca por código armazenado no banco de dados.
    /// </remarks>
    public static IdentifierType FromCode(int code)
        => List().FirstOrDefault(t => t.Code == code)
           ?? throw new ArgumentOutOfRangeException(nameof(code), $"Código de tipo de identificação desconhecido: {code}");

    /// <summary>
    /// Encontra um tipo pelo seu nome.
    /// </summary>
    /// <param name="name">O nome do tipo a ser encontrado (case-insensitive).</param>
    /// <returns>O tipo de identificação correspondente ao nome fornecido.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Lançada quando o nome fornecido não corresponde a nenhum tipo conhecido.
    /// </exception>
    /// <remarks>
    /// A busca é case-insensitive, então "openai platform" e "OpenAI Platform" retornarão o mesmo resultado.
    /// Útil para parsing de strings ou configurações.
    /// </remarks>
    public static IdentifierType FromName(string name)
        => List().FirstOrDefault(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase))
           ?? throw new ArgumentOutOfRangeException(nameof(name), $"Nome de tipo de identificação desconhecido: {name}");

    /// <summary>
    /// Determina se o objeto especificado é igual a este tipo de identificação.
    /// </summary>
    /// <param name="obj">O objeto a ser comparado.</param>
    /// <returns>
    /// <c>true</c> se o objeto for um <see cref="IdentifierType"/> com o mesmo código; caso contrário, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// A comparação é feita pelo código numérico, não pelo nome, garantindo que tipos com o mesmo código
    /// sejam considerados iguais mesmo se tiverem nomes diferentes (o que não deveria acontecer).
    /// </remarks>
    public override bool Equals(object? obj) =>
        obj is IdentifierType other && Code == other.Code;

    /// <summary>
    /// Retorna o código hash para este tipo de identificação.
    /// </summary>
    /// <returns>O código hash baseado no código numérico do tipo.</returns>
    public override int GetHashCode() => Code.GetHashCode();

    /// <summary>
    /// Permite conversão implícita de <see cref="IdentifierType"/> para <see cref="int"/>.
    /// </summary>
    /// <param name="type">O tipo de identificação a ser convertido.</param>
    /// <returns>O código numérico do tipo.</returns>
    /// <remarks>
    /// Facilita o uso do tipo em contextos onde um inteiro é esperado,
    /// como em comparações ou armazenamento em banco de dados.
    /// </remarks>
    public static implicit operator int(IdentifierType type) => type.Code;
}

