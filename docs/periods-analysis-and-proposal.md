# Análise e Proposta: Classes de Períodos (Periods)

## Estrutura Atual

### Classes Identificadas

1. **Period.cs** - Classe base abstrata (`record`)
2. **YearPeriod.cs** - Período anual
3. **MonthPeriod.cs** - Período mensal
4. **WeekPeriod.cs** - Período semanal (ISO-8601)
5. **FortnightPeriod.cs** - Período quinzenal (24 quinzenas/ano)
6. **BimesterPeriod.cs** - Período bimestral (6 bimestres/ano)
7. **TrimesterPeriod.cs** - Período trimestral (4 trimestres/ano)
8. **QuadrimesterPeriod.cs** - Período quadrimestral (3 quadrimestres/ano)
9. **CustomPeriod.cs** - Período customizado

---

## Problemas Identificados

### 1. ❌ Namespaces Inconsistentes

**Problema:** Todas as classes usam `Core.LibrariesDomain.ValueObjects.Periods` ao invés de `Core.Libraries.Domain.ValueObjects.Periods`.

**Impacto:** Inconsistência com o padrão do projeto.

---

### 2. ❌ Period Não Herda de ValueObject

**Problema:** `Period` é um `record` abstrato, mas não herda de `ValueObject<T>`, que existe no projeto.

**Impacto:** 
- Perde funcionalidades de comparação de objetos de valor
- Inconsistente com outros Value Objects do projeto

**Código atual:**
```csharp
public abstract record Period
{
    // Não herda de ValueObject
}
```

**Código esperado:**
```csharp
public abstract record Period : ValueObject<Period>
{
    // Herda de ValueObject
}
```

**OU** (se `Period` não deve herdar de `ValueObject`):
- Remover `ValueObject<T>` se não for usado
- Ou documentar por que `Period` não herda

---

### 3. ❌ Validação de Year Inconsistente

**Problema:** 
- `YearPeriod` valida `year` entre 1-9999
- Outras classes (`MonthPeriod`, `WeekPeriod`, etc.) não validam `year`

**Impacto:** Possível erro em runtime com anos inválidos.

**Exemplo:**
```csharp
// YearPeriod - valida
public YearPeriod(int year) // Valida 1-9999

// MonthPeriod - não valida
public MonthPeriod(int month, int year) // Não valida year!
```

---

### 4. ❌ Inconsistência em Métodos Estáticos

**Problema:** Todas têm `FromDate()`, mas algumas têm `Create()` também.

**Análise:**
- ✅ `YearPeriod.FromDate()`
- ✅ `MonthPeriod.FromDate()`
- ✅ `WeekPeriod.FromDate()`
- ✅ `FortnightPeriod.FromDate()`
- ✅ `BimesterPeriod.FromDate()`
- ✅ `TrimesterPeriod.FromDate()`
- ✅ `QuadrimesterPeriod.FromDate()`
- ✅ `CustomPeriod.Create()` (diferente!)

**Impacto:** API inconsistente.

---

### 5. ❌ Formato de ToString() Inconsistente

**Problema:** Formatos diferentes entre classes.

**Análise:**
- `Period` (base): `"yyyy-MM-dd – yyyy-MM-dd"`
- `YearPeriod`: `"2025"` (apenas ano)
- `MonthPeriod`: `"MM/yyyy"` (ex: "03/2025")
- `WeekPeriod`: `"W03/2025"`
- `FortnightPeriod`: `"F01/2025"`
- `BimesterPeriod`: `"B2/2025"`
- `TrimesterPeriod`: `"Q2/2025"`
- `QuadrimesterPeriod`: `"Q1/2025"` (⚠️ conflito com Trimester!)
- `CustomPeriod`: `"{Label}: yyyy-MM-dd – yyyy-MM-dd"`

**Problemas:**
- `TrimesterPeriod` e `QuadrimesterPeriod` usam o mesmo prefixo `"Q"`
- Formatos não seguem padrão único

---

### 6. ❌ Propriedades Year Sem Validação

**Problema:** Classes que têm propriedade `Year` não validam se está no range válido (1-9999).

**Exemplo:**
```csharp
// MonthPeriod - Year não é validado
public MonthPeriod(int month, int year)
{
    if (month < 1 || month > 12) // ✅ Valida month
        throw new ArgumentException(...);
    // ❌ Não valida year!
    Month = month;
    Year = year;
}
```

---

### 7. ❌ Falta de Métodos Comuns

**Problema:** Algumas funcionalidades comuns poderiam ser extraídas para a classe base.

**Funcionalidades comuns:**
- `FromDate()` - presente em todas (exceto `CustomPeriod`)
- `Previous()` - presente em todas (exceto `CustomPeriod`)
- `Next()` - presente em todas (exceto `CustomPeriod`)

**Proposta:** Criar métodos abstratos ou virtuais na classe base.

---

### 8. ❌ CustomPeriod Não Tem Previous/Next

**Problema:** `CustomPeriod` não implementa `Previous()` e `Next()`.

**Impacto:** API inconsistente - outras classes têm, mas `CustomPeriod` não.

**Solução:** 
- Implementar `Previous()` e `Next()` baseado em duração
- OU documentar por que não tem
- OU criar interface separada

---

### 9. ❌ Falta de Métodos de Utilidade

**Problemas:**
- Não há método para calcular duração do período
- Não há método para verificar sobreposição entre períodos
- Não há método para verificar se períodos são consecutivos
- Não há método para obter todos os períodos entre duas datas

---

## Proposta de Melhorias

### Opção 1: Refatoração Completa (Recomendada)

#### 1.1. Atualizar Namespaces

```csharp
// Antes
namespace Core.LibrariesDomain.ValueObjects.Periods;

// Depois
namespace Core.Libraries.Domain.ValueObjects.Periods;
```

#### 1.2. Adicionar Validação de Year Consistente

```csharp
public abstract record Period
{
    private const int MinYear = 1;
    private const int MaxYear = 9999;
    
    protected static void ValidateYear(int year)
    {
        if (year < MinYear || year > MaxYear)
            throw new ArgumentException(
                $"Year must be between {MinYear} and {MaxYear}.", 
                nameof(year));
    }
}
```

#### 1.3. Padronizar ToString()

```csharp
// Proposta de formatos padronizados:
// YearPeriod: "2025"
// MonthPeriod: "2025-03"
// WeekPeriod: "2025-W03"
// FortnightPeriod: "2025-F01"
// BimesterPeriod: "2025-B02"
// TrimesterPeriod: "2025-T02" (mudar de Q para T)
// QuadrimesterPeriod: "2025-Q01"
// CustomPeriod: "{Label} (2025-01-01 – 2025-12-31)"
```

#### 1.4. Adicionar Métodos Comuns na Base

```csharp
public abstract record Period
{
    // Métodos comuns
    public abstract Period? Previous();
    public abstract Period? Next();
    public abstract Period FromDate(DateOnly date);
    
    // Métodos de utilidade
    public int Days => (End.ToDateTime(TimeOnly.MinValue) - Start.ToDateTime(TimeOnly.MinValue)).Days + 1;
    public bool OverlapsWith(Period other) => Start <= other.End && End >= other.Start;
    public bool IsConsecutiveWith(Period other) => End.AddDays(1) == other.Start || other.End.AddDays(1) == Start;
}
```

#### 1.5. Resolver Conflito de Prefixo Q

```csharp
// TrimesterPeriod: usar "T" ao invés de "Q"
public override string ToString() => $"T{Trimester}/{Year}";

// QuadrimesterPeriod: manter "Q"
public override string ToString() => $"Q{Quadrimester}/{Year}";
```

---

### Opção 2: Melhorias Incrementais

#### 2.1. Apenas Corrigir Namespaces
#### 2.2. Adicionar Validação de Year
#### 2.3. Padronizar ToString()
#### 2.4. Adicionar métodos de utilidade

---

## Comparação: Antes vs Depois

### Antes (Problemas)

```csharp
// Namespace errado
namespace Core.LibrariesDomain.ValueObjects.Periods;

// Sem validação de year
public MonthPeriod(int month, int year) { ... }

// ToString inconsistente
// Trimester: "Q2/2025"
// Quadrimester: "Q1/2025" (conflito!)

// Sem métodos comuns
// Cada classe implementa tudo do zero
```

### Depois (Proposta)

```csharp
// Namespace correto
namespace Core.Libraries.Domain.ValueObjects.Periods;

// Com validação de year
public MonthPeriod(int month, int year)
{
    ValidateYear(year); // ✅
    // ...
}

// ToString padronizado
// Trimester: "2025-T02"
// Quadrimester: "2025-Q01"

// Métodos comuns na base
public abstract Period? Previous();
public abstract Period? Next();
```

---

## Recomendação Final

**Implementar Opção 1 (Refatoração Completa)** porque:
1. ✅ Resolve todos os problemas de uma vez
2. ✅ Cria base sólida para extensões futuras
3. ✅ Melhora consistência e manutenibilidade
4. ✅ Adiciona funcionalidades úteis

**Prioridades:**
1. **Alta**: Corrigir namespaces
2. **Alta**: Adicionar validação de year
3. **Média**: Padronizar ToString()
4. **Média**: Resolver conflito de prefixo Q
5. **Baixa**: Adicionar métodos de utilidade

---

## Próximos Passos

1. Aprovar proposta?
2. Implementar todas as melhorias de uma vez?
3. Implementar incrementalmente (prioridades)?

