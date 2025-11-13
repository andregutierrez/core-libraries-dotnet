namespace Core.Libraries.Domain.ValueObjects.Moneys;

using System;

/// <summary>
/// Representa uma quantia de dinheiro em uma moeda específica.
/// </summary>
public record Money : IComparable<Money>
{
    /// <summary>
    /// A quantia de dinheiro arredondada.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// A moeda da quantia.
    /// </summary>
    public Currency Currency { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Money()
    {
        Amount = default;
        Currency = default!;
    }

    /// <summary>
    /// Inicializa um novo valor Money.
    /// </summary>
    /// <param name="amount">A quantia monetária bruta.</param>
    /// <param name="currency">O objeto de valor da moeda.</param>
    /// <exception cref="ArgumentNullException">
    /// Lançada se <paramref name="currency"/> for null.
    /// </exception>
    public Money(decimal amount, Currency currency)
    {
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        // Round according to currency precision
        Amount = Math.Round(amount, currency.DecimalPlaces);
    }

    /// <summary>
    /// Adiciona dois valores Money da mesma moeda.
    /// </summary>
    /// <param name="other">O outro valor Money a ser adicionado.</param>
    /// <returns>Um novo Money com a soma dos valores.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se as moedas forem diferentes.
    /// </exception>
    public Money Add(Money other)
    {
        if (!Currency.Equals(other.Currency))
            throw new InvalidOperationException("Cannot add amounts with different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }

    /// <summary>
    /// Subtrai outro Money deste (mesma moeda).
    /// </summary>
    /// <param name="other">O outro valor Money a ser subtraído.</param>
    /// <returns>Um novo Money com a diferença dos valores.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se as moedas forem diferentes.
    /// </exception>
    public Money Subtract(Money other)
    {
        if (!Currency.Equals(other.Currency))
            throw new InvalidOperationException("Cannot subtract amounts with different currencies.");
        return new Money(Amount - other.Amount, Currency);
    }

    /// <summary>
    /// Multiplica a quantia pelo fator especificado.
    /// </summary>
    /// <param name="factor">O fator de multiplicação.</param>
    /// <returns>Um novo Money com a quantia multiplicada.</returns>
    public Money Multiply(decimal factor)
        => new Money(Amount * factor, Currency);

    /// <summary>
    /// Divide a quantia pelo divisor especificado.
    /// </summary>
    /// <param name="divisor">O divisor.</param>
    /// <returns>Um novo Money com a quantia dividida.</returns>
    public Money Divide(decimal divisor)
        => new Money(Amount / divisor, Currency);

    /// <summary>
    /// Compara esta instância com outro valor Money.
    /// A comparação só é válida se ambas as instâncias tiverem a mesma moeda.
    /// </summary>
    /// <param name="other">O Money a ser comparado.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa dos valores sendo comparados.
    /// Retorna um número negativo se este valor for menor que <paramref name="other"/>,
    /// zero se forem iguais, ou um número positivo se este valor for maior.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se as moedas forem diferentes.
    /// </exception>
    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        if (!Currency.Equals(other.Currency))
            throw new InvalidOperationException("Cannot compare Money values with different currencies.");
        return Amount.CompareTo(other.Amount);
    }

    /// <summary>
    /// Operador menor que (&lt;).
    /// </summary>
    public static bool operator <(Money? left, Money? right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Operador menor ou igual que (&lt;=).
    /// </summary>
    public static bool operator <=(Money? left, Money? right)
        => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Operador maior que (&gt;).
    /// </summary>
    public static bool operator >(Money? left, Money? right)
        => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Operador maior ou igual que (&gt;=).
    /// </summary>
    public static bool operator >=(Money? left, Money? right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    /// <summary>
    /// Retorna uma string formatada, ex: "USD 123.45".
    /// </summary>
    public override string ToString()
        => $"{Currency.Code} {Amount.ToString("N" + Currency.DecimalPlaces)}";
}
