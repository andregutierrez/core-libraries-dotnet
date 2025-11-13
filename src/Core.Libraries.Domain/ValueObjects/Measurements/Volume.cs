namespace Core.Libraries.Domain.ValueObjects.Measurements;

using System;

/// <summary>
/// Representa uma medida de volume, armazenada internamente em litros.
/// </summary>
public record Volume : IComparable<Volume>
{
    /// <summary>Volume em litros (≥ 0).</summary>
    public decimal Liters { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Volume() => Liters = default;

    /// <summary>
    /// Inicializa um novo Volume.
    /// </summary>
    /// <param name="liters">Volume em litros. Não pode ser negativo.</param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="liters"/> for negativo.
    /// </exception>
    public Volume(decimal liters)
    {
        if (liters < 0m)
            throw new ArgumentException("Volume must be non-negative.", nameof(liters));

        Liters = liters;
    }

    /// <summary>
    /// Cria um Volume a partir de mililitros.
    /// </summary>
    /// <param name="ml">O valor em mililitros.</param>
    /// <returns>Um novo Volume.</returns>
    public static Volume FromMilliliters(decimal ml)
        => new Volume(ml / 1_000m);

    /// <summary>
    /// Cria um Volume a partir de metros cúbicos.
    /// </summary>
    /// <param name="m3">O valor em metros cúbicos.</param>
    /// <returns>Um novo Volume.</returns>
    public static Volume FromCubicMeters(decimal m3)
        => new Volume(m3 * 1_000m);

    /// <summary>
    /// Converte este volume para mililitros.
    /// </summary>
    /// <returns>O valor em mililitros.</returns>
    public decimal ToMilliliters()
        => Liters * 1_000m;

    /// <summary>
    /// Converte este volume para metros cúbicos.
    /// </summary>
    /// <returns>O valor em metros cúbicos.</returns>
    public decimal ToCubicMeters()
        => Liters / 1_000m;

    /// <summary>
    /// Adiciona dois volumes.
    /// </summary>
    /// <param name="other">O outro volume a ser adicionado.</param>
    /// <returns>Um novo Volume com a soma dos volumes.</returns>
    public Volume Add(Volume other)
        => new Volume(Liters + other.Liters);

    /// <summary>
    /// Subtrai outro volume.
    /// </summary>
    /// <param name="other">O outro volume a ser subtraído.</param>
    /// <returns>Um novo Volume com a diferença dos volumes.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se o resultado for negativo.
    /// </exception>
    public Volume Subtract(Volume other)
    {
        var result = Liters - other.Liters;
        if (result < 0m)
            throw new InvalidOperationException("Resulting volume cannot be negative.");
        return new Volume(result);
    }

    /// <summary>
    /// Multiplica este volume por um fator.
    /// </summary>
    /// <param name="factor">O fator de multiplicação.</param>
    /// <returns>Um novo Volume com o volume multiplicado.</returns>
    public Volume Multiply(decimal factor)
        => new Volume(Liters * factor);

    /// <summary>
    /// Compara esta instância com outro volume.
    /// </summary>
    /// <param name="other">O volume a ser comparado.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa dos volumes sendo comparados.
    /// Retorna um número negativo se este volume for menor que <paramref name="other"/>,
    /// zero se forem iguais, ou um número positivo se este volume for maior.
    /// </returns>
    public int CompareTo(Volume? other)
    {
        if (other is null) return 1;
        return Liters.CompareTo(other.Liters);
    }

    /// <summary>
    /// Operador menor que (&lt;).
    /// </summary>
    public static bool operator <(Volume? left, Volume? right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Operador menor ou igual que (&lt;=).
    /// </summary>
    public static bool operator <=(Volume? left, Volume? right)
        => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Operador maior que (&gt;).
    /// </summary>
    public static bool operator >(Volume? left, Volume? right)
        => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Operador maior ou igual que (&gt;=).
    /// </summary>
    public static bool operator >=(Volume? left, Volume? right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    /// <summary>
    /// Retorna uma string formatada, ex: "2.50 L".
    /// </summary>
    public override string ToString()
        => $"{Liters:N2} L";
}
