# Índice da Documentação

## Arquitetura e Padrões

### Domain-Driven Design (DDD)
- **[Entities](./Entities.md)** - Entidades, Agregados e Objetos de Valor
  - `Entity<TKey>` - Classe base para entidades
  - `Aggregate<TEntityId>` - Classe base para agregados
  - `EntityId<T>` - Identificadores fortemente tipados
  - `EntityList<TEntity>` - Coleções especializadas
- **[AlternateKeys](./AlternateKeys.md)** - Chaves Alternativas para Integração
  - `AlternateKey` - Struct para chaves alternativas
  - `IHasAlternateKey` - Interface para entidades com chave alternativa
- **[DomainEvents](./DomainEvents.md)** - Eventos de Domínio
  - `DomainEvent` - Classe que representa um evento de domínio
  - `DomainEventList` - Lista gerenciadora de eventos
  - `IDomainEventList` - Interface para gerenciamento de eventos
  - `IHasDomainEvents` - Interface para entidades com eventos

## 📚 Conceitos Fundamentais

### Entidades
- **Identificação única** com type safety
- **Comparação e igualdade** consistentes
- **Estado transitório** para entidades não persistidas
- **Herança** para especialização

### Agregados
- **Aggregate Root** pattern
- **Eventos de domínio** para comunicação
- **Invariantes de negócio** centralizadas
- **Transações** de domínio

### Identificadores
- **Type safety** com generics
- **Imutabilidade** com records
- **Serialização** para persistência
- **Comparação** para ordenação

### Coleções
- **LINQ support** completo
- **Type safety** com generics
- **Múltiplas interfaces** (ICollection, IQueryable)
- **Extensibilidade** para comportamentos específicos

### Chaves Alternativas
- **Integração entre sistemas** com identificadores estáveis
- **Type safety** com struct readonly
- **Conversões implícitas** para Guid
- **Validação** automática de valores vazios

### Eventos de Domínio
- **Desacoplamento** entre lógica de negócio e efeitos colaterais
- **Integração** entre bounded contexts
- **Auditoria** e rastreamento de operações
- **Processamento assíncrono** de efeitos colaterais

## 🛠️ Guias de Implementação

### Criando Entidades
```csharp
public class Usuario : Entity<EntityId<int>>
{
    public Usuario(EntityId<int> usuarioId) : base(usuarioId) { }
    // Implementação específica
}
```

### Criando Agregados
```csharp
public class Pedido : Aggregate<EntityId<Guid>>
{
    public Pedido(EntityId<Guid> pedidoId) : base(pedidoId) { }
    // Lógica de negócio e eventos
}
```

### Usando Coleções
```csharp
public class ListaUsuarios : EntityList<Usuario>
{
    // Métodos específicos de domínio
}
```

### Implementando Chaves Alternativas
```csharp
public class Usuario : Entity<EntityId<int>>, IHasAlternateKey
{
    public AlternateKey Key { get; private set; }
    
    public Usuario(EntityId<int> usuarioId) : base(usuarioId) 
    {
        Key = AlternateKey.New();
    }
}
```

### Implementando Eventos de Domínio
```csharp
public class Pedido : Aggregate<EntityId<Guid>>
{
    public void ProcessarPedido()
    {
        // Lógica de negócio
        RegisterEvent(new PedidoProcessadoEvent(Id, NumeroPedido));
    }
}
```

## 📖 Referências Rápidas

### Classes Principais
- `Entity<TKey>` - Base para todas as entidades
- `Aggregate<TEntityId>` - Base para agregados
- `EntityId<T>` - Identificadores tipados
- `EntityList<TEntity>` - Coleções de entidades
- `AlternateKey` - Chaves alternativas para integração
- `IHasAlternateKey` - Interface para entidades com chave alternativa
- `DomainEvent` - Classe que representa um evento de domínio
- `DomainEventList` - Lista gerenciadora de eventos

### Interfaces
- `IEntityId` - Contrato para identificadores
- `IHasDomainEvents` - Contrato para eventos de domínio
- `IHasAlternateKey` - Contrato para chaves alternativas
- `IDomainEventList` - Contrato para gerenciamento de eventos

### Padrões Aplicados
- **Repository Pattern** (a implementar)
- **Unit of Work** (a implementar)
- **Domain Events** ✅ Implementado
- **Specification Pattern** (a implementar)

## 🔗 Links Úteis

- [Entities - Documentação Completa](./Entities.md)
- [README Principal](../README.md)
- [Solução Visual Studio](../Core.Libraries.sln)

## 📝 Próximos Passos

1. **Implementar interfaces de eventos** (`IHasDomainEvents`, `IDomainEventList`)
2. **Criar classes de eventos** específicos do domínio
3. **Implementar repositórios** para persistência
4. **Adicionar validações** de domínio
5. **Criar testes unitários** para as classes base
