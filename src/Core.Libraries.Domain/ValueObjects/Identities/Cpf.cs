namespace Core.Libraries.Domain.ValueObjects.Identities;

using System;
using System.Text.RegularExpressions;

/// <summary>
/// Representa um CPF brasileiro (11 dígitos), validando os dígitos verificadores.
/// </summary>
public record Cpf
{
    private static readonly Regex _digitsOnly = new(@"\D", RegexOptions.Compiled);

    /// <summary>
    /// O CPF de 11 dígitos, sem pontuação.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected Cpf() => Value = default!;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="Cpf"/>, validando comprimento e checksum.
    /// </summary>
    /// <param name="cpf">A string CPF bruta (com ou sem pontos/hífens).</param>
    /// <exception cref="ArgumentException">Se for inválido.</exception>
    public Cpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF must not be empty.", nameof(cpf));

        var digits = _digitsOnly.Replace(cpf, "");
        if (digits.Length != 11 || new string(digits[0], 11) == digits)
            throw new ArgumentException("CPF must be 11 digits and not all identical.", nameof(cpf));

        // validação dos dígitos verificadores
        if (!ValidateCheckDigits(digits))
            throw new ArgumentException("CPF check digits are invalid.", nameof(cpf));

        Value = digits;
    }

    private static bool ValidateCheckDigits(string d)
    {
        int Sum(int count, int factorStart)
        {
            var sum = 0;
            for (int i = 0; i < count; i++)
                sum += (d[i] - '0') * (factorStart - i);
            return sum;
        }

        // primeiro dígito
        int rem = Sum(9, 10) % 11;
        int check1 = rem < 2 ? 0 : 11 - rem;
        if (check1 != d[9] - '0') return false;

        // segundo dígito
        rem = Sum(10, 11) % 11;
        int check2 = rem < 2 ? 0 : 11 - rem;
        return check2 == d[10] - '0';
    }

    /// <summary>
    /// Retorna o CPF formatado: "123.456.789-00".
    /// </summary>
    public override string ToString()
        => $"{Value.Substring(0, 3)}.{Value.Substring(3, 3)}.{Value.Substring(6, 3)}-{Value.Substring(9, 2)}";
}
