namespace Core.Libraries.Domain.ValueObjects.Periods;

using System.Globalization;

/// <summary>
/// Representa um período de uma semana (ISO-8601) dentro de um ano específico.
/// </summary>
public record WeekPeriod : Period
{
    /// <summary>
    /// Obtém o número da semana ISO (1–52/53).
    /// </summary>
    public int Week { get; }

    /// <summary>
    /// Obtém o ano do período.
    /// </summary>
    public int Year { get; }

    private const int MinWeek = 1;
    private static readonly Calendar IsoCalendar = CultureInfo.InvariantCulture.Calendar;
    private const CalendarWeekRule IsoRule = CalendarWeekRule.FirstFourDayWeek;
    private const DayOfWeek IsoFirstDay = DayOfWeek.Monday;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="WeekPeriod"/>.
    /// </summary>
    /// <param name="week">O número da semana ISO-8601.</param>
    /// <param name="year">O ano.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando <paramref name="week"/> está fora do range válido de semanas ISO para o ano,
    /// ou quando o ano é inválido.
    /// </exception>
    public WeekPeriod(int week, int year)
        : base(StartOf(week, year), EndOf(week, year))
    {
        ValidateYear(year);
        int maxWeek = GetWeeksInYear(year);
        if (week < MinWeek || week > maxWeek)
            throw new ArgumentException($"Week must be between {MinWeek} and {maxWeek} for year {year}.", nameof(week));

        Week = week;
        Year = year;
    }

    private static DateOnly StartOf(int week, int year)
    {
        // Find Thursday of the first ISO week
        var jan1 = new DateTime(year, 1, 1);
        int offset = IsoFirstDay - jan1.DayOfWeek;
        var firstMonday = jan1.AddDays(offset);
        if (CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(firstMonday, IsoRule, IsoFirstDay) != 1)
            firstMonday = firstMonday.AddDays(7);

        var start = firstMonday.AddDays((week - 1) * 7);
        return DateOnly.FromDateTime(start);
    }

    private static DateOnly EndOf(int week, int year)
        => StartOf(week, year).AddDays(6);

    private static int GetWeeksInYear(int year)
    {
        // ISO week count is week number of Dec 31 (or the last Thursday)
        var dec31 = new DateTime(year, 12, 31);
        int week = IsoCalendar.GetWeekOfYear(dec31, IsoRule, IsoFirstDay);
        if (week == 1)
            week = IsoCalendar.GetWeekOfYear(dec31.AddDays(-7), IsoRule, IsoFirstDay);
        return week;
    }

    /// <summary>
    /// Cria o <see cref="WeekPeriod"/> contendo a data especificada.
    /// </summary>
    /// <param name="date">A data a ser usada.</param>
    /// <returns>Um novo <see cref="WeekPeriod"/>.</returns>
    public static WeekPeriod FromDate(DateOnly date)
    {
        int w = IsoCalendar.GetWeekOfYear(date.ToDateTime(TimeOnly.MinValue), IsoRule, IsoFirstDay);
        return new WeekPeriod(w, date.Year);
    }

    /// <inheritdoc />
    public override Period? Previous()
        => Week == MinWeek
            ? new WeekPeriod(GetWeeksInYear(Year - 1), Year - 1)
            : new WeekPeriod(Week - 1, Year);

    /// <inheritdoc />
    public override Period? Next()
        => Week == GetWeeksInYear(Year)
            ? new WeekPeriod(MinWeek, Year + 1)
            : new WeekPeriod(Week + 1, Year);

    /// <summary>
    /// Retorna uma string curta, ex: "2025-W03".
    /// </summary>
    public override string ToString()
        => $"{Year}-W{Week:D2}";
}