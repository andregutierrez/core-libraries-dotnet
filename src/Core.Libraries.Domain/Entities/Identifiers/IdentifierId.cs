using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Entities.Identifiers;

/// <summary>
/// Representa o identificador de uma instância de identificação no domínio,
/// estendendo <see cref="EntityId"/> com um tipo subjacente <see cref="int"/>.
/// </summary>
/// <remarks>
/// <para>
/// Esta classe encapsula o identificador interno de uma instância de <see cref="Identifier"/>,
/// fornecendo type safety e clareza semântica ao trabalhar com identificações no domínio.
/// </para>
/// <para>
/// O uso de um tipo específico ao invés de <see cref="int"/> diretamente previne erros de programação
/// onde identificadores de diferentes entidades possam ser trocados acidentalmente.
/// </para>
/// <para>
/// Suporta conversões implícitas para facilitar o uso em contextos onde um inteiro é esperado,
/// como em consultas LINQ ou comparações.
/// </para>
/// </remarks>
public sealed record IdentifierId : EntityId
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="IdentifierId"/>
    /// com o valor inteiro fornecido.
    /// </summary>
    /// <param name="value">
    /// Valor inteiro que identifica esta instância de identificação.
    /// Geralmente atribuído pelo banco de dados durante a persistência.
    /// </param>
    public IdentifierId(int value)
        : base(value) { }

    /// <summary>
    /// Converte implicitamente um <see cref="int"/> para um <see cref="IdentifierId"/>.
    /// </summary>
    /// <param name="id">O valor inteiro a ser convertido.</param>
    /// <returns>Uma nova instância de <see cref="IdentifierId"/> com o valor fornecido.</returns>
    /// <remarks>
    /// Esta conversão permite usar inteiros diretamente onde um <see cref="IdentifierId"/> é esperado,
    /// facilitando a criação de instâncias sem necessidade de construtor explícito.
    /// </remarks>
    public static implicit operator IdentifierId(int id)
        => new IdentifierId(id);

    /// <summary>
    /// Converte implicitamente um <see cref="IdentifierId"/> para um <see cref="int"/>,
    /// retornando o valor do identificador subjacente.
    /// </summary>
    /// <param name="identificationId">A instância de <see cref="IdentifierId"/> a ser convertida.</param>
    /// <returns>O valor inteiro do identificador.</returns>
    /// <remarks>
    /// Esta conversão permite usar <see cref="IdentifierId"/> em contextos onde um inteiro é esperado,
    /// como em comparações, cálculos ou armazenamento em banco de dados.
    /// </remarks>
    public static implicit operator int(IdentifierId identificationId)
        => identificationId.Value;
}