namespace Core.Libraries.Domain.ValueObjects.Periods;

/// <summary>
/// Representa um período de quatro meses (quadrimestre) dentro de um ano específico.
/// </summary>
public record QuadrimesterPeriod : Period
{
    /// <summary>
    /// Obtém o número do quadrimestre (1 = Jan–Abr, 2 = Mai–Ago, 3 = Set–Dez).
    /// </summary>
    public int Quadrimester { get; }

    /// <summary>
    /// Obtém o ano do período.
    /// </summary>
    public int Year { get; }

    private const int FirstQuadrimester = 1;
    private const int LastQuadrimester = 3;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="QuadrimesterPeriod"/>.
    /// </summary>
    /// <param name="quadrimester">O quadrimestre (1–3).</param>
    /// <param name="year">O ano.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando <paramref name="quadrimester"/> está fora do range ou o ano é inválido.
    /// </exception>
    public QuadrimesterPeriod(int quadrimester, int year)
        : base(StartOf(quadrimester, year), EndOf(quadrimester, year))
    {
        ValidateYear(year);
        if (quadrimester < FirstQuadrimester || quadrimester > LastQuadrimester)
            throw new ArgumentException("Quadrimester must be between 1 and 3.", nameof(quadrimester));

        Quadrimester = quadrimester;
        Year = year;
    }

    /// <summary>
    /// Calcula o primeiro dia do quadrimestre dado.
    /// </summary>
    private static DateOnly StartOf(int quadrimester, int year)
        => new DateOnly(year, (quadrimester - 1) * 4 + 1, 1);

    /// <summary>
    /// Calcula o último dia do quadrimestre dado.
    /// </summary>
    private static DateOnly EndOf(int quadrimester, int year)
    {
        int lastMonth = quadrimester * 4;
        int lastDay = DateTime.DaysInMonth(year, lastMonth);
        return new DateOnly(year, lastMonth, lastDay);
    }

    /// <summary>
    /// Cria um <see cref="QuadrimesterPeriod"/> que contém a data especificada.
    /// </summary>
    /// <param name="date">A data a ser usada.</param>
    /// <returns>Um novo <see cref="QuadrimesterPeriod"/>.</returns>
    public static QuadrimesterPeriod FromDate(DateOnly date)
    {
        int computed = ((date.Month - 1) / 4) + 1;
        return new QuadrimesterPeriod(computed, date.Year);
    }

    /// <inheritdoc />
    public override Period? Previous()
        => Quadrimester == FirstQuadrimester
            ? new QuadrimesterPeriod(LastQuadrimester, Year - 1)
            : new QuadrimesterPeriod(Quadrimester - 1, Year);

    /// <inheritdoc />
    public override Period? Next()
        => Quadrimester == LastQuadrimester
            ? new QuadrimesterPeriod(FirstQuadrimester, Year + 1)
            : new QuadrimesterPeriod(Quadrimester + 1, Year);

    /// <summary>
    /// Retorna uma representação string curta, ex: "2025-Q01".
    /// </summary>
    public override string ToString()
        => $"{Year}-Q{Quadrimester:D2}";
}