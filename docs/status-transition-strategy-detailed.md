# Sistema de Transições de Status - Strategy Pattern (Detalhado)

## Decisão: Strategy Pattern com Registry

**Justificativa:**
- ✅ Performance essencial → Validação em memória, sem I/O
- ✅ Recompilação aceitável → Type-safety e validação em compile-time
- ✅ Código claro e manutenível
- ✅ Fácil de testar

---

## Arquitetura Proposta

### 1. Interface Base do Validador

```csharp
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
/// Exemplo:
/// <code>
/// public class OrderStatusTransitionValidator : IStatusTransitionValidator&lt;Order, OrderStatus&gt;
/// {
///     public bool CanTransition(OrderStatus fromStatus, OrderStatus toStatus, Order order)
///     {
///         // Regras específicas para Pedido
///         if (fromStatus == OrderStatus.Paid && toStatus == OrderStatus.Cancelled)
///             return false; // Pedido pago não pode ser cancelado
///             
///         return true;
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
    /// Este método deve ser idempotente e não deve lançar exceções.
    /// Use <see cref="ValidateTransition"/> para lançar exceções quando necessário.
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
    /// Este método deve chamar <see cref="CanTransition"/> e lançar exceção se retornar <c>false</c>.
    /// </remarks>
    void ValidateTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
}
```

### 2. Registry para Gerenciar Validadores

```csharp
namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Registry centralizado para gerenciar validadores de transição de status.
/// </summary>
/// <remarks>
/// <para>
/// Este registry permite registrar e recuperar validadores de transição de status
/// para diferentes tipos de entidades. Facilita a injeção de dependência e
/// permite que o sistema encontre automaticamente o validador correto.
/// </para>
/// <para>
/// Uso típico:
/// <code>
/// var registry = new StatusTransitionValidatorRegistry();
/// registry.Register&lt;Order, OrderStatus&gt;(new OrderStatusTransitionValidator());
/// 
/// var validator = registry.GetValidator&lt;Order, OrderStatus&gt;();
/// validator.ValidateTransition(currentStatus, newStatus, order);
/// </code>
/// </para>
/// </remarks>
public class StatusTransitionValidatorRegistry
{
    private readonly Dictionary<Type, object> _validators = new();

    /// <summary>
    /// Registra um validador de transição para um tipo específico de entidade e status.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <typeparam name="TStatus">O tipo enum do status.</typeparam>
    /// <param name="validator">O validador a ser registrado.</param>
    /// <exception cref="ArgumentNullException">Lançada quando validator é null.</exception>
    /// <remarks>
    /// Se um validador já estiver registrado para o mesmo tipo, ele será substituído.
    /// </remarks>
    public void Register<TEntity, TStatus>(IStatusTransitionValidator<TEntity, TStatus> validator)
        where TStatus : Enum
    {
        ArgumentNullException.ThrowIfNull(validator);
        
        var key = typeof(TEntity);
        _validators[key] = validator;
    }

    /// <summary>
    /// Obtém o validador registrado para um tipo específico de entidade e status.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <typeparam name="TStatus">O tipo enum do status.</typeparam>
    /// <returns>
    /// O validador registrado, ou <c>null</c> se nenhum validador estiver registrado.
    /// </returns>
    public IStatusTransitionValidator<TEntity, TStatus>? GetValidator<TEntity, TStatus>()
        where TStatus : Enum
    {
        var key = typeof(TEntity);
        if (_validators.TryGetValue(key, out var validator))
        {
            return (IStatusTransitionValidator<TEntity, TStatus>)validator;
        }
        
        return null;
    }

    /// <summary>
    /// Verifica se existe um validador registrado para um tipo específico.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <typeparam name="TStatus">O tipo enum do status.</typeparam>
    /// <returns>
    /// <c>true</c> se um validador estiver registrado; caso contrário, <c>false</c>.
    /// </returns>
    public bool HasValidator<TEntity, TStatus>()
        where TStatus : Enum
    {
        return _validators.ContainsKey(typeof(TEntity));
    }
}
```

### 3. Classe Helper para Regras Simples

```csharp
namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Classe helper para criar validadores baseados em regras simples de transição.
/// </summary>
/// <typeparam name="TStatus">O tipo enum do status.</typeparam>
/// <remarks>
/// <para>
/// Esta classe facilita a criação de validadores para casos onde as regras são
/// simplesmente "de status X pode ir para status Y". Para regras mais complexas,
/// implemente <see cref="IStatusTransitionValidator{TEntity, TStatus}"/> diretamente.
/// </para>
/// <para>
/// Exemplo:
/// <code>
/// var rules = new StatusTransitionRules&lt;OrderStatus&gt;()
///     .Allow(OrderStatus.Pending, OrderStatus.Paid, OrderStatus.Cancelled)
///     .Allow(OrderStatus.Paid, OrderStatus.Shipped, OrderStatus.Refunded)
///     .Allow(OrderStatus.Shipped, OrderStatus.Delivered);
///     
/// var validator = new RuleBasedStatusTransitionValidator&lt;Order, OrderStatus&gt;(rules);
/// </code>
/// </para>
/// </remarks>
public class StatusTransitionRules<TStatus>
    where TStatus : Enum
{
    private readonly Dictionary<TStatus, HashSet<TStatus>> _allowedTransitions = new();

    /// <summary>
    /// Define que de um status específico é permitido transicionar para os status fornecidos.
    /// </summary>
    /// <param name="fromStatus">O status de origem.</param>
    /// <param name="toStatuses">Os status de destino permitidos.</param>
    /// <returns>Esta instância para permitir method chaining.</returns>
    public StatusTransitionRules<TStatus> Allow(TStatus fromStatus, params TStatus[] toStatuses)
    {
        if (!_allowedTransitions.ContainsKey(fromStatus))
            _allowedTransitions[fromStatus] = new HashSet<TStatus>();

        foreach (var toStatus in toStatuses)
            _allowedTransitions[fromStatus].Add(toStatus);

        return this;
    }

    /// <summary>
    /// Verifica se uma transição específica é permitida.
    /// </summary>
    /// <param name="fromStatus">O status de origem.</param>
    /// <param name="toStatus">O status de destino.</param>
    /// <returns>
    /// <c>true</c> se a transição for permitida; caso contrário, <c>false</c>.
    /// </returns>
    public bool IsTransitionAllowed(TStatus fromStatus, TStatus toStatus)
    {
        return _allowedTransitions.TryGetValue(fromStatus, out var allowed) 
            && allowed.Contains(toStatus);
    }

    /// <summary>
    /// Obtém todos os status permitidos a partir de um status específico.
    /// </summary>
    /// <param name="fromStatus">O status de origem.</param>
    /// <returns>
    /// Uma coleção de status permitidos, ou uma coleção vazia se nenhum for permitido.
    /// </returns>
    public IReadOnlySet<TStatus> GetAllowedTransitions(TStatus fromStatus)
    {
        return _allowedTransitions.TryGetValue(fromStatus, out var allowed)
            ? allowed
            : new HashSet<TStatus>();
    }
}

/// <summary>
/// Validador baseado em regras simples de transição.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
/// <typeparam name="TStatus">O tipo enum do status.</typeparam>
public class RuleBasedStatusTransitionValidator<TEntity, TStatus> 
    : IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    private readonly StatusTransitionRules<TStatus> _rules;

    /// <summary>
    /// Inicializa uma nova instância do validador baseado em regras.
    /// </summary>
    /// <param name="rules">As regras de transição a serem aplicadas.</param>
    public RuleBasedStatusTransitionValidator(StatusTransitionRules<TStatus> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        _rules = rules;
    }

    /// <summary>
    /// Verifica se uma transição de status é permitida pelas regras.
    /// </summary>
    public bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity)
    {
        return _rules.IsTransitionAllowed(fromStatus, toStatus);
    }

    /// <summary>
    /// Valida uma transição de status e lança exceção se inválida.
    /// </summary>
    public void ValidateTransition(TStatus fromStatus, TStatus toStatus, TEntity entity)
    {
        if (!CanTransition(fromStatus, toStatus, entity))
        {
            throw new InvalidStatusTransitionException(
                domainContext: typeof(TEntity).Name,
                fromStatus: fromStatus.ToString(),
                toStatus: toStatus.ToString()
            );
        }
    }
}
```

### 4. Integração com StatusHistory

```csharp
// Modificação proposta para StatusHistory
public abstract class StatusHistory<TStatus> : EntityList<TStatus>, IStatusHistory<TStatus> 
    where TStatus : IStatusBase
{
    private readonly IStatusTransitionValidator<object, TStatus>? _validator;
    private readonly Func<TStatus, Enum> _getStatusType;

    /// <summary>
    /// Construtor padrão sem validador (comportamento atual).
    /// </summary>
    protected StatusHistory()
    {
        _validator = null;
        _getStatusType = null!;
    }

    /// <summary>
    /// Construtor com validador de transição.
    /// </summary>
    /// <param name="validator">O validador de transição a ser usado.</param>
    /// <param name="getStatusType">Função para extrair o tipo enum do status.</param>
    protected StatusHistory(
        IStatusTransitionValidator<object, TStatus>? validator,
        Func<TStatus, Enum> getStatusType)
    {
        _validator = validator;
        _getStatusType = getStatusType;
    }

    public void Add(TStatus entity)
    {
        // Validar transição se validador estiver configurado
        if (_validator != null && _getStatusType != null)
        {
            var currentStatus = GetCurrentOrDefault();
            if (currentStatus != null)
            {
                var fromStatus = (Enum)_getStatusType(currentStatus);
                var toStatus = (Enum)_getStatusType(entity);
                
                // Obter a entidade pai (que contém este StatusHistory)
                var parentEntity = GetParentEntity();
                if (parentEntity != null)
                {
                    _validator.ValidateTransition(fromStatus, toStatus, parentEntity);
                }
            }
        }

        // Verificar se status está marcado como current
        if (entity.IsCurrent == false)
            throw new CannotSetInactiveStatusAsCurrentException();

        // Desativar status atuais
        foreach (var status in this.Where(s => s.IsCurrent))
            status.Deactivate();

        _list.Add(entity);
    }

    private TStatus? GetCurrentOrDefault()
    {
        return this.FirstOrDefault(s => s.IsCurrent);
    }

    // Método abstrato para obter a entidade pai
    // Será implementado nas classes derivadas
    protected abstract object? GetParentEntity();
}
```

---

## Exemplos de Implementação

### Exemplo 1: Pedido com Regras Simples

```csharp
// Enum de status
public enum OrderStatus
{
    Pending,
    Paid,
    Shipped,
    Delivered,
    Cancelled,
    Refunded
}

// Definir regras
var orderRules = new StatusTransitionRules<OrderStatus>()
    .Allow(OrderStatus.Pending, OrderStatus.Paid, OrderStatus.Cancelled)
    .Allow(OrderStatus.Paid, OrderStatus.Shipped, OrderStatus.Refunded)
    .Allow(OrderStatus.Shipped, OrderStatus.Delivered)
    // Estados finais não permitem transições
    .Allow(OrderStatus.Delivered) // Vazio = nenhuma transição permitida
    .Allow(OrderStatus.Cancelled)
    .Allow(OrderStatus.Refunded);

// Criar validador
var orderValidator = new RuleBasedStatusTransitionValidator<Order, OrderStatus>(orderRules);

// Registrar
var registry = new StatusTransitionValidatorRegistry();
registry.Register<Order, OrderStatus>(orderValidator);
```

### Exemplo 2: Pedido com Regras Complexas

```csharp
public class OrderStatusTransitionValidator : IStatusTransitionValidator<Order, OrderStatus>
{
    private static readonly Dictionary<OrderStatus, HashSet<OrderStatus>> AllowedTransitions = new()
    {
        [OrderStatus.Pending] = new() { OrderStatus.Paid, OrderStatus.Cancelled },
        [OrderStatus.Paid] = new() { OrderStatus.Shipped, OrderStatus.Refunded },
        [OrderStatus.Shipped] = new() { OrderStatus.Delivered },
        [OrderStatus.Delivered] = new(), // Estado final
        [OrderStatus.Cancelled] = new(), // Estado final
        [OrderStatus.Refunded] = new() // Estado final
    };

    public bool CanTransition(OrderStatus fromStatus, OrderStatus toStatus, Order order)
    {
        // Regra básica: verificar se transição está na lista permitida
        if (!AllowedTransitions.TryGetValue(fromStatus, out var allowed) 
            || !allowed.Contains(toStatus))
        {
            return false;
        }

        // Regras condicionais baseadas na entidade
        if (fromStatus == OrderStatus.Pending && toStatus == OrderStatus.Paid)
        {
            // Só pode pagar se tiver itens
            if (!order.Items.Any())
                return false;
        }

        if (fromStatus == OrderStatus.Paid && toStatus == OrderStatus.Refunded)
        {
            // Só pode reembolsar se não foi enviado
            if (order.ShippedDate.HasValue)
                return false;
        }

        return true;
    }

    public void ValidateTransition(OrderStatus fromStatus, OrderStatus toStatus, Order order)
    {
        if (!CanTransition(fromStatus, toStatus, order))
        {
            throw new InvalidStatusTransitionException(
                domainContext: "Order",
                fromStatus: fromStatus.ToString(),
                toStatus: toStatus.ToString(),
                entityId: order.Id.ToString(),
                details: new 
                { 
                    OrderTotal = order.TotalAmount,
                    HasItems = order.Items.Any(),
                    ShippedDate = order.ShippedDate
                }
            );
        }
    }
}
```

### Exemplo 3: Cliente com Regras Diferentes

```csharp
public enum CustomerStatus
{
    Active,
    Inactive,
    Suspended,
    Banned
}

public class CustomerStatusTransitionValidator : IStatusTransitionValidator<Customer, CustomerStatus>
{
    public bool CanTransition(CustomerStatus fromStatus, CustomerStatus toStatus, Customer customer)
    {
        // Cliente pode ser suspenso de qualquer estado (exceto banido)
        if (toStatus == CustomerStatus.Suspended && fromStatus != CustomerStatus.Banned)
            return true;

        // Cliente banido não pode mudar de status
        if (fromStatus == CustomerStatus.Banned)
            return false;

        // Transições permitidas
        return (fromStatus, toStatus) switch
        {
            (CustomerStatus.Active, CustomerStatus.Inactive) => true,
            (CustomerStatus.Active, CustomerStatus.Suspended) => true,
            (CustomerStatus.Inactive, CustomerStatus.Active) => true,
            (CustomerStatus.Suspended, CustomerStatus.Active) => true,
            (CustomerStatus.Suspended, CustomerStatus.Banned) => true,
            (CustomerStatus.Active, CustomerStatus.Banned) => true,
            _ => false
        };
    }

    public void ValidateTransition(CustomerStatus fromStatus, CustomerStatus toStatus, Customer customer)
    {
        if (!CanTransition(fromStatus, toStatus, customer))
        {
            throw new InvalidStatusTransitionException(
                domainContext: "Customer",
                fromStatus: fromStatus.ToString(),
                toStatus: toStatus.ToString(),
                entityId: customer.Id.ToString()
            );
        }
    }
}
```

---

## Vantagens desta Abordagem

1. **Performance Máxima**
   - Validação em memória (Dictionary lookup O(1))
   - Sem I/O ou parsing
   - Type-safe (sem reflection)

2. **Type Safety**
   - Compile-time checking
   - IntelliSense completo
   - Refactoring seguro

3. **Testabilidade**
   - Cada validador é isolado
   - Fácil mockar em testes
   - Testes unitários simples

4. **Manutenibilidade**
   - Código claro e explícito
   - Fácil entender regras
   - Fácil adicionar novas regras

5. **Extensibilidade**
   - Fácil adicionar novos tipos de entidades
   - Pode combinar regras simples com complexas
   - Permite evolução gradual

---

## Pontos de Atenção

1. **Obter Entidade Pai**: O `StatusHistory` precisa de uma forma de obter a entidade que o contém para passar ao validador. Isso pode ser feito via:
   - Injeção no construtor
   - Método abstrato para obter a entidade
   - Passar a entidade como parâmetro no método `Add`

2. **Extrair Tipo do Status**: Precisa de uma forma de extrair o enum do `TStatus` que implementa `IStatusBase`. Isso pode ser feito via:
   - Método abstrato na classe base
   - Interface adicional
   - Reflection (mas perde performance)

3. **Validação Opcional**: O sistema deve funcionar sem validador (comportamento atual) e com validador (novo comportamento).

---

## Próximos Passos

Antes de implementar, precisamos decidir:

1. **Como obter a entidade pai no StatusHistory?**
   - Opção A: Passar como parâmetro no `Add(TStatus status, TEntity entity)`
   - Opção B: Injetar no construtor do StatusHistory
   - Opção C: Método abstrato `GetParentEntity()`

2. **Como extrair o tipo enum do status?**
   - Opção A: Método abstrato `GetStatusType(TStatus status) -> Enum`
   - Opção B: Interface adicional `IStatus<TEnum>`
   - Opção C: Usar reflection (não recomendado para performance)

3. **Validação deve ser obrigatória ou opcional?**
   - Opção A: Opcional (backward compatible)
   - Opção B: Obrigatória (mais seguro, mas breaking change)

Qual abordagem você prefere para esses pontos?

