# Exemplo de Uso - Sistema de Transições de Status

## Classes Criadas

1. ✅ `IStatusTransitionValidator<TEntity, TStatus>` - Interface base
2. ✅ `StatusTransitionValidatorRegistry` - Registry centralizado
3. ✅ `StatusHistory<TStatus>` - Modificado para suportar validação

## Exemplo Completo: Cliente com Validação de Transições

### 1. Definir o Enum de Status

```csharp
public enum CustomerStatus
{
    Active,
    Inactive,
    Suspended,
    Banned
}
```

### 2. Criar a Entidade de Status

```csharp
public class CustomerStatusEntity : Status<CustomerStatus>
{
    protected CustomerStatusEntity() { }

    public CustomerStatusEntity(
        Guid key, 
        DateTime createdAt, 
        CustomerStatus type, 
        string notes)
        : base(key, createdAt, type, notes)
    {
    }
}
```

### 3. Implementar o Validador (Exemplo 3 - Pattern Matching)

```csharp
using Core.Libraries.Domain.Entities.Statuses;
using Core.Libraries.Domain.Exceptions;

public class CustomerStatusTransitionValidator 
    : IStatusTransitionValidator<Customer, CustomerStatus>
{
    public bool CanTransition(
        CustomerStatus fromStatus, 
        CustomerStatus toStatus, 
        Customer customer)
    {
        // Cliente pode ser suspenso de qualquer estado (exceto banido)
        if (toStatus == CustomerStatus.Suspended && fromStatus != CustomerStatus.Banned)
            return true;

        // Cliente banido não pode mudar de status
        if (fromStatus == CustomerStatus.Banned)
            return false;

        // Transições permitidas usando pattern matching
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

    public void ValidateTransition(
        CustomerStatus fromStatus, 
        CustomerStatus toStatus, 
        Customer customer)
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

### 4. Criar o StatusHistory para Cliente

```csharp
public class CustomerStatusHistory : StatusHistory<CustomerStatusEntity>
{
    private readonly Customer _customer;

    public CustomerStatusHistory(Customer customer)
    {
        _customer = customer;
        
        // Configurar validador
        var validator = new CustomerStatusTransitionValidator();
        SetValidator<Customer, CustomerStatus>(validator);
    }

    protected override object? GetParentEntity() => _customer;

    protected override Enum GetStatusType(CustomerStatusEntity status) => status.Type;
}
```

### 5. Usar na Entidade Cliente

```csharp
public class Customer : Entity<EntityId>, IHasStatusHistory<CustomerStatusHistory>
{
    private readonly CustomerStatusHistory _statusHistory;

    public Customer()
    {
        _statusHistory = new CustomerStatusHistory(this);
    }

    public CustomerStatusHistory StatusHistory => _statusHistory;

    public void ChangeStatus(CustomerStatus newStatus, string notes)
    {
        var status = new CustomerStatusEntity(
            key: AlternateKey.New(),
            createdAt: DateTime.UtcNow,
            type: newStatus,
            notes: notes
        );

        // Validação automática acontece aqui!
        _statusHistory.Add(status);
    }

    public CustomerStatus GetCurrentStatus()
    {
        return _statusHistory.GetCurrent().Type;
    }
}
```

### 6. Uso

```csharp
var customer = new Customer();

// ✅ Transição permitida
customer.ChangeStatus(CustomerStatus.Active, "Cliente ativado");

// ✅ Transição permitida
customer.ChangeStatus(CustomerStatus.Suspended, "Cliente suspenso por violação");

// ✅ Transição permitida
customer.ChangeStatus(CustomerStatus.Active, "Cliente reativado");

// ✅ Transição permitida
customer.ChangeStatus(CustomerStatus.Banned, "Cliente banido permanentemente");

// ❌ Transição NÃO permitida - lança InvalidStatusTransitionException
customer.ChangeStatus(CustomerStatus.Active, "Tentativa de reativar cliente banido");
```

## Usando o Registry (Opcional)

Se você quiser usar um registry centralizado:

```csharp
// Durante inicialização da aplicação
var registry = new StatusTransitionValidatorRegistry();
registry.Register<Customer, CustomerStatus>(new CustomerStatusTransitionValidator());
registry.Register<Order, OrderStatus>(new OrderStatusTransitionValidator());

// No StatusHistory
public class CustomerStatusHistory : StatusHistory<CustomerStatusEntity>
{
    private readonly Customer _customer;
    private readonly StatusTransitionValidatorRegistry _registry;

    public CustomerStatusHistory(
        Customer customer, 
        StatusTransitionValidatorRegistry registry)
    {
        _customer = customer;
        _registry = registry;
        
        var validator = _registry.GetValidator<Customer, CustomerStatus>();
        if (validator != null)
        {
            SetValidator<Customer, CustomerStatus>(validator);
        }
    }

    protected override object? GetParentEntity() => _customer;
    protected override Enum GetStatusType(CustomerStatusEntity status) => status.Type;
}
```

## Vantagens

1. ✅ **Type-Safe**: Compile-time checking
2. ✅ **Performance**: Validação em memória (O(1) lookup)
3. ✅ **Flexível**: Cada entidade tem suas próprias regras
4. ✅ **Testável**: Fácil mockar validadores em testes
5. ✅ **Backward Compatible**: Funciona sem validador configurado

## Próximos Passos

Para adicionar validação a outras entidades:

1. Criar o enum de status
2. Criar a entidade de status (herdando de `Status<TEnum>`)
3. Implementar `IStatusTransitionValidator<TEntity, TStatus>`
4. Criar o `StatusHistory` implementando os métodos abstratos
5. Configurar o validador no construtor do `StatusHistory`

