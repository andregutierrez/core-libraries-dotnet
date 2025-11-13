namespace Core.Libraries.Domain.ValueObjects.Periods;

/// <summary>
/// Representa um período de um ano calendário completo (12 meses).
/// </summary>
public record YearPeriod : Period
{
    /// <summary>
    /// Obtém o ano do período.
    /// </summary>
    public int Year { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="YearPeriod"/>.
    /// </summary>
    /// <param name="year">O ano (1–9999).</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando <paramref name="year"/> está fora do range válido.
    /// </exception>
    public YearPeriod(int year)
        : base(StartOf(year), EndOf(year))
    {
        ValidateYear(year);
        Year = year;
    }

    /// <summary>
    /// Obtém o primeiro dia do ano.
    /// </summary>
    private static DateOnly StartOf(int year)
        => new DateOnly(year, 1, 1);

    /// <summary>
    /// Obtém o último dia do ano.
    /// </summary>
    private static DateOnly EndOf(int year)
    {
        int lastDay = DateTime.DaysInMonth(year, 12);
        return new DateOnly(year, 12, lastDay);
    }

    /// <summary>
    /// Cria um <see cref="YearPeriod"/> contendo a data especificada.
    /// </summary>
    /// <param name="date">A data a ser usada.</param>
    /// <returns>Um novo <see cref="YearPeriod"/>.</returns>
    public static YearPeriod FromDate(DateOnly date)
        => new YearPeriod(date.Year);

    /// <inheritdoc />
    public override Period? Previous()
        => new YearPeriod(Year - 1);

    /// <inheritdoc />
    public override Period? Next()
        => new YearPeriod(Year + 1);

    /// <summary>
    /// Retorna o ano como string, ex: "2025".
    /// </summary>
    public override string ToString()
        => Year.ToString();
}
