namespace Core.Libraries.Domain.ValueObjects.Periods;

/// <summary>
/// Representa um período de três meses (trimestre) dentro de um ano específico.
/// </summary>
public record TrimesterPeriod : Period
{
    /// <summary>
    /// Obtém o número do trimestre (1 = Jan–Mar, …, 4 = Out–Dez).
    /// </summary>
    public int Trimester { get; }

    /// <summary>
    /// Obtém o ano do período.
    /// </summary>
    public int Year { get; }

    private const int FirstTrimester = 1;
    private const int LastTrimester = 4;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TrimesterPeriod"/>.
    /// </summary>
    /// <param name="trimester">O trimestre (1–4).</param>
    /// <param name="year">O ano.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando <paramref name="trimester"/> está fora do range ou o ano é inválido.
    /// </exception>
    public TrimesterPeriod(int trimester, int year)
        : base(StartOf(trimester, year), EndOf(trimester, year))
    {
        ValidateYear(year);
        if (trimester < FirstTrimester || trimester > LastTrimester)
            throw new ArgumentException("Trimester must be between 1 and 4.", nameof(trimester));

        Trimester = trimester;
        Year = year;
    }

    /// <summary>
    /// Calcula o primeiro dia do trimestre dado.
    /// </summary>
    private static DateOnly StartOf(int trimester, int year)
        => new DateOnly(year, (trimester - 1) * 3 + 1, 1);

    /// <summary>
    /// Calcula o último dia do trimestre dado.
    /// </summary>
    private static DateOnly EndOf(int trimester, int year)
    {
        int lastMonth = trimester * 3;
        int lastDay = DateTime.DaysInMonth(year, lastMonth);
        return new DateOnly(year, lastMonth, lastDay);
    }

    /// <summary>
    /// Cria um <see cref="TrimesterPeriod"/> contendo a data especificada.
    /// </summary>
    /// <param name="date">A data a ser usada.</param>
    /// <returns>Um novo <see cref="TrimesterPeriod"/>.</returns>
    public static TrimesterPeriod FromDate(DateOnly date)
    {
        int computed = ((date.Month - 1) / 3) + 1;
        return new TrimesterPeriod(computed, date.Year);
    }

    /// <inheritdoc />
    public override Period? Previous()
        => Trimester == FirstTrimester
            ? new TrimesterPeriod(LastTrimester, Year - 1)
            : new TrimesterPeriod(Trimester - 1, Year);

    /// <inheritdoc />
    public override Period? Next()
        => Trimester == LastTrimester
            ? new TrimesterPeriod(FirstTrimester, Year + 1)
            : new TrimesterPeriod(Trimester + 1, Year);

    /// <summary>
    /// Retorna uma representação string curta, ex: "2025-T02".
    /// </summary>
    public override string ToString()
        => $"{Year}-T{Trimester:D2}";
}