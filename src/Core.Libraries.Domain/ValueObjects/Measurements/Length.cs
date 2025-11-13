namespace Core.Libraries.Domain.ValueObjects.Measurements;

using System;

/// <summary>
/// Representa uma medida de comprimento, armazenada internamente em metros.
/// </summary>
public record Length : IComparable<Length>
{
    /// <summary>
    /// Valor em metros (deve ser ≥ 0).
    /// </summary>
    public decimal Meters { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Length() => Meters = default;

    /// <summary>
    /// Inicializa um novo Length.
    /// </summary>
    /// <param name="meters">Comprimento em metros. Não pode ser negativo.</param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="meters"/> for negativo.
    /// </exception>
    public Length(decimal meters)
    {
        if (meters < 0m)
            throw new ArgumentException("Length must be non-negative.", nameof(meters));

        Meters = meters;
    }

    /// <summary>
    /// Cria um Length a partir de quilômetros.
    /// </summary>
    /// <param name="km">O valor em quilômetros.</param>
    /// <returns>Um novo Length.</returns>
    public static Length FromKilometers(decimal km)
        => new Length(km * 1_000m);

    /// <summary>
    /// Cria um Length a partir de centímetros.
    /// </summary>
    /// <param name="cm">O valor em centímetros.</param>
    /// <returns>Um novo Length.</returns>
    public static Length FromCentimeters(decimal cm)
        => new Length(cm / 100m);

    /// <summary>
    /// Converte este comprimento para quilômetros.
    /// </summary>
    /// <returns>O valor em quilômetros.</returns>
    public decimal ToKilometers()
        => Meters / 1_000m;

    /// <summary>
    /// Converte este comprimento para centímetros.
    /// </summary>
    /// <returns>O valor em centímetros.</returns>
    public decimal ToCentimeters()
        => Meters * 100m;

    /// <summary>
    /// Adiciona dois comprimentos.
    /// </summary>
    /// <param name="other">O outro comprimento a ser adicionado.</param>
    /// <returns>Um novo Length com a soma dos comprimentos.</returns>
    public Length Add(Length other)
        => new Length(Meters + other.Meters);

    /// <summary>
    /// Subtrai outro comprimento.
    /// </summary>
    /// <param name="other">O outro comprimento a ser subtraído.</param>
    /// <returns>Um novo Length com a diferença dos comprimentos.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se o resultado for negativo.
    /// </exception>
    public Length Subtract(Length other)
    {
        var result = Meters - other.Meters;
        if (result < 0m)
            throw new InvalidOperationException("Resulting length cannot be negative.");
        return new Length(result);
    }

    /// <summary>
    /// Multiplica este comprimento por um fator.
    /// </summary>
    /// <param name="factor">O fator de multiplicação.</param>
    /// <returns>Um novo Length com o comprimento multiplicado.</returns>
    public Length Multiply(decimal factor)
        => new Length(Meters * factor);

    /// <summary>
    /// Compara esta instância com outro comprimento.
    /// </summary>
    /// <param name="other">O comprimento a ser comparado.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa dos comprimentos sendo comparados.
    /// Retorna um número negativo se este comprimento for menor que <paramref name="other"/>,
    /// zero se forem iguais, ou um número positivo se este comprimento for maior.
    /// </returns>
    public int CompareTo(Length? other)
    {
        if (other is null) return 1;
        return Meters.CompareTo(other.Meters);
    }

    /// <summary>
    /// Operador menor que (&lt;).
    /// </summary>
    public static bool operator <(Length? left, Length? right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Operador menor ou igual que (&lt;=).
    /// </summary>
    public static bool operator <=(Length? left, Length? right)
        => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Operador maior que (&gt;).
    /// </summary>
    public static bool operator >(Length? left, Length? right)
        => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Operador maior ou igual que (&gt;=).
    /// </summary>
    public static bool operator >=(Length? left, Length? right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    /// <summary>
    /// Retorna uma string formatada, ex: "10.00 m".
    /// </summary>
    public override string ToString()
        => $"{Meters:N2} m";
}
