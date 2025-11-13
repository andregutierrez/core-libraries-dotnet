namespace Core.Libraries.Domain.Entities.Lifecycle;


public record UserInfo
{
    /// <summary>
    /// Obtém o identificador do usuário que realizou a ação (opcional).
    /// </summary>
    /// <value>
    /// O identificador do usuário, ou <c>null</c> se o rastreamento de usuário
    /// não estiver habilitado ou não disponível.
    /// </value>
    public int? UserId { get; protected set; }
}

