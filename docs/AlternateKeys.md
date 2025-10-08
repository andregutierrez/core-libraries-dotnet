# Documentação - Pasta AlternateKeys

A pasta `AlternateKeys` contém implementações para chaves alternativas de entidades, fornecendo identificadores únicos adicionais além das chaves primárias. Essas chaves são especialmente úteis para integração entre sistemas e referências externas.

## Visão Geral

As chaves alternativas são identificadores únicos adicionais que complementam as chaves primárias das entidades. Elas são especialmente importantes para:
- **Integração entre sistemas**: Identificação consistente entre diferentes bounded contexts
- **Referências externas**: APIs públicas e integrações
- **Auditoria e rastreamento**: Identificadores estáveis para logs e auditoria
- **Migração de dados**: Manutenção de referências durante migrações

## Estrutura de Arquivos

```
src/Core.Libraries.Domain/Entities/AlternateKeys/
├── AlternateKey.cs        # Struct para chave alternativa
└── IHasAlternateKey.cs    # Interface para entidades com chave alternativa
```

## Classes e Interfaces

### 1. AlternateKey

**Arquivo:** `AlternateKey.cs`

Struct readonly que representa uma chave alternativa globalmente única usada para identificação de entidades entre sistemas. Este objeto de valor encapsula um `Guid` para garantir type safety e clareza semântica.

#### Características:
- **Imutável**: Struct readonly para garantir imutabilidade
- **Type Safety**: Encapsula Guid com validação
- **Conversões implícitas**: Facilita uso com Guid
- **Validação**: Impede criação com Guid.Empty

#### Propriedades:
- `Value`: Valor subjacente Guid da chave alternativa

#### Construtores:
```csharp
public AlternateKey(Guid value)    // Com validação de Guid.Empty
```

#### Métodos Estáticos:
```csharp
public static AlternateKey New()   // Gera nova chave com Guid.NewGuid()
```

#### Operadores:
```csharp
// Conversão implícita para Guid
public static implicit operator Guid(AlternateKey key)

// Conversão implícita de Guid
public static implicit operator AlternateKey(Guid guid)
```

#### Métodos:
- `Equals()`: Comparação de igualdade
- `GetHashCode()`: Código hash baseado no Guid
- `ToString()`: Representação em string

#### Exemplo de uso:
```csharp
// Criar nova chave alternativa
var chaveAlternativa = AlternateKey.New();

// Criar a partir de Guid existente
var guid = Guid.NewGuid();
var chaveAlternativa2 = new AlternateKey(guid);

// Conversão implícita
Guid valorGuid = chaveAlternativa;
AlternateKey chave = valorGuid;
```

### 2. IHasAlternateKey

**Arquivo:** `IHasAlternateKey.cs`

Interface que fornece mecanismo para implementar chaves alternativas em entidades. Uma chave alternativa serve como um identificador único adicional para uma entidade, utilizada para propósitos de identificação além da chave primária.

#### Características:
- **Contrato simples**: Interface mínima com uma propriedade
- **Flexibilidade**: Pode ser implementada por qualquer entidade
- **Type Safety**: Usa AlternateKey em vez de Guid direto

#### Propriedades:
- `Key`: A chave alternativa da entidade (tipo AlternateKey)

#### Exemplo de implementação:
```csharp
public class Usuario : Entity<EntityId<int>>, IHasAlternateKey
{
    public AlternateKey Key { get; private set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    
    public Usuario(EntityId<int> usuarioId, string nome, string email) 
        : base(usuarioId)
    {
        Nome = nome;
        Email = email;
        Key = AlternateKey.New(); // Gera chave alternativa automaticamente
    }
}
```

## Padrões de Uso

### Implementando Chave Alternativa em Entidade

```csharp
public class Produto : Entity<EntityId<int>>, IHasAlternateKey
{
    public AlternateKey Key { get; private set; }
    public string Nome { get; set; }
    public string Codigo { get; set; }
    public decimal Preco { get; set; }
    
    public Produto(EntityId<int> produtoId, string nome, string codigo, decimal preco) 
        : base(produtoId)
    {
        Nome = nome;
        Codigo = codigo;
        Preco = preco;
        Key = AlternateKey.New();
    }
    
    // Método para atualizar chave alternativa se necessário
    public void AtualizarChaveAlternativa()
    {
        Key = AlternateKey.New();
    }
}
```

### Implementando Chave Alternativa em Agregado

```csharp
public class Pedido : Aggregate<EntityId<Guid>>, IHasAlternateKey
{
    public AlternateKey Key { get; private set; }
    public string NumeroPedido { get; set; }
    public string NomeCliente { get; set; }
    public decimal Total { get; set; }
    
    public Pedido(EntityId<Guid> pedidoId, string numeroPedido, string nomeCliente) 
        : base(pedidoId)
    {
        NumeroPedido = numeroPedido;
        NomeCliente = nomeCliente;
        Key = AlternateKey.New();
    }
    
    public void ProcessarPedido()
    {
        // Lógica de negócio
        RegisterEvent(new PedidoProcessadoEvent(Key, NumeroPedido));
    }
}
```

### Usando Chaves Alternativas em Repositórios

```csharp
public interface IUsuarioRepository
{
    Task<Usuario?> BuscarPorId(EntityId<int> usuarioId);
    Task<Usuario?> BuscarPorChaveAlternativa(AlternateKey chaveAlternativa);
    Task<Usuario?> BuscarPorEmail(string email);
}

public class UsuarioRepository : IUsuarioRepository
{
    public async Task<Usuario?> BuscarPorChaveAlternativa(AlternateKey chaveAlternativa)
    {
        // Busca usando a chave alternativa
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Key == chaveAlternativa);
    }
}
```

### Integração entre Sistemas

```csharp
// Sistema A - Enviando dados
public class IntegracaoSistemaA
{
    public async Task EnviarUsuario(Usuario usuario)
    {
        var dadosUsuario = new
        {
            ChaveAlternativa = usuario.Key, // Usa chave alternativa
            Nome = usuario.Nome,
            Email = usuario.Email
        };
        
        await _httpClient.PostAsync("/api/usuarios", dadosUsuario);
    }
}

// Sistema B - Recebendo dados
public class IntegracaoSistemaB
{
    public async Task ProcessarUsuario(UsuarioDto dto)
    {
        // Busca por chave alternativa (mais estável que ID interno)
        var usuario = await _usuarioRepository
            .BuscarPorChaveAlternativa(dto.ChaveAlternativa);
            
        if (usuario == null)
        {
            // Criar novo usuário
            usuario = new Usuario(EntityId<int>.New(), dto.Nome, dto.Email);
        }
        else
        {
            // Atualizar usuário existente
            usuario.AtualizarDados(dto.Nome, dto.Email);
        }
    }
}
```

## Benefícios

### 1. **Integração entre Sistemas**
- Identificadores estáveis para comunicação entre bounded contexts
- Reduz acoplamento entre sistemas
- Facilita migrações e sincronização

### 2. **Type Safety**
- Encapsula Guid com validação
- Previne erros de tipo em tempo de compilação
- Conversões implícitas seguras

### 3. **Flexibilidade**
- Pode ser implementada por qualquer entidade
- Não interfere com chaves primárias existentes
- Facilita evolução do modelo de dados

### 4. **Auditoria e Rastreamento**
- Identificadores estáveis para logs
- Facilita correlação entre eventos
- Melhora rastreabilidade de dados

## Considerações de Implementação

### 1. **Quando Usar Chaves Alternativas**
- **Integração entre sistemas**: Quando entidades precisam ser referenciadas externamente
- **APIs públicas**: Para identificadores estáveis em APIs
- **Migração de dados**: Para manter referências durante migrações
- **Auditoria**: Para rastreamento consistente de mudanças

### 2. **Quando NÃO Usar**
- **Chaves primárias simples**: Para entidades internas sem necessidade de integração
- **Performance crítica**: Quando a sobrecarga adicional não é justificada
- **Sistemas isolados**: Quando não há necessidade de integração externa

### 3. **Boas Práticas**
- **Gere automaticamente**: Use `AlternateKey.New()` no construtor
- **Mantenha imutável**: Não altere chaves alternativas após criação
- **Use para integração**: Foque em casos de integração entre sistemas
- **Documente o uso**: Deixe claro quando e por que usar chaves alternativas

### 4. **Performance**
- **Índices**: Crie índices únicos para chaves alternativas
- **Consultas**: Use chaves alternativas para consultas externas
- **Cache**: Considere cache para chaves alternativas frequentemente acessadas

## Dependências

Esta pasta depende de:
- **System**: Para Guid e ArgumentException
- **Core.Libraries.Domain.Entities**: Para Entity e EntityId

## Exemplos de Casos de Uso

### 1. **E-commerce Multi-tenant**
```csharp
// Produto compartilhado entre tenants
public class Produto : Entity<EntityId<int>>, IHasAlternateKey
{
    public AlternateKey Key { get; private set; }
    public string Nome { get; set; }
    public string Codigo { get; set; }
    
    // Chave alternativa permite referência entre tenants
    public void CompartilharEntreTenants()
    {
        // Usa Key para referência estável
    }
}
```

### 2. **Sistema de Notificações**
```csharp
// Evento com referência estável
public class NotificacaoEnviadaEvent : IDomainEvent
{
    public AlternateKey UsuarioKey { get; }
    public string TipoNotificacao { get; }
    public DateTime DataEnvio { get; }
    
    public NotificacaoEnviadaEvent(AlternateKey usuarioKey, string tipo)
    {
        UsuarioKey = usuarioKey;
        TipoNotificacao = tipo;
        DataEnvio = DateTime.UtcNow;
    }
}
```

### 3. **API Externa**
```csharp
// DTO para API externa
public class UsuarioApiDto
{
    public AlternateKey ChaveAlternativa { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    
    // Chave alternativa é estável para clientes da API
}
```