namespace Core.Libraries.Domain.ValueObjects.Moneys;

using System;

/// <summary>
/// Representa uma moeda pelo seu código ISO 4217 e precisão decimal.
/// </summary>
public record Currency
{
    /// <summary>
    /// O código ISO 4217 de 3 letras (ex: "USD", "EUR", "BRL").
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Número de casas decimais usadas por esta moeda (geralmente 2).
    /// </summary>
    public int DecimalPlaces { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Currency()
    {
        Code = default!;
        DecimalPlaces = default;
    }

    /// <summary>
    /// Inicializa uma nova Currency.
    /// </summary>
    /// <param name="code">Código de moeda de 3 letras.</param>
    /// <param name="decimalPlaces">Precisão decimal (deve ser ≥ 0).</param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="code"/> não tiver exatamente 3 letras
    /// ou se <paramref name="decimalPlaces"/> for negativo.
    /// </exception>
    public Currency(string code, int decimalPlaces = 2)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            throw new ArgumentException("Currency code must be a 3-letter ISO 4217 code.", nameof(code));

        Code = code.ToUpperInvariant();

        if (decimalPlaces < 0)
            throw new ArgumentException("Decimal places must be non-negative.", nameof(decimalPlaces));

        DecimalPlaces = decimalPlaces;
    }

    /// <summary>
    /// Retorna o código ISO (ex: "USD").
    /// </summary>
    public override string ToString() => Code;
}
