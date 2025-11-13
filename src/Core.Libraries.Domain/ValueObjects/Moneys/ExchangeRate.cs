namespace Core.Libraries.Domain.ValueObjects.Moneys;

using System;

/// <summary>
/// Representa uma taxa de câmbio entre duas moedas.
/// </summary>
public record ExchangeRate
{
    /// <summary>Moeda de origem.</summary>
    public Currency From { get; }

    /// <summary>Moeda de destino.</summary>
    public Currency To { get; }

    /// <summary>Quantas unidades de <see cref="To"/> para uma unidade de <see cref="From"/>.</summary>
    public decimal Rate { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected ExchangeRate()
    {
        From = default!;
        To = default!;
        Rate = default;
    }

    /// <summary>
    /// Cria um novo ExchangeRate. A taxa deve ser positiva.
    /// </summary>
    /// <param name="from">Moeda de origem.</param>
    /// <param name="to">Moeda de destino.</param>
    /// <param name="rate">Taxa de câmbio.</param>
    /// <exception cref="ArgumentNullException">
    /// Lançada se <paramref name="from"/> ou <paramref name="to"/> forem null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="rate"/> for menor ou igual a zero.
    /// </exception>
    public ExchangeRate(Currency from, Currency to, decimal rate)
    {
        From = from ?? throw new ArgumentNullException(nameof(from));
        To = to ?? throw new ArgumentNullException(nameof(to));

        if (rate <= 0m)
            throw new ArgumentException("Exchange rate must be positive.", nameof(rate));

        Rate = rate;
    }

    /// <summary>
    /// Converte um Money em <see cref="From"/> para um novo Money em <see cref="To"/>.
    /// </summary>
    /// <param name="source">O Money a ser convertido.</param>
    /// <returns>Um novo Money na moeda de destino.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se a moeda de origem não corresponder.
    /// </exception>
    public Money Convert(Money source)
    {
        if (!source.Currency.Equals(From))
            throw new InvalidOperationException("Source currency mismatch.");

        return new Money(source.Amount * Rate, To);
    }

    /// <summary>
    /// Retorna uma string formatada, ex: "1 USD = 1.234567 EUR".
    /// </summary>
    public override string ToString()
        => $"1 {From.Code} = {Rate:N6} {To.Code}";
}
