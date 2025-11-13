namespace Core.Libraries.Domain.ValueObjects.Measurements;

using System;

/// <summary>
/// Representa uma medida de temperatura, armazenada internamente em kelvin.
/// </summary>
public record Temperature : IComparable<Temperature>
{
    /// <summary>Temperatura em kelvin (≥ 0).</summary>
    public decimal Kelvin { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Temperature() => Kelvin = default;

    /// <summary>
    /// Inicializa uma nova Temperature.
    /// </summary>
    /// <param name="kelvin">Temperatura em kelvin. Não pode ser negativo.</param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="kelvin"/> for negativo.
    /// </exception>
    public Temperature(decimal kelvin)
    {
        if (kelvin < 0m)
            throw new ArgumentException("Kelvin must be non-negative.", nameof(kelvin));

        Kelvin = kelvin;
    }

    /// <summary>
    /// Cria uma Temperature a partir de Celsius.
    /// </summary>
    /// <param name="c">O valor em Celsius.</param>
    /// <returns>Uma nova Temperature.</returns>
    public static Temperature FromCelsius(decimal c)
        => new Temperature(c + 273.15m);

    /// <summary>
    /// Cria uma Temperature a partir de Fahrenheit.
    /// </summary>
    /// <param name="f">O valor em Fahrenheit.</param>
    /// <returns>Uma nova Temperature.</returns>
    public static Temperature FromFahrenheit(decimal f)
        => new Temperature((f - 32m) * 5m / 9m + 273.15m);

    /// <summary>
    /// Converte esta temperatura para Celsius.
    /// </summary>
    /// <returns>O valor em Celsius.</returns>
    public decimal ToCelsius()
        => Kelvin - 273.15m;

    /// <summary>
    /// Converte esta temperatura para Fahrenheit.
    /// </summary>
    /// <returns>O valor em Fahrenheit.</returns>
    public decimal ToFahrenheit()
        => (Kelvin - 273.15m) * 9m / 5m + 32m;

    /// <summary>
    /// Compara esta instância com outra temperatura.
    /// </summary>
    /// <param name="other">A temperatura a ser comparada.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa das temperaturas sendo comparadas.
    /// Retorna um número negativo se esta temperatura for menor que <paramref name="other"/>,
    /// zero se forem iguais, ou um número positivo se esta temperatura for maior.
    /// </returns>
    public int CompareTo(Temperature? other)
    {
        if (other is null) return 1;
        return Kelvin.CompareTo(other.Kelvin);
    }

    /// <summary>
    /// Operador menor que (&lt;).
    /// </summary>
    /// <param name="left">A primeira temperatura.</param>
    /// <param name="right">A segunda temperatura.</param>
    /// <returns><c>true</c> se <paramref name="left"/> for menor que <paramref name="right"/>; caso contrário, <c>false</c>.</returns>
    public static bool operator <(Temperature? left, Temperature? right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Operador menor ou igual que (&lt;=).
    /// </summary>
    /// <param name="left">A primeira temperatura.</param>
    /// <param name="right">A segunda temperatura.</param>
    /// <returns><c>true</c> se <paramref name="left"/> for menor ou igual a <paramref name="right"/>; caso contrário, <c>false</c>.</returns>
    public static bool operator <=(Temperature? left, Temperature? right)
        => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Operador maior que (&gt;).
    /// </summary>
    /// <param name="left">A primeira temperatura.</param>
    /// <param name="right">A segunda temperatura.</param>
    /// <returns><c>true</c> se <paramref name="left"/> for maior que <paramref name="right"/>; caso contrário, <c>false</c>.</returns>
    public static bool operator >(Temperature? left, Temperature? right)
        => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Operador maior ou igual que (&gt;=).
    /// </summary>
    /// <param name="left">A primeira temperatura.</param>
    /// <param name="right">A segunda temperatura.</param>
    /// <returns><c>true</c> se <paramref name="left"/> for maior ou igual a <paramref name="right"/>; caso contrário, <c>false</c>.</returns>
    public static bool operator >=(Temperature? left, Temperature? right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    /// <summary>
    /// Retorna uma string formatada, ex: "25.00 °C".
    /// </summary>
    public override string ToString()
        => $"{ToCelsius():N2} °C";
}
