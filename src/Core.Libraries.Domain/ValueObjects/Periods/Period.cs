namespace Core.Libraries.Domain.ValueObjects.Periods;

using System;

/// <summary>
/// Classe base abstrata para representar períodos de tempo com datas de início e fim.
/// </summary>
public abstract record Period : IComparable<Period>
{
    /// <summary>
    /// Data de início inclusiva do período.
    /// </summary>
    public DateOnly Start { get; init; }

    /// <summary>
    /// Data de fim inclusiva do período.
    /// </summary>
    public DateOnly End { get; init; }

    /// <summary>
    /// Ano mínimo suportado (1).
    /// </summary>
    protected const int MinYear = 1;

    /// <summary>
    /// Ano máximo suportado (9999, limite do DateOnly).
    /// </summary>
    protected const int MaxYear = 9999;

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Period() { }

    /// <summary>
    /// Cria um período, garantindo que End ≥ Start.
    /// </summary>
    /// <param name="start">Data de início do período.</param>
    /// <param name="end">Data de fim do período.</param>
    /// <exception cref="ArgumentException">Quando End for anterior a Start.</exception>
    protected Period(DateOnly start, DateOnly end)
    {
        if (end < start)
            throw new ArgumentException("End date cannot precede start date.", nameof(end));

        Start = start;
        End = end;
    }

    /// <summary>
    /// Valida se o ano está no range válido (1-9999).
    /// </summary>
    /// <param name="year">O ano a ser validado.</param>
    /// <exception cref="ArgumentException">Quando o ano está fora do range válido.</exception>
    protected static void ValidateYear(int year)
    {
        if (year < MinYear || year > MaxYear)
            throw new ArgumentException($"Year must be between {MinYear} and {MaxYear}.", nameof(year));
    }

    /// <summary>
    /// Verifica se <paramref name="date"/> está dentro do intervalo (inclusive).
    /// </summary>
    /// <param name="date">A data a ser verificada.</param>
    /// <returns><c>true</c> se a data estiver dentro do período; caso contrário, <c>false</c>.</returns>
    public bool Contains(DateOnly date)
        => date >= Start && date <= End;

    /// <summary>
    /// Obtém o número de dias do período (inclusive).
    /// </summary>
    public int Days => (End.ToDateTime(TimeOnly.MinValue) - Start.ToDateTime(TimeOnly.MinValue)).Days + 1;

    /// <summary>
    /// Verifica se este período sobrepõe com outro período.
    /// </summary>
    /// <param name="other">O outro período a ser verificado.</param>
    /// <returns><c>true</c> se os períodos se sobrepõem; caso contrário, <c>false</c>.</returns>
    public bool OverlapsWith(Period other)
    {
        if (other == null) return false;
        return Start <= other.End && End >= other.Start;
    }

    /// <summary>
    /// Verifica se este período é consecutivo com outro período (um começa quando o outro termina).
    /// </summary>
    /// <param name="other">O outro período a ser verificado.</param>
    /// <returns><c>true</c> se os períodos são consecutivos; caso contrário, <c>false</c>.</returns>
    public bool IsConsecutiveWith(Period other)
    {
        if (other == null) return false;
        return End.AddDays(1) == other.Start || other.End.AddDays(1) == Start;
    }

    /// <summary>
    /// Obtém o período anterior.
    /// </summary>
    /// <returns>O período anterior, ou <c>null</c> se não houver implementação.</returns>
    public abstract Period? Previous();

    /// <summary>
    /// Obtém o próximo período.
    /// </summary>
    /// <returns>O próximo período, ou <c>null</c> se não houver implementação.</returns>
    public abstract Period? Next();

    /// <summary>
    /// Compara esta instância com outro período.
    /// A comparação é baseada na data de início (Start).
    /// </summary>
    /// <param name="other">O período a ser comparado.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa dos períodos sendo comparados.
    /// Retorna um número negativo se este período começar antes de <paramref name="other"/>,
    /// zero se começarem na mesma data, ou um número positivo se este período começar depois.
    /// </returns>
    public int CompareTo(Period? other)
    {
        if (other is null) return 1;
        return Start.CompareTo(other.Start);
    }

    /// <summary>
    /// Operador menor que (&lt;). Compara pela data de início.
    /// </summary>
    public static bool operator <(Period? left, Period? right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Operador menor ou igual que (&lt;=). Compara pela data de início.
    /// </summary>
    public static bool operator <=(Period? left, Period? right)
        => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Operador maior que (&gt;). Compara pela data de início.
    /// </summary>
    public static bool operator >(Period? left, Period? right)
        => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Operador maior ou igual que (&gt;=). Compara pela data de início.
    /// </summary>
    public static bool operator >=(Period? left, Period? right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    /// <summary>
    /// Formato padrão yyyy-MM-dd – yyyy-MM-dd.
    /// </summary>
    public override string ToString()
        => $"{Start:yyyy-MM-dd} – {End:yyyy-MM-dd}";
}
