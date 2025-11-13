# Exemplos de Uso: Sistema de Lifecycle Baseado em Interfaces

## Princípio

As características de lifecycle (auditoria e soft delete) são **ativadas apenas quando a entidade implementa as interfaces** correspondentes. As classes concretas têm o **mínimo de código necessário**.

---

## Exemplo 1: Entidade Simples (Sem Lifecycle)

```csharp
using Core.Libraries.Domain.Entities;

public class Product : Entity<EntityId>
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    protected Product() { }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
        // Sem auditoria - não implementa IAuditable
    }
}
```

**Características**: Nenhuma (não implementa interfaces de lifecycle)

---

## Exemplo 2: Entidade com Auditoria (Mínimo Código)

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

public class User : Entity<EntityId>, IAuditable
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    // Propriedade IAuditable (mínimo necessário)
    private readonly AuditInfo _audit = new();
    public AuditInfo Audit => _audit;

    protected User() { }

    public User(string name, string email, string? createdBy = null)
    {
        Name = name;
        Email = email;
        
        // Inicializar com usuário criador se fornecido
        if (createdBy != null)
        {
            _audit = new AuditInfo(createdBy);
        }
    }

    public void UpdateEmail(string newEmail, string? updatedBy = null)
    {
        Email = newEmail;
        _audit.MarkAsModified(updatedBy); // Atualiza UpdatedAt e UpdatedBy (se fornecido)
    }
}
```

**Características**: Auditoria ativada (implementa `IAuditable`)

---

## Exemplo 3: Entidade com Soft Delete (Mínimo Código)

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

public class Customer : Entity<EntityId>, ISoftDeletable
{
    public string Name { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;

    // Propriedade ISoftDeletable (mínimo necessário)
    private readonly DeletionInfo _deletion = new();
    public DeletionInfo Deletion => _deletion;

    protected Customer() { }

    public Customer(string name, string document)
    {
        Name = name;
        Document = document;
    }

    public void Delete(string? deletedBy = null)
    {
        _deletion.MarkAsDeleted(deletedBy); // Soft delete com rastreamento opcional de usuário
    }

    public void RestoreCustomer()
    {
        _deletion.Restore(); // Restaura a entidade
    }
}
```

**Características**: Soft delete ativado (implementa `ISoftDeletable`)

---

## Exemplo 4: Entidade com Ambos (Mínimo Código)

```csharp
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Lifecycle;

public class Order : Entity<EntityId>, IAuditable, ISoftDeletable
{
    public string OrderNumber { get; private set; } = string.Empty;
    public decimal Total { get; private set; }

    // Propriedades das interfaces (mínimo necessário)
    private readonly AuditInfo _audit = new();
    private readonly DeletionInfo _deletion = new();
    
    public AuditInfo Audit => _audit;
    public DeletionInfo Deletion => _deletion;

    protected Order() { }

    public Order(string orderNumber, decimal total, string? createdBy = null)
    {
        OrderNumber = orderNumber;
        Total = total;
        
        // Inicializar com usuário criador se fornecido
        if (createdBy != null)
        {
            _audit = new AuditInfo(createdBy);
        }
    }

    public void Cancel(string? cancelledBy = null)
    {
        _deletion.MarkAsDeleted(cancelledBy); // Soft delete
        _audit.MarkAsModified(cancelledBy); // Atualiza também como modificação
    }

    public void RestoreOrder(string? restoredBy = null)
    {
        _deletion.Restore(); // Restaura
        _audit.MarkAsModified(restoredBy); // Marca como modificada ao restaurar
    }
}
```

**Características**: Auditoria + Soft delete ativados (implementa ambas interfaces)

---

## Verificação de Características

### Verificar se entidade suporta auditoria

```csharp
if (entity is IAuditable auditable)
{
    Console.WriteLine($"Criado em: {auditable.Audit.CreatedAt}");
    Console.WriteLine($"Modificado em: {auditable.Audit.UpdatedAt}");
    Console.WriteLine($"Criado por: {auditable.Audit.CreatedBy ?? "N/A"}");
    Console.WriteLine($"Modificado por: {auditable.Audit.UpdatedBy ?? "N/A"}");
}
```

### Verificar se entidade suporta soft delete

```csharp
if (entity is ISoftDeletable deletable)
{
    if (deletable.Deletion.IsDeleted)
    {
        Console.WriteLine($"Deletado em: {deletable.Deletion.DeletedAt} por {deletable.Deletion.DeletedBy ?? "N/A"}");
    }
}
```

### Filtrar entidades não deletadas

```csharp
var activeCustomers = customers
    .Where(c => c is not ISoftDeletable deletable || !deletable.Deletion.IsDeleted)
    .ToList();
```

---

## Vantagens desta Abordagem

1. ✅ **Mínimo código**: Apenas propriedades e métodos simples
2. ✅ **Flexibilidade**: Cada entidade decide quais características implementar
3. ✅ **Type-safe**: Verificação com `is` pattern matching
4. ✅ **Sem herança forçada**: Não precisa herdar de classes base
5. ✅ **Ativação condicional**: Características só existem se a interface for implementada

---

## Padrão de Implementação Mínima

### Para IAuditable:
```csharp
// Propriedade (mínimo necessário)
private readonly AuditInfo _audit = new();
public AuditInfo Audit => _audit;

// No construtor (opcional - para rastrear usuário criador)
public MyEntity(string? createdBy = null)
{
    if (createdBy != null)
    {
        _audit = new AuditInfo(createdBy);
    }
}

// Ao modificar
public void UpdateSomething(string? updatedBy = null)
{
    // ... lógica de atualização ...
    _audit.MarkAsModified(updatedBy); // Rastreamento opcional de usuário
}
```

### Para ISoftDeletable:
```csharp
// Propriedade (mínimo necessário)
private readonly DeletionInfo _deletion = new();
public DeletionInfo Deletion => _deletion;

// Ao deletar
public void Delete(string? deletedBy = null)
{
    _deletion.MarkAsDeleted(deletedBy); // Rastreamento opcional de usuário
}

// Ao restaurar
public void Restore()
{
    _deletion.Restore();
}
```

## Rastreamento de Usuário (Opcional)

O rastreamento de usuário é **totalmente opcional**. Você pode:

### Sem rastreamento de usuário:
```csharp
public class Product : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit = new(); // Sem usuário
    public AuditInfo Audit => _audit;
    
    public void UpdatePrice(decimal price)
    {
        Price = price;
        _audit.MarkAsModified(); // Sem passar usuário
    }
}
```

### Com rastreamento de usuário:
```csharp
public class User : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit;
    public AuditInfo Audit => _audit;
    
    public User(string name, string? createdBy = null)
    {
        Name = name;
        _audit = createdBy != null ? new AuditInfo(createdBy) : new AuditInfo();
    }
    
    public void UpdateEmail(string email, string? updatedBy = null)
    {
        Email = email;
        _audit.MarkAsModified(updatedBy); // Com usuário (se fornecido)
    }
}
```

### Comportamento do rastreamento:
- Se `updatedBy` for fornecido → atualiza o campo
- Se `updatedBy` for `null` → mantém o valor atual (não altera)
- Permite manter o último valor conhecido ou deixar como `null`

