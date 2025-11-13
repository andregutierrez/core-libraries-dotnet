namespace Core.Libraries.Domain.ValueObjects.Identities;

using System;
using System.Text.RegularExpressions;

/// <summary>
/// Representa um CNPJ brasileiro (14 dígitos), validando os dígitos verificadores.
/// </summary>
public record Cnpj
{
    private static readonly Regex _digitsOnly = new(@"\D", RegexOptions.Compiled);

    /// <summary>
    /// O CNPJ de 14 dígitos, sem pontuação.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Cnpj() => Value = default!;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="Cnpj"/>, validando comprimento e checksum.
    /// </summary>
    /// <param name="cnpj">A string CNPJ bruta (com ou sem pontuação).</param>
    /// <exception cref="ArgumentException">Se for inválido.</exception>
    public Cnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ must not be empty.", nameof(cnpj));

        var digits = _digitsOnly.Replace(cnpj, "");
        if (digits.Length != 14 || new string(digits[0], 14) == digits)
            throw new ArgumentException("CNPJ must be 14 digits and not all identical.", nameof(cnpj));

        if (!ValidateCheckDigits(digits))
            throw new ArgumentException("CNPJ check digits are invalid.", nameof(cnpj));

        Value = digits;
    }

    private static bool ValidateCheckDigits(string d)
    {
        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int Calc(string s, int[] w)
        {
            int sum = 0;
            for (int i = 0; i < w.Length; i++)
                sum += (s[i] - '0') * w[i];
            int rem = sum % 11;
            return rem < 2 ? 0 : 11 - rem;
        }

        if (Calc(d, weights1) != d[12] - '0') return false;
        if (Calc(d, weights2) != d[13] - '0') return false;
        return true;
    }

    /// <summary>
    /// Retorna o CNPJ formatado: "12.345.678/0001-90".
    /// </summary>
    public override string ToString()
        => $"{Value.Substring(0, 2)}.{Value.Substring(2, 3)}.{Value.Substring(5, 3)}/{Value.Substring(8, 4)}-{Value.Substring(12, 2)}";
}
