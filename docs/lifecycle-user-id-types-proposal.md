# Proposta: Flexibilidade no Tipo de Identificador de Usuário

## Objetivo

Permitir que `AuditInfo` e `DeletionInfo` suportem diferentes tipos de dados para identificação de usuário, mantendo `string?` como padrão para facilitar o uso comum.

## Opções de Implementação

### Opção 1: Classes Genéricas (Recomendada)

Criar versões genéricas `AuditInfo<TUserId>` e `DeletionInfo<TUserId>`, mantendo versões não-genéricas que usam `string?` como alias.

**Vantagens:**
- ✅ Type safety completo
- ✅ Suporta qualquer tipo (string, Guid, EntityId, UserId customizado)
- ✅ Mantém compatibilidade com código existente (versão não-genérica)
- ✅ Flexível para diferentes necessidades

**Exemplo:**
```csharp
// Uso com string (padrão)
var audit = new AuditInfo("user123");

// Uso com Guid
var auditGuid = new AuditInfo<Guid>(Guid.NewGuid());

// Uso com EntityId customizado
public record UserId : EntityId { ... }
var auditTyped = new AuditInfo<UserId>(new UserId(1));
```

### Opção 2: Interface IUserId

Criar uma interface `IUserId` que tipos de identificador de usuário devem implementar.

**Vantagens:**
- ✅ Type safety
- ✅ Permite métodos comuns (ToString, comparação)

**Desvantagens:**
- ❌ Requer criar tipos que implementem a interface
- ❌ Menos flexível para tipos primitivos

### Opção 3: Múltiplas Sobrecargas

Manter `string?` mas adicionar sobrecargas para tipos comuns (Guid, int).

**Vantagens:**
- ✅ Simples
- ✅ Suporta casos comuns

**Desvantagens:**
- ❌ Limitado a tipos pré-definidos
- ❌ Não suporta tipos customizados facilmente

---

## Recomendação: Opção 1 (Genéricos)

### Estrutura Proposta

```csharp
// Versão genérica (flexível)
public class AuditInfo<TUserId>
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public TUserId? CreatedBy { get; protected set; }
    public TUserId? UpdatedBy { get; protected set; }
    
    public AuditInfo() { ... }
    public AuditInfo(TUserId? createdBy) { ... }
    public void MarkAsModified(TUserId? updatedBy = null) { ... }
}

// Versão não-genérica (alias para string - compatibilidade)
public class AuditInfo : AuditInfo<string>
{
    // Herda tudo de AuditInfo<string>
}
```

### Vantagens desta Abordagem

1. **Type Safety**: Previne erros de tipo
2. **Flexibilidade**: Suporta qualquer tipo
3. **Compatibilidade**: Código existente continua funcionando
4. **Mínimo Código**: Classes concretas não precisam mudar muito

### Exemplos de Uso

#### Com string (padrão - atual)
```csharp
public class User : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit = new();
    public AuditInfo Audit => _audit;
    
    public User(string? createdBy = null)
    {
        if (createdBy != null)
            _audit = new AuditInfo(createdBy);
    }
}
```

#### Com Guid
```csharp
public class Order : Entity<EntityId>, IAuditable<Guid>
{
    private readonly AuditInfo<Guid> _audit = new();
    public AuditInfo<Guid> Audit => _audit;
    
    public Order(Guid? createdBy = null)
    {
        if (createdBy.HasValue)
            _audit = new AuditInfo<Guid>(createdBy.Value);
    }
}
```

#### Com tipo customizado
```csharp
public record UserId : EntityId
{
    public UserId(int value) : base(value) { }
}

public class Document : Entity<EntityId>, IAuditable<UserId>
{
    private readonly AuditInfo<UserId> _audit = new();
    public AuditInfo<UserId> Audit => _audit;
    
    public Document(UserId? createdBy = null)
    {
        if (createdBy != null)
            _audit = new AuditInfo<UserId>(createdBy);
    }
}
```

---

## Impacto nas Interfaces

As interfaces também precisariam ser genéricas:

```csharp
// Versão genérica
public interface IAuditable<TUserId>
{
    AuditInfo<TUserId> Audit { get; }
}

// Versão não-genérica (alias)
public interface IAuditable : IAuditable<string>
{
    // Herda tudo de IAuditable<string>
}
```

---

## Decisão Necessária

Qual abordagem você prefere?

1. **Genéricos** (mais flexível, mas requer mudanças nas interfaces)
2. **Manter string** (mais simples, mas menos type-safe)
3. **Híbrido** (genéricos opcionais, string como padrão)

