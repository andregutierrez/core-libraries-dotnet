# Explicação: Palavra-chave `new` em C#

## O que é `new`?

A palavra-chave `new` em C# tem **dois usos principais**:

### 1. Operador `new` (criar instâncias)
```csharp
var obj = new MyClass(); // Cria uma nova instância
```

### 2. Modificador `new` (esconder membros herdados)
```csharp
public class Base
{
    public void Method() { }
}

public class Derived : Base
{
    public new void Method() { } // Esconde o Method() da classe base
}
```

---

## `new` em Interfaces

Quando usado em interfaces, o `new` indica que você está **escondendo (hiding)** um membro de uma interface base.

### Exemplo Correto (com herança):

```csharp
public interface IBase
{
    string Property { get; }
}

public interface IDerived : IBase
{
    new int Property { get; } // Esconde Property da interface base
}
```

### Exemplo no seu código:

```csharp
public interface ISoftDeletable
{
    DeletionInfo Deletion { get; }
}

public interface ISoftDeletable<TUserId>
{
    new DeletionInfo<TUserId> Deletion { get; } // ⚠️ PROBLEMA!
}
```

---

## Problema no Código Atual

No seu código, `ISoftDeletable<TUserId>` **NÃO herda** de `ISoftDeletable`, então o `new` **não faz sentido** e pode causar confusão.

### Situação Atual (Incorreta):
```csharp
// Duas interfaces independentes
public interface ISoftDeletable { ... }
public interface ISoftDeletable<TUserId> { ... } // Não herda de ISoftDeletable
```

### Solução 1: Remover o `new` (se não há herança)
```csharp
public interface ISoftDeletable<TUserId>
{
    DeletionInfo<TUserId> Deletion { get; } // Sem 'new'
}
```

### Solução 2: Criar relação de herança (recomendado)
```csharp
// Interface genérica (base)
public interface ISoftDeletable<TUserId>
{
    DeletionInfo<TUserId> Deletion { get; }
}

// Interface não-genérica (herda da genérica com string)
public interface ISoftDeletable : ISoftDeletable<string>
{
    new DeletionInfo Deletion { get; } // ✅ Agora o 'new' faz sentido!
}
```

---

## Por que usar `new`?

O `new` é necessário quando:
1. Você herda de uma interface/classe base
2. Você quer **esconder** um membro da base
3. Você quer indicar **explicitamente** que está escondendo (evita warnings do compilador)

---

## Recomendação

Para o seu código, você tem duas opções:

### Opção A: Remover `new` (se interfaces são independentes)
```csharp
public interface ISoftDeletable<TUserId>
{
    DeletionInfo<TUserId> Deletion { get; } // Sem 'new'
}
```

### Opção B: Criar herança (melhor para compatibilidade)
```csharp
public interface ISoftDeletable<TUserId>
{
    DeletionInfo<TUserId> Deletion { get; }
}

public interface ISoftDeletable : ISoftDeletable<string>
{
    new DeletionInfo Deletion { get; } // Esconde DeletionInfo<TUserId>
}
```

---

## Resumo

- **`new`** = "Estou escondendo um membro herdado"
- **Sem herança** = `new` não faz sentido
- **Com herança** = `new` é necessário para esconder membro da base

