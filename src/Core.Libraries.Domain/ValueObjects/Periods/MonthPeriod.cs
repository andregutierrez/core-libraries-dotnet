namespace Core.Libraries.Domain.ValueObjects.Periods;

/// <summary>
/// Representa um período de um único mês dentro de um ano específico.
/// </summary>
public record MonthPeriod : Period
{
    /// <summary>
    /// Obtém o número do mês (1 = Janeiro, 12 = Dezembro).
    /// </summary>
    public int Month { get; }

    /// <summary>
    /// Obtém o ano do período.
    /// </summary>
    public int Year { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="MonthPeriod"/>.
    /// </summary>
    /// <param name="month">O mês (1-12).</param>
    /// <param name="year">O ano.</param>
    /// <exception cref="ArgumentException">Lançada quando o mês está fora do range válido ou o ano é inválido.</exception>
    public MonthPeriod(int month, int year)
        : base(GetStartDate(month, year), GetEndDate(month, year))
    {
        ValidateYear(year);
        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be between 1 and 12.", nameof(month));

        Month = month;
        Year = year;
    }

    /// <summary>
    /// Obtém o primeiro dia do período.
    /// </summary>
    /// <param name="month">O mês (1-12).</param>
    /// <param name="year">O ano.</param>
    /// <returns>O primeiro dia como <see cref="DateOnly"/>.</returns>
    private static DateOnly GetStartDate(int month, int year)
        => new DateOnly(year, month, 1);

    /// <summary>
    /// Obtém o último dia do período.
    /// </summary>
    /// <param name="month">O mês (1-12).</param>
    /// <param name="year">O ano.</param>
    /// <returns>O último dia como <see cref="DateOnly"/>.</returns>
    private static DateOnly GetEndDate(int month, int year)
    {
        var lastDay = DateTime.DaysInMonth(year, month);
        return new DateOnly(year, month, lastDay);
    }

    /// <summary>
    /// Cria um <see cref="MonthPeriod"/> a partir de uma data.
    /// </summary>
    /// <param name="date">A data da qual extrair mês e ano.</param>
    /// <returns>Um novo <see cref="MonthPeriod"/>.</returns>
    public static MonthPeriod FromDate(DateOnly date)
        => new MonthPeriod(date.Month, date.Year);

    /// <inheritdoc />
    public override Period? Previous()
    {
        int prevMonth = Month == 1 ? 12 : Month - 1;
        int prevYear = Month == 1 ? Year - 1 : Year;
        return new MonthPeriod(prevMonth, prevYear);
    }

    /// <inheritdoc />
    public override Period? Next()
    {
        int nextMonth = Month == 12 ? 1 : Month + 1;
        int nextYear = Month == 12 ? Year + 1 : Year;
        return new MonthPeriod(nextMonth, nextYear);
    }

    /// <summary>
    /// Retorna o período no formato "yyyy-MM".
    /// </summary>
    public override string ToString()
        => $"{Year}-{Month:D2}";
}
