namespace Core.Libraries.Domain.ValueObjects.Periods;

/// <summary>
/// Representa um dos 24 períodos de meio mês (quinzenas) dentro de um ano específico.
/// </summary>
public record FortnightPeriod : Period
{
    /// <summary>
    /// Obtém o número da quinzena:
    /// 1 = Jan 1–15, 2 = Jan 16–31, 3 = Fev 1–15, …, 24 = Dez 16–31.
    /// </summary>
    public int Fortnight { get; }

    /// <summary>
    /// Obtém o ano deste período.
    /// </summary>
    public int Year { get; }

    private const int FirstFortnight = 1;
    private const int LastFortnight = 24;

    /// <summary>
    /// Cria um novo <see cref="FortnightPeriod"/>.
    /// </summary>
    /// <param name="fortnight">A quinzena (1–24).</param>
    /// <param name="year">O ano.</param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="fortnight"/> está fora do range 1–24 ou se o ano é inválido.
    /// </exception>
    public FortnightPeriod(int fortnight, int year)
        : base(StartOf(fortnight, year), EndOf(fortnight, year))
    {
        ValidateYear(year);
        if (fortnight < FirstFortnight || fortnight > LastFortnight)
            throw new ArgumentException($"Fortnight must be between {FirstFortnight} and {LastFortnight}.",
                                        nameof(fortnight));

        Fortnight = fortnight;
        Year = year;
    }

    /// <summary>
    /// Calcula a data de início da quinzena dada.
    /// </summary>
    private static DateOnly StartOf(int fortnight, int year)
    {
        int month = ((fortnight - 1) / 2) + 1;
        bool secondHalf = ((fortnight - 1) % 2) == 1;
        int day = secondHalf ? 16 : 1;
        return new DateOnly(year, month, day);
    }

    /// <summary>
    /// Calcula a data de fim da quinzena dada.
    /// </summary>
    private static DateOnly EndOf(int fortnight, int year)
    {
        int month = ((fortnight - 1) / 2) + 1;
        bool secondHalf = ((fortnight - 1) % 2) == 1;
        int day = secondHalf
                         ? DateTime.DaysInMonth(year, month)
                         : 15;
        return new DateOnly(year, month, day);
    }

    /// <summary>
    /// Cria o <see cref="FortnightPeriod"/> contendo a data especificada.
    /// </summary>
    /// <param name="date">A data a ser usada.</param>
    /// <returns>Um novo <see cref="FortnightPeriod"/>.</returns>
    public static FortnightPeriod FromDate(DateOnly date)
    {
        int half = date.Day <= 15 ? 1 : 2;
        int number = ((date.Month - 1) * 2) + half;
        return new FortnightPeriod(number, date.Year);
    }

    /// <inheritdoc />
    public override Period? Previous()
        => Fortnight == FirstFortnight
           ? new FortnightPeriod(LastFortnight, Year - 1)
           : new FortnightPeriod(Fortnight - 1, Year);

    /// <inheritdoc />
    public override Period? Next()
        => Fortnight == LastFortnight
           ? new FortnightPeriod(FirstFortnight, Year + 1)
           : new FortnightPeriod(Fortnight + 1, Year);

    /// <summary>
    /// Retorna uma label curta, ex: "2025-F01".
    /// </summary>
    public override string ToString()
        => $"{Year}-F{Fortnight:D2}";
}
