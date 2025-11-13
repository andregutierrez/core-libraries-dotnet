namespace Core.Libraries.Domain.ValueObjects.Moneys;

using System;

/// <summary>
/// Representa um intervalo monetário [Min, Max] na mesma moeda.
/// </summary>
public record MoneyRange
{
    /// <summary>
    /// O valor mínimo do intervalo.
    /// </summary>
    public Money Min { get; }

    /// <summary>
    /// O valor máximo do intervalo.
    /// </summary>
    public Money Max { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected MoneyRange()
    {
        Min = default!;
        Max = default!;
    }

    /// <summary>
    /// Cria um MoneyRange. Min ≤ Max e mesma moeda.
    /// </summary>
    /// <param name="min">O valor mínimo do intervalo.</param>
    /// <param name="max">O valor máximo do intervalo.</param>
    /// <exception cref="ArgumentException">
    /// Lançada se as moedas não corresponderem ou se Min > Max.
    /// </exception>
    public MoneyRange(Money min, Money max)
    {
        if (!min.Currency.Equals(max.Currency))
            throw new ArgumentException("Currencies must match.", nameof(max));

        if (min.Amount > max.Amount)
            throw new ArgumentException("Min cannot exceed Max.", nameof(max));

        Min = min;
        Max = max;
    }

    /// <summary>
    /// Verifica se o valor especificado está dentro de [Min, Max].
    /// </summary>
    /// <param name="value">O valor Money a ser verificado.</param>
    /// <returns><c>true</c> se o valor estiver dentro do intervalo; caso contrário, <c>false</c>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se a moeda não corresponder.
    /// </exception>
    public bool Contains(Money value)
    {
        if (!value.Currency.Equals(Min.Currency))
            throw new InvalidOperationException("Currency mismatch.");

        return value.Amount >= Min.Amount && value.Amount <= Max.Amount;
    }

    /// <summary>
    /// Retorna uma string formatada, ex: "USD 10.00 – USD 20.00".
    /// </summary>
    public override string ToString()
        => $"{Min} – {Max}";
}