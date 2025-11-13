# Propostas para Sistema de Transições de Status Flexível

## Contexto Atual

O sistema atual de `StatusHistory` permite adicionar novos status, mas não valida transições entre estados. Existe uma exceção `InvalidStatusTransitionException`, mas não há mecanismo de validação implementado.

## Requisitos

1. ✅ Controle refinado de transições (ex: "pedido pago não pode ser cancelado")
2. ✅ Regras diferentes por tipo de entidade (Pedido vs Cliente)
3. ✅ Flexibilidade para alterar regras sem modificar classes de domínio
4. ✅ Extensibilidade para novos tipos de entidades

---

## Proposta 1: Strategy Pattern com Registry (Recomendada)

### Conceito
Criar uma interface `IStatusTransitionValidator<TEntity, TStatus>` e um registry que mapeia tipos de entidade para validadores específicos.

### Estrutura Proposta

```csharp
// Interface base para validadores
public interface IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
    void ValidateTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
}

// Registry para gerenciar validadores
public class StatusTransitionValidatorRegistry
{
    private readonly Dictionary<Type, object> _validators = new();
    
    public void Register<TEntity, TStatus>(IStatusTransitionValidator<TEntity, TStatus> validator)
        where TStatus : Enum;
    
    public IStatusTransitionValidator<TEntity, TStatus>? GetValidator<TEntity, TStatus>()
        where TStatus : Enum;
}

// Exemplo de implementação para Pedido
public class OrderStatusTransitionValidator : IStatusTransitionValidator<Order, OrderStatus>
{
    private static readonly Dictionary<OrderStatus, HashSet<OrderStatus>> AllowedTransitions = new()
    {
        [OrderStatus.Pending] = new() { OrderStatus.Paid, OrderStatus.Cancelled },
        [OrderStatus.Paid] = new() { OrderStatus.Shipped, OrderStatus.Refunded },
        [OrderStatus.Shipped] = new() { OrderStatus.Delivered },
        [OrderStatus.Cancelled] = new(), // Estado final
        [OrderStatus.Delivered] = new(), // Estado final
        [OrderStatus.Refunded] = new() // Estado final
    };
    
    public bool CanTransition(OrderStatus fromStatus, OrderStatus toStatus, Order order)
    {
        return AllowedTransitions.TryGetValue(fromStatus, out var allowed) 
            && allowed.Contains(toStatus);
    }
    
    public void ValidateTransition(OrderStatus fromStatus, OrderStatus toStatus, Order order)
    {
        if (!CanTransition(fromStatus, toStatus, order))
        {
            throw new InvalidStatusTransitionException(
                domainContext: "Order",
                fromStatus: fromStatus.ToString(),
                toStatus: toStatus.ToString(),
                entityId: order.Id.ToString()
            );
        }
    }
}
```

### Vantagens
- ✅ Type-safe com genéricos
- ✅ Fácil de testar (cada validador isolado)
- ✅ Permite lógica complexa por entidade
- ✅ Performance (validação em memória)
- ✅ Compile-time safety

### Desvantagens
- ⚠️ Requer recompilação para mudar regras
- ⚠️ Código distribuído em múltiplas classes

---

## Proposta 2: Configuration-Based (Mais Flexível)

### Conceito
Definir regras de transição em configuração (JSON, banco de dados) e carregar em runtime.

### Estrutura Proposta

```csharp
// Modelo de configuração
public class StatusTransitionRule
{
    public string EntityType { get; set; } = string.Empty;
    public string FromStatus { get; set; } = string.Empty;
    public HashSet<string> AllowedToStatuses { get; set; } = new();
    public List<TransitionCondition>? Conditions { get; set; } // Para regras condicionais
}

public class TransitionCondition
{
    public string Property { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty; // "equals", "greaterThan", etc.
    public object? Value { get; set; }
}

// Validador baseado em configuração
public class ConfigurationBasedStatusTransitionValidator<TEntity, TStatus> 
    : IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    private readonly List<StatusTransitionRule> _rules;
    
    public ConfigurationBasedStatusTransitionValidator(List<StatusTransitionRule> rules)
    {
        _rules = rules;
    }
    
    public bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity)
    {
        var entityType = typeof(TEntity).Name;
        var rule = _rules.FirstOrDefault(r => 
            r.EntityType == entityType && 
            r.FromStatus == fromStatus.ToString());
            
        return rule?.AllowedToStatuses.Contains(toStatus.ToString()) ?? false;
    }
    
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

// Exemplo de configuração JSON
{
  "statusTransitionRules": [
    {
      "entityType": "Order",
      "fromStatus": "Paid",
      "allowedToStatuses": ["Shipped", "Refunded"],
      "conditions": null
    },
    {
      "entityType": "Order",
      "fromStatus": "Pending",
      "allowedToStatuses": ["Paid", "Cancelled"]
    },
    {
      "entityType": "Customer",
      "fromStatus": "Active",
      "allowedToStatuses": ["Inactive", "Suspended"]
    }
  ]
}
```

### Vantagens
- ✅ Máxima flexibilidade (mudanças sem recompilação)
- ✅ Centralizado (todas as regras em um lugar)
- ✅ Pode ser carregado de banco de dados
- ✅ Fácil de versionar e auditar

### Desvantagens
- ⚠️ Menos type-safe (strings)
- ⚠️ Validação em runtime pode ser mais lenta
- ⚠️ Requer parsing e validação de configuração
- ⚠️ Lógica complexa pode ser difícil de expressar

---

## Proposta 3: Hybrid (Recomendada para Flexibilidade Máxima)

### Conceito
Combinar Strategy Pattern com suporte a configuração. Permite validadores customizados quando necessário, mas também suporta configuração simples.

### Estrutura Proposta

```csharp
// Interface base
public interface IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
    void ValidateTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
}

// Validador baseado em regras simples (configuração)
public class RuleBasedStatusTransitionValidator<TEntity, TStatus> 
    : IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    private readonly StatusTransitionRules<TStatus> _rules;
    
    public RuleBasedStatusTransitionValidator(StatusTransitionRules<TStatus> rules)
    {
        _rules = rules;
    }
    
    public bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity)
    {
        return _rules.IsTransitionAllowed(fromStatus, toStatus);
    }
    
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

// Classe para definir regras de forma fluente
public class StatusTransitionRules<TStatus>
    where TStatus : Enum
{
    private readonly Dictionary<TStatus, HashSet<TStatus>> _allowedTransitions = new();
    
    public StatusTransitionRules<TStatus> Allow(TStatus from, params TStatus[] to)
    {
        if (!_allowedTransitions.ContainsKey(from))
            _allowedTransitions[from] = new HashSet<TStatus>();
            
        foreach (var status in to)
            _allowedTransitions[from].Add(status);
            
        return this;
    }
    
    public bool IsTransitionAllowed(TStatus from, TStatus to)
    {
        return _allowedTransitions.TryGetValue(from, out var allowed) 
            && allowed.Contains(to);
    }
}

// Validador customizado para lógica complexa
public class OrderStatusTransitionValidator : IStatusTransitionValidator<Order, OrderStatus>
{
    public bool CanTransition(OrderStatus fromStatus, OrderStatus toStatus, Order order)
    {
        // Regras simples
        if (fromStatus == OrderStatus.Paid && toStatus == OrderStatus.Cancelled)
            return false;
            
        // Regras condicionais
        if (fromStatus == OrderStatus.Pending && toStatus == OrderStatus.Paid)
            return order.TotalAmount > 0;
            
        // Regras baseadas em data
        if (fromStatus == OrderStatus.Shipped && toStatus == OrderStatus.Delivered)
            return DateTime.UtcNow >= order.ShippedDate?.AddDays(1);
            
        return true; // Ou usar um validador baseado em regras como fallback
    }
    
    public void ValidateTransition(OrderStatus fromStatus, OrderStatus toStatus, Order order)
    {
        if (!CanTransition(fromStatus, toStatus, order))
        {
            throw new InvalidStatusTransitionException(
                domainContext: "Order",
                fromStatus: fromStatus.ToString(),
                toStatus: toStatus.ToString(),
                entityId: order.Id.ToString()
            );
        }
    }
}
```

### Uso

```csharp
// Para regras simples - usar RuleBased
var orderRules = new StatusTransitionRules<OrderStatus>()
    .Allow(OrderStatus.Pending, OrderStatus.Paid, OrderStatus.Cancelled)
    .Allow(OrderStatus.Paid, OrderStatus.Shipped, OrderStatus.Refunded)
    .Allow(OrderStatus.Shipped, OrderStatus.Delivered);

var validator = new RuleBasedStatusTransitionValidator<Order, OrderStatus>(orderRules);
registry.Register<Order, OrderStatus>(validator);

// Para lógica complexa - usar validador customizado
var customValidator = new OrderStatusTransitionValidator();
registry.Register<Order, OrderStatus>(customValidator);
```

### Vantagens
- ✅ Melhor dos dois mundos
- ✅ Type-safe quando possível
- ✅ Flexível para casos complexos
- ✅ Permite evolução gradual (começar simples, adicionar complexidade depois)

### Desvantagens
- ⚠️ Mais complexo de implementar
- ⚠️ Requer decisão sobre quando usar cada abordagem

---

## Proposta 4: Rule Engine com Expressões

### Conceito
Criar um sistema de regras mais genérico que pode avaliar condições complexas.

### Estrutura Proposta

```csharp
public interface IStatusTransitionRule<TEntity, TStatus>
    where TStatus : Enum
{
    string Name { get; }
    bool Evaluate(TStatus fromStatus, TStatus toStatus, TEntity entity);
}

public class StatusTransitionRuleBuilder<TEntity, TStatus>
    where TStatus : Enum
{
    private readonly List<IStatusTransitionRule<TEntity, TStatus>> _rules = new();
    
    public StatusTransitionRuleBuilder<TEntity, TStatus> AddRule(
        string name,
        Func<TStatus, TStatus, TEntity, bool> condition)
    {
        _rules.Add(new LambdaStatusTransitionRule<TEntity, TStatus>(name, condition));
        return this;
    }
    
    public bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity)
    {
        return _rules.All(r => r.Evaluate(fromStatus, toStatus, entity));
    }
}

// Uso
var orderRules = new StatusTransitionRuleBuilder<Order, OrderStatus>()
    .AddRule("PaidCannotBeCancelled", (from, to, order) => 
        !(from == OrderStatus.Paid && to == OrderStatus.Cancelled))
    .AddRule("MustHaveItems", (from, to, order) => 
        to != OrderStatus.Paid || order.Items.Any())
    .AddRule("RefundRequiresPayment", (from, to, order) => 
        to != OrderStatus.Refunded || from == OrderStatus.Paid);
```

### Vantagens
- ✅ Muito flexível
- ✅ Permite regras complexas
- ✅ Pode ser combinado com configuração

### Desvantagens
- ⚠️ Pode ser difícil de debugar
- ⚠️ Performance pode ser um problema com muitas regras
- ⚠️ Requer cuidado para não criar regras conflitantes

---

## Recomendação: Proposta 3 (Hybrid)

### Por quê?

1. **Flexibilidade Gradual**: Começa simples com regras básicas, evolui para lógica complexa quando necessário
2. **Type Safety**: Mantém type safety onde possível
3. **Testabilidade**: Fácil de testar cada validador isoladamente
4. **Performance**: Validação rápida em memória
5. **Extensibilidade**: Fácil adicionar novos tipos de entidades

### Integração com StatusHistory

```csharp
public abstract class StatusHistory<TStatus> : EntityList<TStatus>, IStatusHistory<TStatus> 
    where TStatus : IStatusBase
{
    private readonly IStatusTransitionValidator<object, TStatus>? _validator;
    
    protected StatusHistory(IStatusTransitionValidator<object, TStatus>? validator = null)
    {
        _validator = validator;
    }
    
    public void Add(TStatus entity)
    {
        var currentStatus = GetCurrentOrDefault();
        var newStatusType = GetStatusType(entity);
        
        // Validar transição se validador estiver configurado
        if (_validator != null && currentStatus != null)
        {
            var currentStatusType = GetStatusType(currentStatus);
            _validator.ValidateTransition(currentStatusType, newStatusType, this);
        }
        
        // Resto da lógica existente...
        foreach (var status in this.Where(s => s.IsCurrent))
            status.Deactivate();
            
        _list.Add(entity);
    }
}
```

---

## Próximos Passos para Discussão

1. **Qual abordagem faz mais sentido para o seu contexto?**
2. **Você precisa de regras condicionais complexas ou regras simples são suficientes?**
3. **As regras mudam frequentemente ou são relativamente estáveis?**
4. **Você precisa de suporte a regras dinâmicas (carregadas de banco de dados)?**
5. **Performance é crítica ou flexibilidade é mais importante?**

---

## Exemplo Completo de Uso (Proposta 3)

```csharp
// 1. Definir regras para Pedido
var orderRules = new StatusTransitionRules<OrderStatus>()
    .Allow(OrderStatus.Pending, OrderStatus.Paid, OrderStatus.Cancelled)
    .Allow(OrderStatus.Paid, OrderStatus.Shipped, OrderStatus.Refunded)
    .Allow(OrderStatus.Shipped, OrderStatus.Delivered);

var orderValidator = new RuleBasedStatusTransitionValidator<Order, OrderStatus>(orderRules);

// 2. Registrar no registry
var registry = new StatusTransitionValidatorRegistry();
registry.Register<Order, OrderStatus>(orderValidator);

// 3. Usar no StatusHistory
public class OrderStatusHistory : StatusHistory<OrderStatus>
{
    public OrderStatusHistory(StatusTransitionValidatorRegistry registry)
        : base(registry.GetValidator<Order, OrderStatus>())
    {
    }
}

// 4. Uso na entidade
public class Order : Entity<EntityId>, IHasStatusHistory<OrderStatusHistory>
{
    private readonly OrderStatusHistory _statusHistory;
    
    public OrderStatusHistory StatusHistory => _statusHistory;
    
    public void ChangeStatus(OrderStatus newStatus, string notes)
    {
        // Validação automática acontece no Add()
        var status = new OrderStatus(
            key: AlternateKey.New(),
            createdAt: DateTime.UtcNow,
            type: newStatus,
            notes: notes
        );
        
        _statusHistory.Add(status);
    }
}
```

