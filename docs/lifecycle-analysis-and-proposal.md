# Análise e Proposta: Sistema de Lifecycle e Auditoria

## Análise da Estrutura Atual

### Arquivos Existentes

1. **ISoftDeletable.cs** - Interface para soft delete
   - Retorna `DeletionInfo?` (nullable)
   - Problema: Informação não está diretamente na entidade

2. **IAuditable.cs** - Interface para auditoria
   - Retorna `AuditInfo?` (nullable)
   - Problema: Informação não está diretamente na entidade

3. **DeletionInfo.cs** - Classe para informações de deleção
   - `IsDeleted`, `DeletedAt`
   - Métodos: `MarkAsDeleted()`, `Restore()`
   - Problema: Usa `DateTimeOffset` enquanto outras partes usam `DateTime`

4. **AuditInfo.cs** - Classe para informações de auditoria
   - `CreatedAt`, `UpdatedAt`
   - Método: `Touch()`
   - Problema: Não inicializa `CreatedAt` automaticamente
   - Problema: Não rastreia quem criou/modificou

### Problemas Identificados

1. ❌ **Namespaces antigos**: Usando `Core.LibrariesDomain.*` ao invés de `Core.Libraries.Domain.*`
2. ❌ **Propriedades nullable**: Interfaces retornam objetos nullable, dificultando uso
3. ❌ **Falta de integração**: Não há integração com `Entity<TKey>` base
4. ❌ **Inconsistência de tipos**: `DateTimeOffset` vs `DateTime`
5. ❌ **Falta rastreamento de usuário**: Não rastreia quem criou/modificou/deletou
6. ❌ **Falta inicialização automática**: `CreatedAt` não é inicializado automaticamente
7. ❌ **Falta método para atualizar**: Não há forma fácil de marcar entidade como modificada

---

## Proposta: Estrutura Melhorada

### Princípios de Design

1. ✅ **Propriedades diretas na entidade** - Não usar objetos nullable separados
2. ✅ **Inicialização automática** - `CreatedAt` e `IsDeleted` inicializados no construtor
3. ✅ **Rastreamento de usuário** - Opcional, mas preparado para futuro
4. ✅ **Consistência de tipos** - Usar `DateTime` (UTC) em todo o sistema
5. ✅ **Métodos helper** - Facilitar operações comuns (soft delete, restore, touch)
6. ✅ **Integração com Entity base** - Classes base que estendem `Entity<TKey>`

---

## Estrutura Proposta

### 1. Classe Base: `AuditableEntity<TKey>`

```csharp
namespace Core.Libraries.Domain.Entities.Lifecycle;

/// <summary>
/// Classe base para entidades que precisam de rastreamento de criação e modificação.
/// </summary>
public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditable
    where TKey : IEntityId
{
    /// <summary>
    /// Data e hora de criação da entidade (UTC).
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Data e hora da última modificação da entidade (UTC).
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Identificador do usuário que criou a entidade (opcional).
    /// </summary>
    public string? CreatedBy { get; protected set; }

    /// <summary>
    /// Identificador do usuário que modificou a entidade pela última vez (opcional).
    /// </summary>
    public string? UpdatedBy { get; protected set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected AuditableEntity(TKey id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca a entidade como modificada, atualizando UpdatedAt e UpdatedBy.
    /// </summary>
    public void MarkAsModified(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
```

### 2. Classe Base: `SoftDeletableEntity<TKey>`

```csharp
namespace Core.Libraries.Domain.Entities.Lifecycle;

/// <summary>
/// Classe base para entidades que suportam soft delete.
/// </summary>
public abstract class SoftDeletableEntity<TKey> : Entity<TKey>, ISoftDeletable
    where TKey : IEntityId
{
    /// <summary>
    /// Indica se a entidade foi deletada (soft delete).
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// Data e hora da deleção da entidade (UTC).
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// Identificador do usuário que deletou a entidade (opcional).
    /// </summary>
    public string? DeletedBy { get; protected set; }

    /// <summary>
    /// Marca a entidade como deletada (soft delete).
    /// </summary>
    public void MarkAsDeleted(string? deletedBy = null)
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }
    }

    /// <summary>
    /// Restaura a entidade, removendo a marcação de deletada.
    /// </summary>
    public void Restore()
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

### 3. Classe Combinada: `AuditableSoftDeletableEntity<TKey>`

```csharp
namespace Core.Libraries.Domain.Entities.Lifecycle;

/// <summary>
/// Classe base para entidades que precisam de auditoria e soft delete.
/// </summary>
public abstract class AuditableSoftDeletableEntity<TKey> 
    : AuditableEntity<TKey>, ISoftDeletable
    where TKey : IEntityId
{
    /// <summary>
    /// Indica se a entidade foi deletada (soft delete).
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// Data e hora da deleção da entidade (UTC).
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// Identificador do usuário que deletou a entidade (opcional).
    /// </summary>
    public string? DeletedBy { get; protected set; }

    /// <summary>
    /// Marca a entidade como deletada (soft delete).
    /// </summary>
    public void MarkAsDeleted(string? deletedBy = null)
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
            
            // Atualiza também como modificação
            MarkAsModified(deletedBy);
        }
    }

    /// <summary>
    /// Restaura a entidade, removendo a marcação de deletada.
    /// </summary>
    public void Restore(string? restoredBy = null)
    {
        if (IsDeleted)
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            
            // Marca como modificada ao restaurar
            MarkAsModified(restoredBy);
        }
    }
}
```

### 4. Interfaces Atualizadas

```csharp
namespace Core.Libraries.Domain.Entities.Lifecycle;

/// <summary>
/// Interface para entidades que suportam auditoria de criação e modificação.
/// </summary>
public interface IAuditable
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    string? CreatedBy { get; }
    string? UpdatedBy { get; }
    
    void MarkAsModified(string? updatedBy = null);
}

/// <summary>
/// Interface para entidades que suportam soft delete.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }
    
    void MarkAsDeleted(string? deletedBy = null);
    void Restore(string? restoredBy = null);
}
```

---

## Vantagens da Proposta

### ✅ Propriedades Diretas
- Informações diretamente na entidade
- Não precisa verificar null
- Mais fácil de usar e consultar

### ✅ Inicialização Automática
- `CreatedAt` inicializado automaticamente
- `IsDeleted` inicializado como `false`
- Menos código boilerplate

### ✅ Rastreamento de Usuário
- Preparado para rastrear quem criou/modificou/deletou
- Opcional (nullable), não quebra código existente
- Facilita auditoria futura

### ✅ Consistência
- Usa `DateTime` (UTC) em todo o sistema
- Alinhado com outras partes do código (ex: `Status.CreatedAt`)

### ✅ Flexibilidade
- Classes base separadas (Auditable, SoftDeletable)
- Classe combinada para quem precisa de ambos
- Interfaces para verificação de tipo

### ✅ Métodos Helper
- `MarkAsModified()` - Atualiza automaticamente
- `MarkAsDeleted()` - Soft delete com timestamp
- `Restore()` - Restauração com auditoria

---

## Exemplo de Uso

### Exemplo 1: Entidade com Auditoria

```csharp
public class Product : AuditableEntity<EntityId>
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    protected Product() { }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
        // CreatedAt é inicializado automaticamente
    }

    public void UpdatePrice(decimal newPrice, string? updatedBy = null)
    {
        Price = newPrice;
        MarkAsModified(updatedBy); // Atualiza UpdatedAt e UpdatedBy
    }
}
```

### Exemplo 2: Entidade com Soft Delete

```csharp
public class Customer : SoftDeletableEntity<EntityId>
{
    public string Name { get; private set; } = string.Empty;

    protected Customer() { }

    public Customer(string name)
    {
        Name = name;
    }

    public void Delete(string? deletedBy = null)
    {
        MarkAsDeleted(deletedBy); // Soft delete
    }
}
```

### Exemplo 3: Entidade Completa (Auditoria + Soft Delete)

```csharp
public class Order : AuditableSoftDeletableEntity<EntityId>
{
    public string OrderNumber { get; private set; } = string.Empty;
    public decimal Total { get; private set; }

    protected Order() { }

    public Order(string orderNumber, decimal total)
    {
        OrderNumber = orderNumber;
        Total = total;
        // CreatedAt inicializado automaticamente
    }

    public void Cancel(string? cancelledBy = null)
    {
        MarkAsDeleted(cancelledBy); // Soft delete + atualiza UpdatedAt
    }

    public void RestoreOrder(string? restoredBy = null)
    {
        Restore(restoredBy); // Restaura + atualiza UpdatedAt
    }
}
```

---

## Migração da Estrutura Atual

### Passos para Migração

1. ✅ Atualizar namespaces de `Core.LibrariesDomain.*` para `Core.Libraries.Domain.*`
2. ✅ Criar classes base: `AuditableEntity`, `SoftDeletableEntity`, `AuditableSoftDeletableEntity`
3. ✅ Atualizar interfaces: `IAuditable`, `ISoftDeletable`
4. ✅ Remover classes `AuditInfo` e `DeletionInfo` (ou manter para backward compatibility)
5. ✅ Atualizar documentação

---

## Próximos Passos

1. **Decisão**: Aprovar esta estrutura?
2. **Implementação**: Criar as classes base e interfaces
3. **Migração**: Atualizar namespaces e estruturas existentes
4. **Testes**: Criar testes para as novas classes base

---

## Perguntas para Discussão

1. **Rastreamento de usuário**: Usar `string?` ou criar um tipo específico (ex: `UserId`)?
2. **Backward compatibility**: Manter `AuditInfo` e `DeletionInfo` ou remover?
3. **Validação**: Adicionar validações (ex: não permitir modificar entidade deletada)?
4. **Eventos de domínio**: Disparar eventos quando deletar/restaurar/modificar?

