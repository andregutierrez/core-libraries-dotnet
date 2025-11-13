# Padrões de Código - Projeto de Domínio

## Visão Geral

Este documento define os padrões de código, convenções e boas práticas que devem ser seguidos ao desenvolver código em projetos que referenciem o **Core.Libraries.Domain**. Esta documentação serve como referência para desenvolvedores e IAs para garantir consistência e qualidade do código.

**IMPORTANTE**: Todo código gerado ou modificado deve seguir rigorosamente estes padrões.

---

## 1. Estrutura de Namespaces

### Regra Geral
- Os namespaces devem refletir a estrutura de pastas do projeto
- Use `Core.Libraries.Domain` como namespace raiz
- Subnamespaces devem seguir a hierarquia de pastas

### Exemplos

```
Pasta: src/Core.Libraries.Domain/Entities/
Namespace: Core.Libraries.Domain.Entities

Pasta: src/Core.Libraries.Domain/Entities/DomainEvents/
Namespace: Core.Libraries.Domain.Entities.DomainEvents

Pasta: src/Core.Libraries.Domain/ValueObjects/
Namespace: Core.Libraries.Domain.ValueObjects
```

### Regras
- ✅ **SEMPRE** alinhe o namespace com a estrutura de pastas
- ❌ **NUNCA** use namespaces que não correspondam à estrutura de pastas
- ✅ Use subpastas para organizar código relacionado

---

## 2. Domain-Driven Design (DDD)

### 2.1 Entidades (Entities)

#### Definição
Entidades são objetos de domínio que possuem identidade única e ciclo de vida.

#### Padrões Obrigatórios

1. **Herança de `Entity<TKey>`**
   ```csharp
   public class MinhaEntidade : Entity<EntityId>
   {
       // Implementação
   }
   ```

2. **Construtores Protegidos para ORM**
   ```csharp
   // Construtor padrão (para ORM/serialização)
   protected MinhaEntidade() { }
   
   // Construtor com ID
   protected MinhaEntidade(EntityId id) : base(id) { }
   ```

3. **Propriedades de Domínio**
   ```csharp
   public string Nome { get; protected set; } // Para ORM
   // OU
   public string Nome { get; init; } // Para imutabilidade
   ```

4. **Validações no Construtor**
   ```csharp
   protected MinhaEntidade(EntityId id, string nome) : base(id)
   {
       ArgumentNullException.ThrowIfNull(nome);
       if (string.IsNullOrWhiteSpace(nome))
           throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));
       
       Nome = nome;
   }
   ```

#### Regras
- ✅ **SEMPRE** herde de `Entity<TKey>` onde `TKey` implementa `IEntityId`
- ✅ **SEMPRE** forneça construtor protegido sem parâmetros para ORM
- ✅ **SEMPRE** valide parâmetros em construtores públicos/protegidos
- ✅ **SEMPRE** use `protected set` ou `init` para propriedades que precisam ser modificadas por ORM
- ❌ **NUNCA** exponha setters públicos para propriedades de domínio
- ❌ **NUNCA** crie entidades sem validação de invariantes

---

### 2.2 Agregados (Aggregates)

#### Definição
Agregados são clusters de entidades e objetos de valor tratados como uma unidade. O Aggregate Root é a única entrada para o agregado.

#### Padrões Obrigatórios

1. **Herança de `Aggregate<TEntityId>`**
   ```csharp
   public class MeuAgregado : Aggregate<EntityId>
   {
       // Implementação
   }
   ```

2. **Registro de Eventos de Domínio**
   ```csharp
   public void RealizarOperacao()
   {
       // Lógica de negócio
       RegisterEvent(new OperacaoRealizadaEvent(Id, DateTime.UtcNow));
   }
   ```

3. **Acesso a Eventos (via interface)**
   ```csharp
   var eventos = ((IHasDomainEvents)agregado).Events;
   ```

#### Regras
- ✅ **SEMPRE** herde de `Aggregate<TEntityId>` para Aggregate Roots
- ✅ **SEMPRE** registre eventos de domínio usando `RegisterEvent(object eventData)`
- ✅ **SEMPRE** valide que `eventData` não é null (já feito internamente)
- ❌ **NUNCA** acesse `_domainEvents` diretamente
- ❌ **NUNCA** registre eventos fora de métodos de domínio

---

### 2.3 Objetos de Valor (Value Objects)

#### Definição
Objetos de valor são imutáveis e identificados apenas por seus valores, não por identidade.

#### Padrões Obrigatórios

1. **Uso de `record`**
   ```csharp
   public record MeuValueObject
   {
       public string Valor { get; init; }
       
       public MeuValueObject(string valor)
       {
           ArgumentNullException.ThrowIfNull(valor);
           Valor = valor;
       }
       
       protected MeuValueObject() => Valor = default!; // Para ORM
   }
   ```

2. **Imutabilidade**
   ```csharp
   // ✅ CORRETO: Usar init
   public string Valor { get; init; }
   
   // ❌ ERRADO: Setter público
   public string Valor { get; set; }
   ```

3. **Validação**
   ```csharp
   public record Email
   {
       public string Value { get; init; }
       
       public Email(string value)
       {
           if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
               throw new ArgumentException("Email inválido.", nameof(value));
           
           Value = value;
       }
   }
   ```

#### Regras
- ✅ **SEMPRE** use `record` para objetos de valor
- ✅ **SEMPRE** use `init` para propriedades (não `set`)
- ✅ **SEMPRE** forneça construtor protegido sem parâmetros para ORM
- ✅ **SEMPRE** valide valores no construtor
- ❌ **NUNCA** use setters públicos em objetos de valor
- ❌ **NUNCA** crie objetos de valor mutáveis

---

### 2.4 Identificadores de Entidade (Entity IDs)

#### Padrões Obrigatórios

1. **Uso de `EntityId` ou tipos customizados**
   ```csharp
   // Usando EntityId padrão
   public class MinhaEntidade : Entity<EntityId>
   {
       // ...
   }
   
   // Usando ID customizado
   public record PedidoId : IEntityId, IComparable<PedidoId>
   {
       public int Value { get; init; }
       // Implementação...
   }
   ```

2. **Validação de IDs**
   ```csharp
   public EntityId(int value)
   {
       if (value < 0)
           throw new ArgumentOutOfRangeException(nameof(value), "Entity ID cannot be negative.");
       
       Value = value;
   }
   ```

#### Regras
- ✅ **SEMPRE** use tipos fortemente tipados para IDs (não `int` ou `Guid` diretamente)
- ✅ **SEMPRE** valide que IDs não são negativos (se aplicável)
- ✅ **SEMPRE** implemente `IEntityId`, `IEquatable<T>`, `IComparable<T>` para IDs customizados
- ❌ **NUNCA** use tipos primitivos diretamente como IDs de entidade

---

### 2.5 Eventos de Domínio (Domain Events)

#### Padrões Obrigatórios

1. **Criação de Eventos**
   ```csharp
   // ✅ CORRETO: Usar DomainEvent.Create
   var evento = DomainEvent.Create(entidade, eventData);
   
   // ❌ ERRADO: Usar construtor diretamente (exceto em casos especiais)
   var evento = new DomainEvent(entidade, eventData, order);
   ```

2. **Registro de Eventos em Agregados**
   ```csharp
   public void Confirmar()
   {
       // Lógica de negócio
       RegisterEvent(new PedidoConfirmadoEvent(Id, DateTime.UtcNow));
   }
   ```

3. **Estrutura de EventData**
   ```csharp
   // EventData pode ser qualquer objeto
   RegisterEvent(new { Action = "PedidoConfirmado", PedidoId = Id });
   
   // Ou uma classe específica
   public record PedidoConfirmadoEvent(EntityId PedidoId, DateTime ConfirmadoEm);
   RegisterEvent(new PedidoConfirmadoEvent(Id, DateTime.UtcNow));
   ```

#### Regras
- ✅ **SEMPRE** use `DomainEvent.Create()` para criar eventos (método recomendado)
- ✅ **SEMPRE** registre eventos através de `RegisterEvent()` em agregados
- ✅ **SEMPRE** passe a instância do agregado como referência (feito automaticamente)
- ❌ **NUNCA** registre eventos com `eventData` null
- ❌ **NUNCA** acesse `_domainEvents` diretamente em agregados

---

### 2.6 Coleções de Entidades (Entity Lists)

#### Padrões Obrigatórios

1. **Herança de `EntityList<TEntity>`**
   ```csharp
   public class MinhaLista : EntityList<MinhaEntidade>
   {
       // Implementação específica se necessário
   }
   ```

2. **Uso como ICollection**
   ```csharp
   var lista = new MinhaLista();
   ICollection<MinhaEntidade> collection = lista;
   
   collection.Add(entidade);
   collection.Remove(entidade);
   ```

3. **Suporte a LINQ (IQueryable)**
   ```csharp
   var lista = new MinhaLista();
   // Adicionar itens...
   
   // Consultas LINQ funcionam diretamente
   var resultado = lista.Where(e => e.Nome == "Teste").ToList();
   ```

#### Regras
- ✅ **SEMPRE** herde de `EntityList<TEntity>` para coleções de entidades
- ✅ **SEMPRE** use `ICollection<T>` para adicionar/remover itens
- ✅ **SEMPRE** valide que itens não são null antes de adicionar
- ❌ **NUNCA** acesse `_list` diretamente (use a interface)
- ❌ **NUNCA** adicione itens null à coleção

---

## 3. Convenções de Nomenclatura

### 3.1 Classes e Records

- ✅ **PascalCase** para nomes de classes, records e interfaces
- ✅ Nomes descritivos e específicos do domínio
- ❌ Evite abreviações desnecessárias

```csharp
// ✅ CORRETO
public class Pedido : Aggregate<EntityId> { }
public record Email : IValueObject { }

// ❌ ERRADO
public class Pdd : Aggregate<EntityId> { }
public record Eml { }
```

### 3.2 Propriedades e Métodos

- ✅ **PascalCase** para propriedades e métodos públicos
- ✅ **camelCase** para parâmetros e variáveis locais
- ✅ **PascalCase** para constantes

```csharp
// ✅ CORRETO
public string Nome { get; init; }
public void ProcessarPedido(int pedidoId) { }

// ❌ ERRADO
public string nome { get; init; }
public void processarPedido(int PedidoId) { }
```

### 3.3 Campos Privados

- ✅ Prefixo `_` (underscore) para campos privados
- ✅ **camelCase** após o underscore

```csharp
// ✅ CORRETO
private readonly DomainEventList _domainEvents = new();
protected readonly List<TEntity> _list = new();

// ❌ ERRADO
private readonly DomainEventList domainEvents = new();
protected readonly List<TEntity> list = new();
```

### 3.4 Interfaces

- ✅ Prefixo `I` para interfaces
- ✅ Nomes descritivos

```csharp
// ✅ CORRETO
public interface IHasDomainEvents { }
public interface IDomainEventList { }

// ❌ ERRADO
public interface HasDomainEvents { }
public interface DomainEventListInterface { }
```

---

## 4. Documentação XML

### 4.1 Obrigatoriedade

- ✅ **SEMPRE** documente classes públicas, métodos públicos e propriedades públicas
- ✅ **SEMPRE** documente construtores públicos/protegidos
- ✅ **SEMPRE** documente exceções lançadas

### 4.2 Estrutura Padrão

```csharp
/// <summary>
/// Descrição breve e clara do propósito.
/// </summary>
/// <remarks>
/// <para>
/// Descrição detalhada quando necessário.
/// </para>
/// </remarks>
/// <typeparam name="T">Descrição do parâmetro de tipo.</typeparam>
/// <param name="parametro">Descrição do parâmetro.</param>
/// <returns>Descrição do retorno.</returns>
/// <exception cref="ArgumentNullException">Quando o parâmetro é null.</exception>
public void Metodo<T>(T parametro) { }
```

### 4.3 Tags XML Comuns

- `<summary>`: Descrição breve (obrigatório)
- `<remarks>`: Descrição detalhada com `<para>` para parágrafos
- `<param>`: Descrição de parâmetros
- `<returns>`: Descrição do valor de retorno
- `<exception>`: Documentação de exceções
- `<typeparam>`: Descrição de parâmetros de tipo genérico
- `<example>`: Exemplos de uso com `<code>`
- `<see cref="..."/>`: Referência a outros tipos/membros

### 4.4 Idioma

- ✅ **SEMPRE** use **Português** para documentação XML
- ✅ Use termos técnicos em inglês quando apropriado (ex: "Aggregate Root", "Domain Event")

---

## 5. Validação e Tratamento de Erros

### 5.1 Validação de Null

- ✅ **SEMPRE** use `ArgumentNullException.ThrowIfNull()` para validação de null

```csharp
// ✅ CORRETO
public void Metodo(string parametro)
{
    ArgumentNullException.ThrowIfNull(parametro);
    // ...
}

// ❌ ERRADO
public void Metodo(string parametro)
{
    if (parametro == null)
        throw new ArgumentNullException(nameof(parametro));
    // ...
}
```

### 5.2 Validação de Argumentos

- ✅ **SEMPRE** valide argumentos no início dos métodos/construtores
- ✅ **SEMPRE** use exceções apropriadas (`ArgumentException`, `ArgumentOutOfRangeException`, etc.)
- ✅ **SEMPRE** inclua o nome do parâmetro usando `nameof()`

```csharp
// ✅ CORRETO
public EntityId(int value)
{
    if (value < 0)
        throw new ArgumentOutOfRangeException(nameof(value), "Entity ID cannot be negative.");
    
    Value = value;
}
```

### 5.3 Mensagens de Erro

- ✅ **SEMPRE** use mensagens de erro em **Inglês** para consistência
- ✅ Seja claro e específico sobre o que está errado

```csharp
// ✅ CORRETO
throw new ArgumentException("Entity ID cannot be negative.", nameof(value));

// ❌ ERRADO (inconsistente)
throw new ArgumentException("ID não pode ser negativo.", nameof(value));
```

---

## 6. Imutabilidade e Thread-Safety

### 6.1 Objetos de Valor

- ✅ **SEMPRE** torne objetos de valor imutáveis
- ✅ Use `init` ao invés de `set` para propriedades

```csharp
// ✅ CORRETO
public record Email
{
    public string Value { get; init; }
}

// ❌ ERRADO
public record Email
{
    public string Value { get; set; }
}
```

### 6.2 Thread-Safety em Eventos

- ✅ **SEMPRE** use `Interlocked.Increment` para incremento thread-safe de contadores

```csharp
// ✅ CORRETO
private static long _last;
var order = Interlocked.Increment(ref _last);

// ❌ ERRADO
private static long _last;
var order = ++_last; // Não é thread-safe
```

---

## 7. Compatibilidade com ORM

### 7.1 Construtores Protegidos

- ✅ **SEMPRE** forneça construtor protegido sem parâmetros para ORM/serialização

```csharp
// ✅ CORRETO
protected Entity() => _id = default!;
protected Aggregate() { }
protected EntityId() => Value = default;

// ❌ ERRADO (sem construtor protegido)
// ORM não conseguirá instanciar
```

### 7.2 Propriedades com Setters Protegidos

- ✅ Use `protected set` quando ORM precisar modificar propriedades

```csharp
// ✅ CORRETO (para ORM)
public string Nome { get; protected set; }

// ✅ TAMBÉM CORRETO (para imutabilidade)
public string Nome { get; init; }
```

---

## 8. Performance e Otimização

### 8.1 Cache de IQueryable

- ✅ **SEMPRE** limpe o cache de `IQueryable` quando a coleção for modificada

```csharp
// ✅ CORRETO (como em EntityList)
void ICollection<TEntity>.Add(TEntity item)
{
    ArgumentNullException.ThrowIfNull(item);
    _list.Add(item);
    _queryable = null; // Limpa o cache
}
```

### 8.2 Comparações Diretas

- ✅ Use comparações diretas quando possível (mais eficiente)

```csharp
// ✅ CORRETO
public int CompareTo(EntityId? other)
{
    if (other is null) return 1;
    return Value.CompareTo(other.Value); // Direto
}

// ❌ MENOS EFICIENTE
return Comparer<int>.Default.Compare(Value, other.Value);
```

---

## 9. Exemplos Completos

### 9.1 Entidade Completa

```csharp
namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Representa um pedido no sistema.
/// </summary>
public class Pedido : Entity<EntityId>
{
    /// <summary>
    /// Obtém o número do pedido.
    /// </summary>
    public string Numero { get; protected set; } = string.Empty;
    
    /// <summary>
    /// Obtém a data de criação do pedido.
    /// </summary>
    public DateTime CriadoEm { get; protected set; }
    
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Pedido"/>.
    /// </summary>
    protected Pedido() { }
    
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Pedido"/> com um identificador e número.
    /// </summary>
    /// <param name="id">O identificador único do pedido.</param>
    /// <param name="numero">O número do pedido.</param>
    /// <exception cref="ArgumentNullException">Lançado quando numero é null.</exception>
    protected Pedido(EntityId id, string numero) : base(id)
    {
        ArgumentNullException.ThrowIfNull(numero);
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("Número do pedido não pode ser vazio.", nameof(numero));
        
        Numero = numero;
        CriadoEm = DateTime.UtcNow;
    }
}
```

### 9.2 Agregado Completo

```csharp
namespace Core.Libraries.Domain.Entities;

/// <summary>
/// Representa um agregado de pedido com eventos de domínio.
/// </summary>
public class PedidoAgregado : Aggregate<EntityId>
{
    /// <summary>
    /// Obtém o número do pedido.
    /// </summary>
    public string Numero { get; protected set; } = string.Empty;
    
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="PedidoAgregado"/>.
    /// </summary>
    protected PedidoAgregado() { }
    
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="PedidoAgregado"/>.
    /// </summary>
    /// <param name="id">O identificador único do pedido.</param>
    /// <param name="numero">O número do pedido.</param>
    protected PedidoAgregado(EntityId id, string numero) : base(id)
    {
        ArgumentNullException.ThrowIfNull(numero);
        Numero = numero;
    }
    
    /// <summary>
    /// Confirma o pedido.
    /// </summary>
    public void Confirmar()
    {
        // Lógica de negócio
        RegisterEvent(new PedidoConfirmadoEvent(Id, DateTime.UtcNow));
    }
}

/// <summary>
/// Evento de domínio para pedido confirmado.
/// </summary>
public record PedidoConfirmadoEvent(EntityId PedidoId, DateTime ConfirmadoEm);
```

### 9.3 Objeto de Valor Completo

```csharp
namespace Core.Libraries.Domain.ValueObjects;

/// <summary>
/// Representa um endereço de email válido.
/// </summary>
public record Email
{
    /// <summary>
    /// Obtém o valor do email.
    /// </summary>
    public string Value { get; init; }
    
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Email"/>.
    /// </summary>
    protected Email() => Value = default!;
    
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Email"/> com um valor.
    /// </summary>
    /// <param name="value">O valor do email.</param>
    /// <exception cref="ArgumentException">Lançado quando o email é inválido.</exception>
    public Email(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            throw new ArgumentException("Email inválido.", nameof(value));
        
        Value = value;
    }
    
    /// <summary>
    /// Conversão implícita de string para Email.
    /// </summary>
    public static implicit operator Email(string value) => new(value);
}
```

---

## 10. Checklist de Código

Antes de considerar o código completo, verifique:

### Estrutura
- [ ] Namespace corresponde à estrutura de pastas
- [ ] Classe/Record herda da classe base apropriada (`Entity<TKey>`, `Aggregate<TEntityId>`, etc.)
- [ ] Construtor protegido sem parâmetros para ORM existe

### Validação
- [ ] Todos os parâmetros são validados
- [ ] `ArgumentNullException.ThrowIfNull()` é usado para null checks
- [ ] Mensagens de erro estão em inglês

### Documentação
- [ ] XML documentation completa para membros públicos
- [ ] Documentação em português
- [ ] Exceções documentadas com `<exception>`

### Nomenclatura
- [ ] PascalCase para classes, métodos, propriedades
- [ ] camelCase para parâmetros e variáveis locais
- [ ] Prefixo `_` para campos privados
- [ ] Prefixo `I` para interfaces

### Imutabilidade
- [ ] Objetos de valor usam `init` ao invés de `set`
- [ ] Propriedades de domínio usam `protected set` ou `init`

### Eventos de Domínio
- [ ] Eventos registrados usando `RegisterEvent()`
- [ ] `DomainEvent.Create()` usado para criar eventos

---

## 11. Referências e Recursos

- **Domain-Driven Design (DDD)**: Padrões de design para modelagem de domínio
- **C# Records**: Documentação oficial sobre records
- **XML Documentation Comments**: Padrões de documentação XML

---

## 12. Notas Finais

Este documento deve ser consultado sempre que:
- Criar novas classes de domínio
- Modificar código existente
- Revisar código de outros desenvolvedores
- Configurar IAs para gerar código

**Lembre-se**: Consistência é fundamental. Seguir estes padrões garante código limpo, manutenível e alinhado com as melhores práticas de DDD e C#.

---

**Última atualização**: 2025
**Versão**: 1.0

