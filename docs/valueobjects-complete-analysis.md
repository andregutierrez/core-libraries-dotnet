# Análise Completa: Classes ValueObjects

## Estrutura Atual

### Classes Identificadas

1. **Enumeration.cs** - Classe base para enumerações ricas
2. **Identities/**
   - `EmailAddress.cs` - Endereço de email
   - `Cpf.cs` - CPF brasileiro
   - `Cnpj.cs` - CNPJ brasileiro
3. **Moneys/**
   - `Currency.cs` - Moeda (ISO 4217)
   - `Money.cs` - Valor monetário
   - `ExchangeRate.cs` - Taxa de câmbio
   - `MoneyRange.cs` - Intervalo monetário
   - `Percentage.cs` - Porcentagem
4. **Measurements/**
   - `Length.cs` - Comprimento (metros)
   - `Weight.cs` - Peso (quilogramas)
   - `Volume.cs` - Volume (litros)
   - `Temperature.cs` - Temperatura (kelvin)
5. **Periods/** (já analisado e corrigido)

---

## Problemas Identificados

### 1. ❌ Namespaces Inconsistentes

**Problema:** A maioria das classes usa `Core.LibrariesDomain.ValueObjects.*` ao invés de `Core.Libraries.Domain.ValueObjects.*`.

**Classes Afetadas:**
- ✅ `Periods/*` - **CORRIGIDO** (usam `Core.Libraries.Domain.ValueObjects.Periods`)
- ❌ `Enumeration.cs` - `Core.LibrariesDomain.ValueObjects`
- ❌ `Identities/*` - `Core.LibrariesDomain.ValueObjects.Identities`
- ❌ `Moneys/*` - `Core.LibrariesDomain.ValueObjects.Moneys`
- ❌ `Measurements/*` - `Core.LibrariesDomain.ValueObjects.Measurements`

**Impacto:** Inconsistência com o padrão do projeto.

---

### 2. ❌ EmailAddress Sem Namespace

**Problema:** `EmailAddress.cs` não tem declaração de namespace.

**Código atual:**
```csharp
using System;
using System.Net.Mail;

public record EmailAddress  // ❌ Sem namespace!
{
    // ...
}
```

**Código esperado:**
```csharp
namespace Core.Libraries.Domain.ValueObjects.Identities;

public record EmailAddress
{
    // ...
}
```

**Impacto:** Classe no namespace global, pode causar conflitos.

---

### 3. ❌ Enumeration Falta GetHashCode()

**Problema:** `Enumeration` sobrescreve `Equals()` mas não `GetHashCode()`, violando o contrato.

**Código atual:**
```csharp
public override bool Equals(object? obj) { /* ... */ }
// ❌ Falta GetHashCode()
```

**Código esperado:**
```csharp
public override int GetHashCode() => Id.GetHashCode();
```

**Impacto:** 
- Warning CS0659
- Problemas em coleções (Dictionary, HashSet)
- Violação de contrato Object.Equals/GetHashCode

---

### 4. ❌ Enumeration Falta Null-Safety

**Problema:** `CompareTo()` pode lançar `NullReferenceException` se `obj` for `null`.

**Código atual:**
```csharp
public int CompareTo(object? obj) => Id.CompareTo(((Enumeration)obj).Id);
// ❌ Não verifica null antes do cast
```

**Código esperado:**
```csharp
public int CompareTo(object? obj)
{
    if (obj is null) return 1;
    if (obj is not Enumeration other) 
        throw new ArgumentException("Object must be of type Enumeration.", nameof(obj));
    return Id.CompareTo(other.Id);
}
```

**Impacto:** Possível `NullReferenceException` ou `InvalidCastException`.

---

### 5. ❌ Inconsistência de Estilo: Chaves

**Problema:** Algumas classes usam chaves na mesma linha do namespace, outras não.

**Padrão inconsistente:**
```csharp
// Identities/Cpf.cs - com chaves
namespace Core.LibrariesDomain.ValueObjects.Identities
{
    public record Cpf { }
}

// Moneys/Money.cs - sem chaves (file-scoped)
namespace Core.LibrariesDomain.ValueObjects.Moneys;

public record Money { }
```

**Recomendação:** Padronizar em **file-scoped namespaces** (sem chaves), que é o padrão moderno do C#.

---

### 6. ❌ Falta de Construtor Protegido para ORM

**Problema:** Alguns `record` não têm construtor protegido sem parâmetros para EF Core.

**Classes afetadas:**
- ❌ `EmailAddress` - não tem construtor protegido
- ❌ `Cpf` - não tem construtor protegido
- ❌ `Cnpj` - não tem construtor protegido
- ❌ `Currency` - não tem construtor protegido
- ❌ `Money` - não tem construtor protegido
- ❌ `ExchangeRate` - não tem construtor protegido
- ❌ `MoneyRange` - não tem construtor protegido
- ❌ `Percentage` - não tem construtor protegido
- ❌ `Length` - não tem construtor protegido
- ❌ `Weight` - não tem construtor protegido
- ❌ `Volume` - não tem construtor protegido
- ❌ `Temperature` - não tem construtor protegido

**Exemplo esperado:**
```csharp
public record EmailAddress
{
    public string Value { get; }
    
    // Para EF Core e desserialização
    protected EmailAddress() => Value = default!;
    
    public EmailAddress(string email) { /* ... */ }
}
```

**Impacto:** Problemas com EF Core e desserialização.

---

### 7. ❌ Documentação em Inglês

**Problema:** Todas as classes têm documentação XML em inglês, enquanto `Periods` foi traduzida para português.

**Impacto:** Inconsistência de documentação.

**Recomendação:** Traduzir para português ou manter tudo em inglês (escolher um padrão).

---

### 8. ❌ Inconsistência em Métodos de Operação

**Problema:** Algumas classes têm métodos `Add`/`Subtract`, outras não.

**Classes com operações:**
- ✅ `Money` - `Add`, `Subtract`, `Multiply`, `Divide`
- ✅ `Length` - `Add`, `Subtract`, `Multiply`
- ✅ `Weight` - `Add`, `Subtract`, `Multiply`
- ✅ `Volume` - `Add`, `Subtract`, `Multiply`
- ❌ `Temperature` - **não tem** `Add`, `Subtract` (faz sentido? Temperatura não se soma)

**Análise:**
- `Temperature` não deveria ter `Add`/`Subtract` (faz sentido físico)
- Outras classes estão consistentes

---

### 9. ❌ EmailAddress: Falta Validação de Null

**Problema:** `EmailAddress` não valida se `email` é `null` antes de usar.

**Código atual:**
```csharp
public EmailAddress(string email)
{
    if (string.IsNullOrWhiteSpace(email))  // ✅ Valida vazio
        throw new ArgumentException("Email must not be empty.", nameof(email));
    
    try
    {
        var addr = new MailAddress(email);  // ❌ Pode lançar se null
        // ...
    }
}
```

**Observação:** Na verdade está correto, pois `string.IsNullOrWhiteSpace` já valida `null`. Mas poderia ser mais explícito.

---

### 10. ❌ MoneyRange: Falta Documentação XML

**Problema:** `MoneyRange` tem propriedades sem documentação XML.

**Código atual:**
```csharp
public record MoneyRange
{
    public Money Min { get; }  // ❌ Sem <summary>
    public Money Max { get; }  // ❌ Sem <summary>
}
```

---

### 11. ❌ ExchangeRate: Falta Using

**Problema:** `ExchangeRate.cs` usa `ArgumentNullException` e `InvalidOperationException` mas não tem `using System;`.

**Código atual:**
```csharp
namespace Core.LibrariesDomain.ValueObjects.Moneys;

// ❌ Falta using System;

public record ExchangeRate
{
    public ExchangeRate(Currency from, Currency to, decimal rate)
    {
        From = from ?? throw new ArgumentNullException(nameof(from));  // ❌ Sem using
        // ...
    }
}
```

**Observação:** Na verdade, `ArgumentNullException` e `InvalidOperationException` estão no namespace `System`, então precisa de `using System;` ou usar `System.ArgumentNullException`.

---

### 12. ❌ Measurements: Falta Using System

**Problema:** Classes em `Measurements` usam `ArgumentException` e `InvalidOperationException` mas não têm `using System;`.

**Classes afetadas:**
- `Length.cs`
- `Weight.cs`
- `Volume.cs`
- `Temperature.cs`

---

### 13. ⚠️ Inconsistência em ToString()

**Análise:**
- ✅ `Money`: `"USD 123.45"` - formatado
- ✅ `Currency`: `"USD"` - código
- ✅ `ExchangeRate`: `"1 USD = 1.234567 EUR"` - formatado
- ✅ `MoneyRange`: `"USD 10.00 – USD 20.00"` - formatado
- ✅ `Percentage`: `"25.00 %"` - formatado
- ✅ `Length`: `"10.00 m"` - formatado
- ✅ `Weight`: `"5.00 kg"` - formatado
- ✅ `Volume`: `"2.50 L"` - formatado
- ✅ `Temperature`: `"25.00 °C"` - formatado
- ✅ `EmailAddress`: `"user@example.com"` - valor
- ✅ `Cpf`: `"123.456.789-00"` - formatado
- ✅ `Cnpj`: `"12.345.678/0001-90"` - formatado

**Conclusão:** ToString() está consistente e bem formatado em todas as classes.

---

## Resumo de Problemas por Prioridade

### 🔴 Crítico (Corrigir Imediatamente)

1. **Namespaces incorretos** - Todas as classes (exceto Periods)
2. **EmailAddress sem namespace** - Classe no namespace global
3. **Enumeration.GetHashCode() faltando** - Violação de contrato
4. **Enumeration.CompareTo() sem null-safety** - Possível exceção

### 🟡 Importante (Corrigir em Breve)

5. **Falta construtor protegido para ORM** - Todas as classes record
6. **Falta using System** - ExchangeRate e Measurements
7. **Inconsistência de estilo (chaves)** - Padronizar file-scoped namespaces

### 🟢 Melhorias (Opcional)

8. **Documentação em inglês** - Traduzir ou padronizar
9. **MoneyRange sem documentação XML** - Adicionar summaries
10. **EmailAddress validação** - Tornar mais explícita

---

## Propostas de Correção

### Opção 1: Correção Completa (Recomendada)

1. ✅ Corrigir todos os namespaces
2. ✅ Adicionar namespace em EmailAddress
3. ✅ Implementar GetHashCode() em Enumeration
4. ✅ Corrigir CompareTo() em Enumeration
5. ✅ Adicionar construtores protegidos em todos os records
6. ✅ Adicionar using System onde necessário
7. ✅ Padronizar file-scoped namespaces
8. ✅ Traduzir documentação para português (ou manter inglês)

### Opção 2: Correção Mínima

1. ✅ Corrigir namespaces
2. ✅ Adicionar namespace em EmailAddress
3. ✅ Corrigir Enumeration (GetHashCode + CompareTo)
4. ✅ Adicionar construtores protegidos

---

## Comparação: Antes vs Depois

### Antes (Problemas)

```csharp
// Namespace errado
namespace Core.LibrariesDomain.ValueObjects.Moneys;

// Sem construtor protegido
public record Money
{
    public decimal Amount { get; }
    public Money(decimal amount, Currency currency) { }
}

// Enumeration sem GetHashCode
public abstract class Enumeration
{
    public override bool Equals(object? obj) { }
    // ❌ Falta GetHashCode()
}
```

### Depois (Proposta)

```csharp
// Namespace correto
namespace Core.Libraries.Domain.ValueObjects.Moneys;

// Com construtor protegido
public record Money
{
    public decimal Amount { get; }
    
    protected Money() => Amount = default;  // ✅ Para ORM
    
    public Money(decimal amount, Currency currency) { }
}

// Enumeration completo
public abstract class Enumeration
{
    public override bool Equals(object? obj) { }
    public override int GetHashCode() => Id.GetHashCode();  // ✅
    public int CompareTo(object? obj) { /* com null-safety */ }  // ✅
}
```

---

## Recomendação Final

**Implementar Opção 1 (Correção Completa)** porque:
1. ✅ Resolve todos os problemas críticos
2. ✅ Padroniza o código
3. ✅ Melhora manutenibilidade
4. ✅ Previne problemas futuros

**Prioridades:**
1. **Alta**: Namespaces, EmailAddress namespace, Enumeration.GetHashCode/CompareTo
2. **Média**: Construtores protegidos, using System, file-scoped namespaces
3. **Baixa**: Tradução de documentação, melhorias opcionais

---

## Próximos Passos

1. Aprovar proposta?
2. Implementar todas as correções de uma vez?
3. Implementar incrementalmente (prioridades)?

