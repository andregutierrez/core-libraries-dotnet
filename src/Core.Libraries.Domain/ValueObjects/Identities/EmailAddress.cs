namespace Core.Libraries.Domain.ValueObjects.Identities;

using System;
using System.Net.Mail;

/// <summary>
/// Representa um endereço de email normalizado (minúsculas).
/// </summary>
public record EmailAddress
{
    /// <summary>
    /// Endereço de email normalizado (minúsculas).
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Obtém a parte antes do '@' (parte local) do email.
    /// </summary>
    public string LocalPart
        => Value[..Value.IndexOf('@')];

    /// <summary>
    /// Obtém a parte depois do '@' (domínio) do email.
    /// </summary>
    public string Domain
        => Value[(Value.IndexOf('@') + 1)..];

    /// <summary>
    /// Construtor protegido sem parâmetros para EF Core e desserialização.
    /// </summary>
    protected EmailAddress() => Value = default!;

    /// <summary>
    /// Cria um novo EmailAddress, validando o formato via System.Net.Mail.MailAddress.
    /// </summary>
    /// <param name="email">String de email bruta.</param>
    /// <exception cref="ArgumentException">Se for null/vazio ou formato inválido.</exception>
    public EmailAddress(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email must not be empty.", nameof(email));

        try
        {
            var addr = new MailAddress(email);
            Value = addr.Address.ToLowerInvariant();
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }
    }

    /// <summary>
    /// Retorna o endereço de email como string.
    /// </summary>
    public override string ToString() => Value;
}
