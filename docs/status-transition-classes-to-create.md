# Classes Abstratas para Sistema de Transições de Status

## Estrutura de Classes a Criar

### 1. Interface Base (Essencial)

**Arquivo**: `src/Core.Libraries.Domain/Entities/Statuses/IStatusTransitionValidator.cs`

```csharp
namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Define o contrato para validação de transições de status entre estados de uma entidade.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade que possui o histórico de status.</typeparam>
/// <typeparam name="TStatus">O tipo enum que representa os possíveis status.</typeparam>
public interface IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    bool CanTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
    void ValidateTransition(TStatus fromStatus, TStatus toStatus, TEntity entity);
}
```

**Por quê**: Interface base que permite implementar validadores customizados como no Exemplo 3.

---

### 2. Registry (Essencial)

**Arquivo**: `src/Core.Libraries.Domain/Entities/Statuses/StatusTransitionValidatorRegistry.cs`

```csharp
namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Registry centralizado para gerenciar validadores de transição de status.
/// </summary>
public class StatusTransitionValidatorRegistry
{
    private readonly Dictionary<Type, object> _validators = new();

    public void Register<TEntity, TStatus>(IStatusTransitionValidator<TEntity, TStatus> validator)
        where TStatus : Enum;
    
    public IStatusTransitionValidator<TEntity, TStatus>? GetValidator<TEntity, TStatus>()
        where TStatus : Enum;
    
    public bool HasValidator<TEntity, TStatus>()
        where TStatus : Enum;
}
```

**Por quê**: Permite registrar e recuperar validadores de forma centralizada, facilitando injeção de dependência.

---

### 3. Helper para Regras Simples (Opcional mas Recomendado)

**Arquivo**: `src/Core.Libraries.Domain/Entities/Statuses/StatusTransitionRules.cs`

```csharp
namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Classe helper para criar validadores baseados em regras simples de transição.
/// </summary>
public class StatusTransitionRules<TStatus>
    where TStatus : Enum
{
    // Métodos para definir regras de forma fluente
    public StatusTransitionRules<TStatus> Allow(TStatus fromStatus, params TStatus[] toStatuses);
    public bool IsTransitionAllowed(TStatus fromStatus, TStatus toStatus);
    public IReadOnlySet<TStatus> GetAllowedTransitions(TStatus fromStatus);
}
```

**Por quê**: Facilita criação de validadores para casos simples (Exemplo 1), mas não obrigatório se você sempre usar implementações customizadas.

---

### 4. Validador Baseado em Regras (Opcional mas Recomendado)

**Arquivo**: `src/Core.Libraries.Domain/Entities/Statuses/RuleBasedStatusTransitionValidator.cs`

```csharp
namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Validador baseado em regras simples de transição.
/// </summary>
public class RuleBasedStatusTransitionValidator<TEntity, TStatus> 
    : IStatusTransitionValidator<TEntity, TStatus>
    where TStatus : Enum
{
    // Implementação usando StatusTransitionRules
}
```

**Por quê**: Implementação pronta para casos simples, mas você pode criar validadores customizados diretamente.

---

### 5. Modificação em StatusHistory (Essencial)

**Arquivo**: `src/Core.Libraries.Domain/Entities/Statuses/StatusHistory.cs` (modificar existente)

Adicionar:
- Construtor opcional que aceita validador
- Método abstrato para obter entidade pai
- Validação no método `Add()`

---

## Resumo: O que Criar

### ✅ Obrigatório (Essencial)

1. ✅ **IStatusTransitionValidator.cs** - Interface base
2. ✅ **StatusTransitionValidatorRegistry.cs** - Registry
3. ✅ **Modificar StatusHistory.cs** - Integração

### ⚠️ Opcional (Mas Recomendado)

4. ⚠️ **StatusTransitionRules.cs** - Helper para regras simples
5. ⚠️ **RuleBasedStatusTransitionValidator.cs** - Validador baseado em regras

---

## Decisão de Design Necessária

Para integrar com `StatusHistory`, precisamos decidir como obter:
1. **A entidade pai** (Order, Customer, etc.)
2. **O tipo enum do status** (OrderStatus, CustomerStatus, etc.)

### Opção Recomendada: Métodos Abstratos

```csharp
public abstract class StatusHistory<TStatus> : EntityList<TStatus>, IStatusHistory<TStatus> 
    where TStatus : IStatusBase
{
    // Método abstrato para obter entidade pai
    protected abstract object? GetParentEntity();
    
    // Método abstrato para extrair tipo enum do status
    protected abstract Enum GetStatusType(TStatus status);
    
    // Validador opcional
    private readonly IStatusTransitionValidator<object, Enum>? _validator;
}
```

**Vantagens**:
- ✅ Type-safe
- ✅ Performance (sem reflection)
- ✅ Flexível (cada classe derivada implementa)

---

## Estrutura de Arquivos

```
src/Core.Libraries.Domain/Entities/Statuses/
├── IStatusTransitionValidator.cs          (NOVO - Essencial)
├── StatusTransitionValidatorRegistry.cs   (NOVO - Essencial)
├── StatusTransitionRules.cs               (NOVO - Opcional)
├── RuleBasedStatusTransitionValidator.cs  (NOVO - Opcional)
├── StatusHistory.cs                       (MODIFICAR - Adicionar validação)
├── IStatus.cs                             (Existente)
├── Status.cs                              (Existente)
└── ...
```

---

## Próximo Passo

Posso criar todas essas classes agora. Você prefere:

**A)** Criar tudo (incluindo opcionais) - mais completo
**B)** Criar apenas o essencial (1, 2, 3) - mais enxuto

Qual você prefere?

