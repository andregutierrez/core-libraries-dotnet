# Exemplos: Tipos de Identificador de Usuário no Lifecycle

## Visão Geral

As classes `AuditInfo` e `DeletionInfo` agora suportam diferentes tipos de identificador de usuário através de versões genéricas, mantendo `string` como padrão para compatibilidade.

---

## Opções Disponíveis

### 1. String (Padrão - Compatibilidade)

A versão não-genérica usa `string?` como tipo de identificador, mantendo compatibilidade com código existente.

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

public class User : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit = new();
    public AuditInfo Audit => _audit;
    
    public User(string? createdBy = null)
    {
        if (createdBy != null)
        {
            _audit = new AuditInfo(createdBy);
        }
    }
    
    public void UpdateEmail(string email, string? updatedBy = null)
    {
        Email = email;
        _audit.MarkAsModified(updatedBy);
    }
}
```

**Vantagens:**
- ✅ Simples e flexível
- ✅ Compatível com qualquer sistema de autenticação
- ✅ Fácil de serializar/deserializar

---

### 2. Guid (Para Sistemas com GUIDs)

Use quando seu sistema usa GUIDs como identificadores de usuário.

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

public class Order : Entity<EntityId>, IAuditable<Guid>, ISoftDeletable<Guid>
{
    private readonly AuditInfo<Guid> _audit = new();
    private readonly DeletionInfo<Guid> _deletion = new();
    
    public AuditInfo<Guid> Audit => _audit;
    public DeletionInfo<Guid> Deletion => _deletion;
    
    public Order(Guid? createdBy = null)
    {
        if (createdBy.HasValue)
        {
            _audit = new AuditInfo<Guid>(createdBy.Value);
        }
    }
    
    public void Cancel(Guid? cancelledBy = null)
    {
        _deletion.MarkAsDeleted(cancelledBy);
        _audit.MarkAsModified(cancelledBy);
    }
}
```

**Vantagens:**
- ✅ Type-safe
- ✅ Evita erros de conversão
- ✅ Ideal para sistemas distribuídos

---

### 3. Int (Para IDs Numéricos)

Use quando seu sistema usa IDs numéricos (inteiros) como identificadores.

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

public class Product : Entity<EntityId>, IAuditable<int>
{
    private readonly AuditInfo<int> _audit = new();
    public AuditInfo<int> Audit => _audit;
    
    public Product(int? createdBy = null)
    {
        if (createdBy.HasValue)
        {
            _audit = new AuditInfo<int>(createdBy.Value);
        }
    }
    
    public void UpdatePrice(decimal price, int? updatedBy = null)
    {
        Price = price;
        _audit.MarkAsModified(updatedBy);
    }
}
```

**Vantagens:**
- ✅ Eficiente em banco de dados
- ✅ Type-safe
- ✅ Ideal para sistemas com IDs sequenciais

---

### 4. Tipo Customizado (EntityId, UserId, etc.)

Use quando você tem tipos fortemente tipados para identificadores de usuário.

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

// Definir tipo customizado
public record UserId : EntityId
{
    public UserId(int value) : base(value) { }
}

// Usar na entidade
public class Document : Entity<EntityId>, IAuditable<UserId>, ISoftDeletable<UserId>
{
    private readonly AuditInfo<UserId> _audit = new();
    private readonly DeletionInfo<UserId> _deletion = new();
    
    public AuditInfo<UserId> Audit => _audit;
    public DeletionInfo<UserId> Deletion => _deletion;
    
    public Document(UserId? createdBy = null)
    {
        if (createdBy != null)
        {
            _audit = new AuditInfo<UserId>(createdBy);
        }
    }
    
    public void Delete(UserId? deletedBy = null)
    {
        _deletion.MarkAsDeleted(deletedBy);
    }
}
```

**Vantagens:**
- ✅ Máxima type safety
- ✅ Previne erros de programação
- ✅ Alinhado com DDD (Domain-Driven Design)
- ✅ Clareza semântica

---

## Interfaces Genéricas

Para usar tipos customizados, você também precisa criar interfaces genéricas correspondentes:

```csharp
// Interface genérica (opcional - para tipos customizados)
public interface IAuditable<TUserId>
{
    AuditInfo<TUserId> Audit { get; }
}

// Interface não-genérica (padrão - usa string)
public interface IAuditable : IAuditable<string>
{
    // Herda tudo de IAuditable<string>
}

// Uso
public class Order : Entity<EntityId>, IAuditable<Guid>
{
    // ...
}
```

---

## Comparação de Abordagens

| Tipo | Type Safety | Flexibilidade | Performance | Recomendado Para |
|------|------------|---------------|-------------|------------------|
| `string` | ⚠️ Baixa | ✅ Alta | ✅ Boa | Sistemas simples, APIs externas |
| `Guid` | ✅ Alta | ✅ Alta | ✅ Boa | Sistemas distribuídos, microserviços |
| `int` | ✅ Alta | ⚠️ Média | ✅ Excelente | Sistemas com IDs sequenciais |
| `UserId` (customizado) | ✅ Muito Alta | ⚠️ Baixa | ✅ Boa | DDD, sistemas complexos |

---

## Migração de Código Existente

### Antes (string)
```csharp
public class User : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit = new();
    public AuditInfo Audit => _audit;
}
```

### Depois (Guid)
```csharp
public class User : Entity<EntityId>, IAuditable<Guid>
{
    private readonly AuditInfo<Guid> _audit = new();
    public AuditInfo<Guid> Audit => _audit;
}
```

**Nota:** O código existente continua funcionando sem alterações, pois `AuditInfo` (não-genérico) ainda existe e usa `string` como padrão.

---

## Recomendações

1. **Para novos projetos**: Considere usar `Guid` ou tipos customizados para melhor type safety
2. **Para projetos existentes**: Continue usando `string` para manter compatibilidade
3. **Para DDD**: Use tipos customizados (`UserId`, `EntityId`, etc.) para máxima clareza semântica
4. **Para performance**: Use `int` se IDs numéricos forem suficientes

---

## Exemplo Completo: Sistema com UserId Customizado

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

// 1. Definir tipo de usuário
public record UserId : EntityId
{
    public UserId(int value) : base(value) { }
    
    public static implicit operator UserId(int value) => new(value);
    public static implicit operator int(UserId userId) => userId.Value;
}

// 2. Criar interfaces genéricas (se necessário)
public interface IAuditable<TUserId>
{
    AuditInfo<TUserId> Audit { get; }
}

public interface ISoftDeletable<TUserId>
{
    DeletionInfo<TUserId> Deletion { get; }
}

// 3. Implementar na entidade
public class Order : Entity<EntityId>, IAuditable<UserId>, ISoftDeletable<UserId>
{
    public string OrderNumber { get; private set; } = string.Empty;
    
    private readonly AuditInfo<UserId> _audit = new();
    private readonly DeletionInfo<UserId> _deletion = new();
    
    public AuditInfo<UserId> Audit => _audit;
    public DeletionInfo<UserId> Deletion => _deletion;
    
    public Order(string orderNumber, UserId? createdBy = null)
    {
        OrderNumber = orderNumber;
        if (createdBy != null)
        {
            _audit = new AuditInfo<UserId>(createdBy);
        }
    }
    
    public void Cancel(UserId? cancelledBy = null)
    {
        _deletion.MarkAsDeleted(cancelledBy);
        _audit.MarkAsModified(cancelledBy);
    }
}

// 4. Uso
var userId = new UserId(123);
var order = new Order("ORD-001", userId);
order.Cancel(userId);
```

---

## Conclusão

A flexibilidade de tipos permite que você escolha a abordagem que melhor se adequa ao seu domínio, mantendo type safety e facilitando a manutenção do código.

