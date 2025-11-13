namespace Core.Libraries.Domain.ValueObjects.Periods;

/// <summary>
/// Representa um período de dois meses (bimestre) dentro de um ano específico.
/// </summary>
public record BimesterPeriod : Period
{
    /// <summary>
    /// Obtém o número do bimestre (1 = Jan–Fev, …, 6 = Nov–Dez).
    /// </summary>
    public int Bimester { get; }

    /// <summary>
    /// Obtém o ano do período.
    /// </summary>
    public int Year { get; }

    private const int FirstBimester = 1;
    private const int LastBimester = 6;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="BimesterPeriod"/>.
    /// </summary>
    /// <param name="bimester">O bimestre (1–6).</param>
    /// <param name="year">O ano.</param>
    /// <exception cref="ArgumentException">Lançada quando <paramref name="bimester"/> está fora do range ou o ano é inválido.</exception>
    public BimesterPeriod(int bimester, int year)
        : base(StartOf(bimester, year), EndOf(bimester, year))
    {
        ValidateYear(year);
        if (bimester < FirstBimester || bimester > LastBimester)
            throw new ArgumentException("Bimester must be between 1 and 6.", nameof(bimester));

        Bimester = bimester;
        Year = year;
    }

    /// <summary>
    /// Calcula o primeiro dia do bimestre dado.
    /// </summary>
    private static DateOnly StartOf(int bimester, int year)
        => new DateOnly(year, (bimester - 1) * 2 + 1, 1);

    /// <summary>
    /// Calcula o último dia do bimestre dado.
    /// </summary>
    private static DateOnly EndOf(int bimester, int year)
    {
        int lastMonth = bimester * 2;
        int lastDay = DateTime.DaysInMonth(year, lastMonth);
        return new DateOnly(year, lastMonth, lastDay);
    }

    /// <summary>
    /// Cria um <see cref="BimesterPeriod"/> que contém a data especificada.
    /// </summary>
    /// <param name="date">A data a ser usada.</param>
    /// <returns>Um novo <see cref="BimesterPeriod"/>.</returns>
    public static BimesterPeriod FromDate(DateOnly date)
    {
        int computed = ((date.Month - 1) / 2) + 1;
        return new BimesterPeriod(computed, date.Year);
    }

    /// <inheritdoc />
    public override Period? Previous()
        => Bimester == FirstBimester
            ? new BimesterPeriod(LastBimester, Year - 1)
            : new BimesterPeriod(Bimester - 1, Year);

    /// <inheritdoc />
    public override Period? Next()
        => Bimester == LastBimester
            ? new BimesterPeriod(FirstBimester, Year + 1)
            : new BimesterPeriod(Bimester + 1, Year);

    /// <summary>
    /// Retorna uma representação string curta, ex: "2025-B02".
    /// </summary>
    public override string ToString()
        => $"{Year}-B{Bimester:D2}";
}
