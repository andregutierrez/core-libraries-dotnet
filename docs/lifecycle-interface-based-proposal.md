# Proposta: Sistema de Lifecycle Baseado em Interfaces

## Princípios

1. ✅ **Interfaces ativam características** - Apenas implementando a interface, a funcionalidade está disponível
2. ✅ **Mínimo código nas classes concretas** - Apenas propriedades simples
3. ✅ **Sem classes base obrigatórias** - Entidades herdam diretamente de `Entity<TKey>`
4. ✅ **Propriedades diretas** - Não usar objetos nullable complexos
5. ✅ **Métodos helper opcionais** - Extension methods ou métodos simples

---

## Estrutura Proposta

### 1. Interface IAuditable

```csharp
namespace Core.Libraries.Domain.Entities.Lifecycle;

/// <summary>
/// Interface para entidades que suportam rastreamento de criação e modificação.
/// </summary>
/// <remarks>
/// <para>
/// Ao implementar esta interface, a entidade deve fornecer propriedades para rastrear
/// quando foi criada e quando foi modificada pela última vez.
/// </para>
/// <para>
/// Exemplo de implementação mínima:
/// <code>
/// public class User : Entity&lt;EntityId&gt;, IAuditable
/// {
///     public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
///     public DateTime? UpdatedAt { get; protected set; }
///     public string? CreatedBy { get; protected set; }
///     public string? UpdatedBy { get; protected set; }
///     
///     public void MarkAsModified(string? updatedBy = null)
///     {
///         UpdatedAt = DateTime.UtcNow;
///         UpdatedBy = updatedBy;
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public interface IAuditable
{
    /// <summary>
    /// Data e hora de criação da entidade (UTC).
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Data e hora da última modificação da entidade (UTC).
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// Identificador do usuário que criou a entidade (opcional).
    /// </summary>
    string? CreatedBy { get; }

    /// <summary>
    /// Identificador do usuário que modificou a entidade pela última vez (opcional).
    /// </summary>
    string? UpdatedBy { get; }

    /// <summary>
    /// Marca a entidade como modificada, atualizando UpdatedAt e UpdatedBy.
    /// </summary>
    /// <param name="updatedBy">Identificador do usuário que realizou a modificação (opcional).</param>
    void MarkAsModified(string? updatedBy = null);
}
```

### 2. Interface ISoftDeletable

```csharp
namespace Core.Libraries.Domain.Entities.Lifecycle;

/// <summary>
/// Interface para entidades que suportam soft delete (deleção lógica).
/// </summary>
/// <remarks>
/// <para>
/// Ao implementar esta interface, a entidade pode ser marcada como deletada sem
/// ser fisicamente removida do banco de dados, permitindo auditoria e recuperação.
/// </para>
/// <para>
/// Exemplo de implementação mínima:
/// <code>
/// public class Customer : Entity&lt;EntityId&gt;, ISoftDeletable
/// {
///     public bool IsDeleted { get; protected set; }
///     public DateTime? DeletedAt { get; protected set; }
///     public string? DeletedBy { get; protected set; }
///     
///     public void MarkAsDeleted(string? deletedBy = null)
///     {
///         if (!IsDeleted)
///         {
///             IsDeleted = true;
///             DeletedAt = DateTime.UtcNow;
///             DeletedBy = deletedBy;
///         }
///     }
///     
///     public void Restore(string? restoredBy = null)
///     {
///         if (IsDeleted)
///         {
///             IsDeleted = false;
///             DeletedAt = null;
///             DeletedBy = null;
///         }
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public interface ISoftDeletable
{
    /// <summary>
    /// Indica se a entidade foi deletada (soft delete).
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Data e hora da deleção da entidade (UTC).
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Identificador do usuário que deletou a entidade (opcional).
    /// </summary>
    string? DeletedBy { get; }

    /// <summary>
    /// Marca a entidade como deletada (soft delete).
    /// </summary>
    /// <param name="deletedBy">Identificador do usuário que realizou a deleção (opcional).</param>
    void MarkAsDeleted(string? deletedBy = null);

    /// <summary>
    /// Restaura a entidade, removendo a marcação de deletada.
    /// </summary>
    /// <param name="restoredBy">Identificador do usuário que realizou a restauração (opcional).</param>
    void Restore(string? restoredBy = null);
}
```

### 3. Extension Methods (Opcional - para reduzir código)

```csharp
namespace Core.Libraries.Domain.Entities.Lifecycle;

/// <summary>
/// Métodos de extensão para facilitar o uso de interfaces de lifecycle.
/// </summary>
public static class LifecycleExtensions
{
    /// <summary>
    /// Verifica se a entidade foi modificada após a criação.
    /// </summary>
    public static bool HasBeenModified<T>(this T entity) where T : IAuditable
    {
        return entity.UpdatedAt.HasValue;
    }

    /// <summary>
    /// Obtém a data da última atividade (criação ou modificação).
    /// </summary>
    public static DateTime GetLastActivityDate<T>(this T entity) where T : IAuditable
    {
        return entity.UpdatedAt ?? entity.CreatedAt;
    }

    /// <summary>
    /// Verifica se a entidade está ativa (não deletada).
    /// </summary>
    public static bool IsActive<T>(this T entity) where T : ISoftDeletable
    {
        return !entity.IsDeleted;
    }
}
```

---

## Exemplos de Implementação

### Exemplo 1: Entidade Simples (sem lifecycle)

```csharp
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
}
```

### Exemplo 2: Entidade com Auditoria (mínimo código)

```csharp
public class User : Entity<EntityId>, IAuditable
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    // Propriedades IAuditable (mínimo necessário)
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    protected User() { }

    public User(string name, string email, string? createdBy = null)
    {
        Name = name;
        Email = email;
        CreatedBy = createdBy;
    }

    public void UpdateEmail(string newEmail, string? updatedBy = null)
    {
        Email = newEmail;
        MarkAsModified(updatedBy);
    }

    // Implementação mínima do método da interface
    public void MarkAsModified(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
```

### Exemplo 3: Entidade com Soft Delete (mínimo código)

```csharp
public class Customer : Entity<EntityId>, ISoftDeletable
{
    public string Name { get; private set; } = string.Empty;

    // Propriedades ISoftDeletable (mínimo necessário)
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    protected Customer() { }

    public Customer(string name)
    {
        Name = name;
    }

    public void Delete(string? deletedBy = null)
    {
        MarkAsDeleted(deletedBy);
    }

    // Implementação mínima dos métodos da interface
    public void MarkAsDeleted(string? deletedBy = null)
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }
    }

    public void Restore(string? restoredBy = null)
    {
        if (IsDeleted)
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
```

### Exemplo 4: Entidade com Ambos (mínimo código)

```csharp
public class Order : Entity<EntityId>, IAuditable, ISoftDeletable
{
    public string OrderNumber { get; private set; } = string.Empty;
    public decimal Total { get; private set; }

    // Propriedades IAuditable
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    // Propriedades ISoftDeletable
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    protected Order() { }

    public Order(string orderNumber, decimal total, string? createdBy = null)
    {
        OrderNumber = orderNumber;
        Total = total;
        CreatedBy = createdBy;
    }

    public void Cancel(string? cancelledBy = null)
    {
        MarkAsDeleted(cancelledBy);
        MarkAsModified(cancelledBy); // Atualiza também como modificação
    }

    // Implementação IAuditable
    public void MarkAsModified(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    // Implementação ISoftDeletable
    public void MarkAsDeleted(string? deletedBy = null)
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }
    }

    public void Restore(string? restoredBy = null)
    {
        if (IsDeleted)
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            MarkAsModified(restoredBy);
        }
    }
}
```

---

## Vantagens desta Abordagem

### ✅ Flexibilidade Total
- Cada entidade decide quais características implementar
- Não há herança forçada
- Pode combinar interfaces livremente

### ✅ Mínimo Código
- Apenas propriedades e métodos simples
- Sem classes base intermediárias
- Implementação direta e clara

### ✅ Ativação Condicional
- Características só existem se a interface for implementada
- Fácil verificar: `if (entity is IAuditable auditable) { ... }`
- Type-safe

### ✅ Sem Dependências Desnecessárias
- Entidades simples não precisam de lifecycle
- Apenas quem precisa implementa
- Reduz complexidade

---

## Comparação: Antes vs Depois

### Antes (com classes base)
```csharp
public class User : AuditableEntity<EntityId> // Forçado a ter auditoria
{
    // CreatedAt, UpdatedAt já estão lá (mesmo que não precise)
}
```

### Depois (com interfaces)
```csharp
// Sem lifecycle
public class Product : Entity<EntityId> { }

// Com auditoria (se quiser)
public class User : Entity<EntityId>, IAuditable
{
    // Apenas o mínimo necessário
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    // ...
}
```

---

## Próximos Passos

1. **Atualizar namespaces** - De `Core.LibrariesDomain.*` para `Core.Libraries.Domain.*`
2. **Criar interfaces** - `IAuditable` e `ISoftDeletable` com propriedades diretas
3. **Remover classes antigas** - `AuditInfo` e `DeletionInfo` (ou manter para compatibilidade)
4. **Criar extension methods** - Para facilitar uso (opcional)
5. **Documentação** - Exemplos de implementação mínima

---

## Perguntas

1. **Extension methods**: Criar métodos de extensão para facilitar uso ou deixar apenas as interfaces?
2. **Validações**: Adicionar validações nas interfaces (ex: não permitir modificar entidade deletada)?
3. **Eventos**: As interfaces devem disparar eventos de domínio automaticamente?

