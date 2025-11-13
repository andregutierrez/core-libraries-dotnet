# Proposta: Otimização com Herança no Lifecycle

## Análise Atual

### Padrões Identificados

1. **Rastreamento de Usuário com Timestamp**:
   - `AuditInfo<TUserId>`: `CreatedBy`, `UpdatedBy` + `CreatedAt`, `UpdatedAt`
   - `DeletionInfo<TUserId>`: `DeletedBy` + `DeletedAt`
   - Ambos têm lógica similar: atualizar timestamp e opcionalmente o usuário

2. **Lógica de Atualização Condicional**:
   ```csharp
   if (userId != null)
   {
       UserIdProperty = userId;
   }
   ```

3. **Uso de DateTime.UtcNow**: Ambas usam para timestamps

---

## Proposta de Otimização

### Opção 1: Classe Base para Rastreamento de Usuário (Recomendada)

Criar uma classe base abstrata `UserTrackingInfo<TUserId>` que encapsula o padrão comum:

```csharp
public abstract class UserTrackingInfo<TUserId>
{
    public TUserId? UserId { get; protected set; }
    public DateTime? Timestamp { get; protected set; }
    
    protected void UpdateTracking(TUserId? userId = default)
    {
        Timestamp = DateTime.UtcNow;
        if (userId != null)
        {
            UserId = userId;
        }
    }
    
    protected void ClearTracking()
    {
        Timestamp = null;
        UserId = default;
    }
}
```

**Vantagens:**
- ✅ Reduz duplicação de código
- ✅ Centraliza lógica de rastreamento
- ✅ Facilita manutenção
- ✅ Permite extensão futura

**Desvantagens:**
- ⚠️ `AuditInfo` precisa de 2 rastreamentos (Created e Updated)
- ⚠️ Pode adicionar complexidade desnecessária

---

### Opção 2: Composição com Helper Interno

Criar uma classe helper interna que encapsula a lógica:

```csharp
public class AuditInfo<TUserId>
{
    private readonly UserTracking _created;
    private readonly UserTracking _updated;
    
    private class UserTracking
    {
        public TUserId? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        
        public void Update(TUserId? userId = default)
        {
            Timestamp = DateTime.UtcNow;
            if (userId != null) UserId = userId;
        }
    }
}
```

**Vantagens:**
- ✅ Encapsula lógica sem herança
- ✅ Flexível para múltiplos rastreamentos
- ✅ Não expõe classe base desnecessariamente

**Desvantagens:**
- ⚠️ Ainda tem alguma duplicação
- ⚠️ Classes helper podem ser menos claras

---

### Opção 3: Métodos Estáticos Helper

Criar métodos estáticos helper para reduzir duplicação:

```csharp
internal static class UserTrackingHelper
{
    public static void UpdateTracking<TUserId>(
        ref TUserId? userIdProperty,
        ref DateTime? timestampProperty,
        TUserId? userId = default)
    {
        timestampProperty = DateTime.UtcNow;
        if (userId != null)
        {
            userIdProperty = userId;
        }
    }
}
```

**Vantagens:**
- ✅ Simples
- ✅ Sem herança
- ✅ Reutilizável

**Desvantagens:**
- ⚠️ Usa `ref` que pode ser confuso
- ⚠️ Não é orientado a objetos

---

## Recomendação: Opção 1 (Classe Base) com Adaptação

Para `AuditInfo`, que precisa de 2 rastreamentos, podemos usar composição:

```csharp
public class AuditInfo<TUserId>
{
    private readonly UserTrackingInfo<TUserId> _created;
    private readonly UserTrackingInfo<TUserId> _updated;
    
    public DateTime CreatedAt => _created.Timestamp ?? DateTime.UtcNow;
    public TUserId? CreatedBy => _created.UserId;
    public DateTime? UpdatedAt => _updated.Timestamp;
    public TUserId? UpdatedBy => _updated.UserId;
    
    public AuditInfo(TUserId? createdBy = default)
    {
        _created = new UserTrackingInfo<TUserId>();
        _created.UpdateTracking(createdBy);
        _updated = new UserTrackingInfo<TUserId>();
    }
    
    public void MarkAsModified(TUserId? updatedBy = default)
    {
        _updated.UpdateTracking(updatedBy);
    }
}

public class DeletionInfo<TUserId> : UserTrackingInfo<TUserId>
{
    public bool IsDeleted { get; protected set; }
    
    public DateTime? DeletedAt => Timestamp;
    public TUserId? DeletedBy => UserId;
    
    public void MarkAsDeleted(TUserId? deletedBy = default)
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            UpdateTracking(deletedBy);
        }
    }
    
    public void Restore()
    {
        if (IsDeleted)
        {
            IsDeleted = false;
            ClearTracking();
        }
    }
}
```

---

## Decisão

Qual abordagem você prefere?

1. **Classe Base** (mais OOP, mas pode ser complexa para AuditInfo)
2. **Composição** (mais flexível, menos herança)
3. **Manter como está** (simples, mas com duplicação)

