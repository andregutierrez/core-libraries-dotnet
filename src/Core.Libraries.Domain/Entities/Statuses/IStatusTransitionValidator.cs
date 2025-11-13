using Core.Libraries.Domain.Exceptions;

namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Define o contrato para validação de transições de status entre estados de uma entidade.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade que possui o histórico de status.</typeparam>
/// <typeparam name="TStatus">O tipo enum que representa os possíveis status.</typeparam>
/// <remarks>
/// <para>
/// Implementações desta interface devem definir as regras de negócio específicas para
/// transições de status de um tipo de entidade. Cada tipo de entidade (ex: Pedido, Cliente)
/// pode ter suas próprias regras de transição.
/// </para>
/// <para>
/// Exemplo de implementação:
/// <code>
/// public class CustomerStatusTransitionValidator : IStatusTransitionValidator&lt;Customer, CustomerStatus&gt;
/// {
///     public bool CanTransition(CustomerStatus fromStatus, CustomerStatus toStatus, Customer customer)
///     {
///         // Cliente banido não pode mudar de status
///         if (fromStatus == CustomerStatus.Banned)
///             return false;
///             
///         // Transições permitidas usando pattern matching
///         return (fromStatus, toStatus) switch
///         {
///             (CustomerStatus.Active, CustomerStatus.Inactive) => true,
///             (CustomerStatus.Active, CustomerStatus.Suspended) => true,
///             (CustomerStatus.Inactive, CustomerStatus.Active) => true,
///             _ => false
///         };
///     }
///     
///     public void ValidateTransition(CustomerStatus fromStatus, CustomerStatus toStatus, Customer customer)
///     {
///         if (!CanTransition(fromStatus, toStatus, customer))
///         {
///             throw new InvalidStatusTransitionException(
///                 domainContext: "Customer",
///                 fromStatus: fromStatus.ToString(),
///                 toStatus: toStatus.ToString(),
///                 entityId: customer.Id.ToString()
///             );
///         }
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public interface IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    /// <summary>
    /// Verifica se uma transição de status é permitida.
    /// </summary>
    /// <param name="fromStatus">O status atual da entidade.</param>
    /// <param name="toStatus">O status desejado para transição.</param>
    /// <param name="entity">A entidade que está realizando a transição.</param>
    /// <returns>
    /// <c>true</c> se a transição for permitida; caso contrário, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Este método deve ser idempotente e não deve lançar exceções.
    /// Use <see cref="ValidateTransition"/> para lançar exceções quando necessário.
    /// </para>
    /// <para>
    /// Este método pode ser usado para verificar transições antes de tentá-las,
    /// permitindo feedback ao usuário sem lançar exceções.
    /// </para>
    /// </remarks>
    bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);

    /// <summary>
    /// Valida uma transição de status e lança exceção se inválida.
    /// </summary>
    /// <param name="fromStatus">O status atual da entidade.</param>
    /// <param name="toStatus">O status desejado para transição.</param>
    /// <param name="entity">A entidade que está realizando a transição.</param>
    /// <exception cref="InvalidStatusTransitionException">
    /// Lançada quando a transição não é permitida pelas regras de negócio.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Este método deve chamar <see cref="CanTransition"/> e lançar exceção se retornar <c>false</c>.
    /// </para>
    /// <para>
    /// Use este método quando quiser garantir que uma transição inválida resulte em exceção,
    /// como no método <see cref="StatusHistory{TStatus}.Add"/>.
    /// </para>
    /// </remarks>
    void ValidateTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
}

