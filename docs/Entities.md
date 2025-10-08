# Documentação - Pasta Entities

A pasta `Entities` contém as classes fundamentais para o modelo de domínio, implementando padrões do Domain-Driven Design (DDD) e fornecendo uma base sólida para entidades e agregados.

## Visão Geral

Esta pasta implementa os conceitos centrais do domínio, incluindo:
- **Entidades** com identificação única
- **Agregados** com gerenciamento de eventos de domínio
- **Identificadores** fortemente tipados
- **Coleções** especializadas para entidades

## Estrutura de Arquivos

```
src/Core.Libraries.Domain/Entities/
├── Entity.cs          # Classe base para entidades
├── Aggregate.cs       # Classe base para agregados
├── EntityId.cs        # Identificador genérico fortemente tipado
├── IEntityId.cs       # Interface marcadora para identificadores
└── EntityList.cs      # Coleção especializada para entidades
```

## Classes e Interfaces

### 1. Entity<TKey>

**Arquivo:** `Entity.cs`

Classe base abstrata para todas as entidades de domínio no sistema. Fornece funcionalidade comum para identificação, comparação e igualdade de entidades.

#### Características:
- **Identificação única**: Cada entidade possui um identificador único
- **Comparação**: Implementa `IComparable` para ordenação
- **Igualdade**: Sobrescreve `Equals` e `GetHashCode`
- **Estado transitório**: Detecta se a entidade ainda não foi persistida

#### Construtores:
```csharp
protected Entity()                   // Para ORM/serialização
protected Entity(TKey id)            // Com ID específico
```

#### Propriedades:
- `Id`: Identificador único da entidade
- `IsTransient`: Indica se a entidade é transitória (não persistida)

#### Métodos:
- `CompareTo()`: Compara entidades para ordenação
- `Equals()`: Verifica igualdade entre entidades
- `GetHashCode()`: Retorna código hash
- `ToString()`: Representação em string

### 2. Aggregate<TEntityId>

**Arquivo:** `Aggregate.cs`

Classe base abstrata para agregados de domínio. Um agregado é um cluster de objetos de domínio que são tratados como uma unidade para fins de mudança de dados.

#### Características:
- **Herda de Entity**: Possui identificação única
- **Eventos de Domínio**: Gerencia eventos através de `IHasDomainEvents`
- **Padrão Aggregate Root**: Implementa o padrão DDD

#### Construtores:
```csharp
protected Aggregate()                    // Para ORM/serialização
protected Aggregate(TEntityId id)       // Com ID específico
```

#### Métodos:
- `RegisterEvent()`: Registra eventos de domínio
- `Events`: Acessa a lista de eventos (implementação de interface)

### 3. EntityId<T>

**Arquivo:** `EntityId.cs`

Objeto de valor genérico que representa um identificador de entidade fortemente tipado.

#### Características:
- **Fortemente tipado**: Usa generics para type safety
- **Imutável**: Implementa `record` para imutabilidade
- **Comparável**: Implementa `IComparable` e `IEquatable`
- **Serializável**: Suporta serialização/deserialização

#### Construtores:
```csharp
public EntityId(T value)        // Com valor específico
protected EntityId()            // Para serialização/ORM
```

#### Propriedades:
- `Value`: Valor subjacente do identificador

#### Exemplo de uso:
```csharp
var usuarioId = new EntityId<int>(123);
var pedidoId = new EntityId<Guid>(Guid.NewGuid());
```

### 4. IEntityId

**Arquivo:** `IEntityId.cs`

Interface marcadora para identificadores de entidade. Todos os identificadores de entidade devem implementar esta interface para garantir que possam ser comparados.

#### Características:
- **Interface mínima**: Herda apenas de `IComparable`
- **Contrato**: Garante que IDs podem ser comparados
- **Marcadora**: Usada para constraint de generics

### 5. EntityList<TEntity>

**Arquivo:** `EntityList.cs`

Classe base abstrata para coleções de entidades de domínio. Fornece uma coleção fortemente tipada que implementa múltiplas interfaces de coleção e suporta consultas LINQ.

#### Características:
- **Múltiplas interfaces**: `ICollection`, `IReadOnlyCollection`, `IQueryable`
- **LINQ support**: Suporte completo a consultas LINQ
- **Type safety**: Fortemente tipada para entidades específicas
- **Flexível**: Pode ser estendida para comportamentos específicos

#### Propriedades:
- `Count`: Número de entidades na coleção
- `ElementType`: Tipo dos elementos (IQueryable)
- `Expression`: Árvore de expressão (IQueryable)
- `Provider`: Provedor de consulta (IQueryable)

#### Métodos:
- `AsReadOnly()`: Retorna wrapper somente leitura
- `AsQueryable()`: Retorna como fonte consultável para LINQ

#### Implementações de Interface:
- **ICollection**: `Add()`, `Remove()`, `Clear()`, `Contains()`, `CopyTo()`
- **IEnumerable**: `GetEnumerator()`
- **IQueryable**: Suporte completo a LINQ

## Padrões de Uso

### Convenções de Nomenclatura

#### IDs Fortemente Tipados
- **Padrão**: `nome + Id` (ex: `usuarioId`, `pedidoId`, `produtoId`)
- **Exemplo**: `var usuarioId = new EntityId<int>(123);`
- **Consistência**: Sempre use o sufixo `Id` para variáveis de identificadores

### Criando uma Entidade

```csharp
public class Usuario : Entity<EntityId<int>>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    
    public Usuario(EntityId<int> usuarioId, string nome, string email) 
        : base(usuarioId)
    {
        Nome = nome;
        Email = email;
    }
}
```

### Criando um Agregado

```csharp
public class Pedido : Aggregate<EntityId<Guid>>
{
    public string NomeCliente { get; set; }
    public decimal Total { get; set; }
    
    public Pedido(EntityId<Guid> pedidoId, string nomeCliente) 
        : base(pedidoId)
    {
        NomeCliente = nomeCliente;
    }
    
    public void AdicionarItem(string nomeProduto, decimal preco)
    {
        // Lógica de negócio
        RegisterEvent(new ItemPedidoAdicionadoEvent(Id, nomeProduto, preco));
    }
}
```

### Criando uma Coleção de Entidades

```csharp
public class ListaUsuarios : EntityList<Usuario>
{
    public Usuario? BuscarPorEmail(string email)
    {
        return this.FirstOrDefault(u => u.Email == email);
    }
    
    public IEnumerable<Usuario> ObterUsuariosAtivos()
    {
        return this.Where(u => u.Ativo);
    }
}
```

## Benefícios

### 1. **Type Safety**
- Identificadores fortemente tipados previnem erros de tipo
- Generics garantem consistência em tempo de compilação

### 2. **Domain-Driven Design**
- Implementa padrões DDD fundamentais
- Separação clara entre entidades e agregados
- Gerenciamento de eventos de domínio

### 3. **Flexibilidade**
- Classes base abstratas permitem extensão
- Interfaces bem definidas para contratos
- Suporte completo a LINQ

### 4. **Consistência**
- Comportamento uniforme para todas as entidades
- Implementação padrão de comparação e igualdade
- Gerenciamento consistente de estado

## Considerações de Implementação

### 1. **Identificadores**
- Use `EntityId<T>` para type safety
- Prefira tipos primitivos simples (int, Guid, string)
- Evite identificadores compostos quando possível
- **Nomenclatura**: Sempre use o padrão `nome + Id` (ex: `usuarioId`, `pedidoId`)

### 2. **Entidades vs Agregados**
- Use `Entity` para objetos simples com identificação
- Use `Aggregate` para objetos complexos com regras de negócio
- Agregados devem gerenciar seus próprios eventos

### 3. **Coleções**
- Herde de `EntityList<T>` para coleções especializadas
- Implemente métodos específicos de domínio
- Use LINQ para consultas complexas

### 4. **Eventos de Domínio**
- Registre eventos em métodos de negócio
- Use eventos para comunicação entre bounded contexts
- Mantenha eventos simples e focados