namespace Core.Libraries.Domain.ValueObjects.Measurements;

using System;

/// <summary>
/// Representa uma medida de massa, armazenada internamente em quilogramas.
/// </summary>
public record Weight : IComparable<Weight>
{
    /// <summary>Massa em quilogramas (≥ 0).</summary>
    public decimal Kilograms { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Weight() => Kilograms = default;

    /// <summary>
    /// Inicializa um novo Weight.
    /// </summary>
    /// <param name="kilograms">Peso em quilogramas. Não pode ser negativo.</param>
    /// <exception cref="ArgumentException">
    /// Lançada se <paramref name="kilograms"/> for negativo.
    /// </exception>
    public Weight(decimal kilograms)
    {
        if (kilograms < 0m)
            throw new ArgumentException("Weight must be non-negative.", nameof(kilograms));

        Kilograms = kilograms;
    }

    /// <summary>
    /// Cria um Weight a partir de gramas.
    /// </summary>
    /// <param name="g">O valor em gramas.</param>
    /// <returns>Um novo Weight.</returns>
    public static Weight FromGrams(decimal g)
        => new Weight(g / 1_000m);

    /// <summary>
    /// Cria um Weight a partir de libras.
    /// </summary>
    /// <param name="lb">O valor em libras.</param>
    /// <returns>Um novo Weight.</returns>
    public static Weight FromPounds(decimal lb)
        => new Weight(lb * 0.45359237m);

    /// <summary>
    /// Converte este peso para gramas.
    /// </summary>
    /// <returns>O valor em gramas.</returns>
    public decimal ToGrams()
        => Kilograms * 1_000m;

    /// <summary>
    /// Converte este peso para libras.
    /// </summary>
    /// <returns>O valor em libras.</returns>
    public decimal ToPounds()
        => Kilograms / 0.45359237m;

    /// <summary>
    /// Adiciona dois pesos.
    /// </summary>
    /// <param name="other">O outro peso a ser adicionado.</param>
    /// <returns>Um novo Weight com a soma dos pesos.</returns>
    public Weight Add(Weight other)
        => new Weight(Kilograms + other.Kilograms);

    /// <summary>
    /// Subtrai outro peso.
    /// </summary>
    /// <param name="other">O outro peso a ser subtraído.</param>
    /// <returns>Um novo Weight com a diferença dos pesos.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se o resultado for negativo.
    /// </exception>
    public Weight Subtract(Weight other)
    {
        var result = Kilograms - other.Kilograms;
        if (result < 0m)
            throw new InvalidOperationException("Resulting weight cannot be negative.");
        return new Weight(result);
    }

    /// <summary>
    /// Multiplica este peso por um fator.
    /// </summary>
    /// <param name="factor">O fator de multiplicação.</param>
    /// <returns>Um novo Weight com o peso multiplicado.</returns>
    public Weight Multiply(decimal factor)
        => new Weight(Kilograms * factor);

    /// <summary>
    /// Compara esta instância com outro peso.
    /// </summary>
    /// <param name="other">O peso a ser comparado.</param>
    /// <returns>
    /// Um valor que indica a ordem relativa dos pesos sendo comparados.
    /// Retorna um número negativo se este peso for menor que <paramref name="other"/>,
    /// zero se forem iguais, ou um número positivo se este peso for maior.
    /// </returns>
    public int CompareTo(Weight? other)
    {
        if (other is null) return 1;
        return Kilograms.CompareTo(other.Kilograms);
    }

    /// <summary>
    /// Operador menor que (&lt;).
    /// </summary>
    public static bool operator <(Weight? left, Weight? right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Operador menor ou igual que (&lt;=).
    /// </summary>
    public static bool operator <=(Weight? left, Weight? right)
        => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Operador maior que (&gt;).
    /// </summary>
    public static bool operator >(Weight? left, Weight? right)
        => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Operador maior ou igual que (&gt;=).
    /// </summary>
    public static bool operator >=(Weight? left, Weight? right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    /// <summary>
    /// Retorna uma string formatada, ex: "5.00 kg".
    /// </summary>
    public override string ToString()
        => $"{Kilograms:N2} kg";
}
