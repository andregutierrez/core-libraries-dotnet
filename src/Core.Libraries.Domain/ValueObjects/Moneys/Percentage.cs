namespace Core.Libraries.Domain.ValueObjects.Moneys;

using System;

/// <summary>
/// Representa uma porcentagem entre 0.0 (0%) e 1.0 (100%).
/// </summary>
public record Percentage
{
    /// <summary>Valor como fração. Ex: 0.25 = 25%.</summary>
    public decimal Value { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Percentage() => Value = default;

    /// <summary>
    /// Cria uma nova Percentage. Deve estar entre 0.0 e 1.0 (inclusive).
    /// </summary>
    /// <param name="value">O valor da porcentagem como fração (0.0 a 1.0).</param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="value"/> estiver fora do range válido.
    /// </exception>
    public Percentage(decimal value)
    {
        if (value < 0m || value > 1m)
            throw new ArgumentException("Percentage must be between 0 and 1.", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Aplica esta porcentagem sobre um Money, retornando o valor calculado.
    /// </summary>
    /// <param name="money">O Money sobre o qual aplicar a porcentagem.</param>
    /// <returns>Um novo Money com o valor calculado.</returns>
    public Money ApplyTo(Money money)
        => money.Multiply(Value);

    /// <summary>
    /// Retorna uma string formatada, ex: "25.00 %".
    /// </summary>
    public override string ToString()
        => $"{Value:P}";
}
