namespace Core.Libraries.Domain.ValueObjects.Periods;

/// <summary>
/// Representa um período definido pelo usuário com uma label customizada e datas de início/fim arbitrárias.
/// </summary>
public record CustomPeriod : Period
{
    /// <summary>
    /// Obtém a label ou nome customizado deste período (ex: "Q1 Fiscal", "Campanha Inverno").
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CustomPeriod"/>.
    /// </summary>
    /// <param name="label">
    /// Um nome não-vazio para o período.
    /// </param>
    /// <param name="start">
    /// A data de início inclusiva do período.
    /// </param>
    /// <param name="end">
    /// A data de fim inclusiva do período. Não deve ser anterior a <paramref name="start"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="label"/> é null/vazio ou se <paramref name="end"/> &lt; <paramref name="start"/>.
    /// </exception>
    public CustomPeriod(string label, DateOnly start, DateOnly end)
        : base(start, end)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label must not be empty.", nameof(label));

        Label = label;
    }

    /// <summary>
    /// Cria um <see cref="CustomPeriod"/> a partir de duas datas e uma label.
    /// </summary>
    /// <param name="label">A label do período.</param>
    /// <param name="start">A data de início.</param>
    /// <param name="end">A data de fim.</param>
    /// <returns>Um novo <see cref="CustomPeriod"/>.</returns>
    public static CustomPeriod Create(string label, DateOnly start, DateOnly end)
        => new CustomPeriod(label, start, end);

    /// <inheritdoc />
    /// <remarks>
    /// Para períodos customizados, não é possível determinar o período anterior ou próximo
    /// sem conhecimento adicional sobre a duração ou padrão do período.
    /// </remarks>
    public override Period? Previous() => null;

    /// <inheritdoc />
    /// <remarks>
    /// Para períodos customizados, não é possível determinar o período anterior ou próximo
    /// sem conhecimento adicional sobre a duração ou padrão do período.
    /// </remarks>
    public override Period? Next() => null;


    /// <summary>
    /// Retorna uma string formatada: "{Label} (yyyy-MM-dd – yyyy-MM-dd)".
    /// </summary>
    public override string ToString()
        => $"{Label} ({Start:yyyy-MM-dd} – {End:yyyy-MM-dd})";
}
