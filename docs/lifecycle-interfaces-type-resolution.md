# Como as Interfaces Determinam o Tipo de Identificador de Usuário

## Visão Geral

As interfaces `IAuditable` e `ISoftDeletable` agora são genéricas, permitindo que você especifique o tipo de identificador de usuário diretamente na declaração da interface. O tipo é determinado **em tempo de compilação** através do parâmetro genérico.

---

## Como Funciona

### 1. Interface Genérica (Base)

A interface genérica `IAuditable<TUserId>` define o contrato:

```csharp
public interface IAuditable<TUserId>
{
    AuditInfo<TUserId> Audit { get; }
}
```

### 2. Interface Não-Genérica (Alias)

A interface não-genérica `IAuditable` herda da genérica com `string` como padrão:

```csharp
public interface IAuditable : IAuditable<string>
{
    new AuditInfo Audit { get; } // Retorna AuditInfo (que é AuditInfo<string>)
}
```

### 3. Resolução do Tipo

O tipo é determinado **quando você implementa a interface** na sua classe:

```csharp
// Tipo string (padrão)
public class User : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit = new(); // AuditInfo = AuditInfo<string>
    public AuditInfo Audit => _audit;
}

// Tipo Guid (explícito)
public class Order : Entity<EntityId>, IAuditable<Guid>
{
    private readonly AuditInfo<Guid> _audit = new(); // AuditInfo<Guid>
    public AuditInfo<Guid> Audit => _audit;
}

// Tipo customizado (explícito)
public class Document : Entity<EntityId>, IAuditable<UserId>
{
    private readonly AuditInfo<UserId> _audit = new(); // AuditInfo<UserId>
    public AuditInfo<UserId> Audit => _audit;
}
```

---

## Exemplos Práticos

### Exemplo 1: String (Padrão)

```csharp
public class User : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit = new();
    public AuditInfo Audit => _audit;
    
    // IAuditable é equivalente a IAuditable<string>
    // AuditInfo é equivalente a AuditInfo<string>
    // CreatedBy e UpdatedBy são do tipo string?
}
```

**Tipo resolvido:** `string`

---

### Exemplo 2: Guid (Explícito)

```csharp
public class Order : Entity<EntityId>, IAuditable<Guid>
{
    private readonly AuditInfo<Guid> _audit = new();
    public AuditInfo<Guid> Audit => _audit;
    
    // IAuditable<Guid> especifica Guid como tipo
    // AuditInfo<Guid> usa Guid como tipo
    // CreatedBy e UpdatedBy são do tipo Guid?
}
```

**Tipo resolvido:** `Guid`

---

### Exemplo 3: Tipo Customizado (Explícito)

```csharp
public record UserId : EntityId
{
    public UserId(int value) : base(value) { }
}

public class Document : Entity<EntityId>, IAuditable<UserId>
{
    private readonly AuditInfo<UserId> _audit = new();
    public AuditInfo<UserId> Audit => _audit;
    
    // IAuditable<UserId> especifica UserId como tipo
    // AuditInfo<UserId> usa UserId como tipo
    // CreatedBy e UpdatedBy são do tipo UserId?
}
```

**Tipo resolvido:** `UserId`

---

## Fluxo de Resolução

```
1. Classe implementa interface
   ↓
   public class User : Entity<EntityId>, IAuditable<Guid>
   
2. Compilador resolve o tipo genérico
   ↓
   IAuditable<Guid> → TUserId = Guid
   
3. Interface requer AuditInfo<TUserId>
   ↓
   AuditInfo<Guid>
   
4. Propriedades usam o tipo resolvido
   ↓
   CreatedBy: Guid?
   UpdatedBy: Guid?
```

---

## Verificação em Tempo de Compilação

O compilador garante type safety:

```csharp
public class Order : Entity<EntityId>, IAuditable<Guid>
{
    private readonly AuditInfo<Guid> _audit = new();
    public AuditInfo<Guid> Audit => _audit;
    
    public void Create(Guid userId)
    {
        _audit = new AuditInfo<Guid>(userId); // ✅ OK - Guid
        // _audit = new AuditInfo<Guid>("user123"); // ❌ Erro de compilação!
    }
    
    public void Update(string userId)
    {
        // _audit.MarkAsModified(userId); // ❌ Erro de compilação!
        // Espera Guid?, recebeu string
    }
}
```

---

## Compatibilidade com Código Existente

O código existente continua funcionando porque `IAuditable` (não-genérica) é um alias para `IAuditable<string>`:

```csharp
// Código antigo (ainda funciona)
public class User : Entity<EntityId>, IAuditable
{
    private readonly AuditInfo _audit = new();
    public AuditInfo Audit => _audit;
}

// É equivalente a:
public class User : Entity<EntityId>, IAuditable<string>
{
    private readonly AuditInfo<string> _audit = new();
    public AuditInfo<string> Audit => _audit;
}
```

---

## Resumo

| Interface | Tipo de Usuário | Como Determinar |
|-----------|----------------|-----------------|
| `IAuditable` | `string` | Padrão (não-genérica) |
| `IAuditable<Guid>` | `Guid` | Especificar na declaração |
| `IAuditable<int>` | `int` | Especificar na declaração |
| `IAuditable<UserId>` | `UserId` | Especificar na declaração |

---

## Vantagens desta Abordagem

1. ✅ **Type Safety**: O compilador garante que o tipo correto seja usado
2. ✅ **Flexibilidade**: Suporta qualquer tipo de identificador
3. ✅ **Compatibilidade**: Código existente continua funcionando
4. ✅ **Clareza**: O tipo é explícito na declaração da classe
5. ✅ **IntelliSense**: IDE mostra o tipo correto automaticamente

---

## Conclusão

O tipo de identificador de usuário é determinado **em tempo de compilação** através do parâmetro genérico da interface. Quando você declara `IAuditable<Guid>`, o compilador sabe que todas as propriedades relacionadas a usuário devem ser do tipo `Guid?`. Isso garante type safety e previne erros de programação.

