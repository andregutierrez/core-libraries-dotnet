# Sistema de Identifiers para DEPara (Data Exchange Para)

## Visão Geral

O sistema de **Identifiers** foi criado para permitir que entidades do domínio sejam identificadas em sistemas externos, facilitando o mapeamento DEPara (Data Exchange Para) entre diferentes sistemas e plataformas.

## Conceitos Principais

### Identifier
Classe base abstrata que representa uma identificação externa de uma entidade. Cada `Identifier` possui:
- **Key**: Uma chave alternativa (GUID) para referência externa
- **Type**: O tipo de sistema externo (ex: OpenAI, Salesforce, Microsoft Graph)
- **Id**: Identificador interno da própria identificação

### IdentifierType
Representa o tipo de sistema externo. Atualmente suporta:
- `OpenAIPlatform` - Para identificações do sistema OpenAI

### IdentifiersList
Coleção tipada que armazena múltiplos identificadores para uma entidade, permitindo que uma mesma entidade seja identificada em vários sistemas externos.

## Como Usar

### 1. Criar uma Classe de Identifier Específica

```csharp
using Core.Libraries.Domain.Entities;
using Core.LibrariesDomain.Entities.Identifiers;

namespace YourNamespace;

public class OpenAIIdentifier : Identifier
{
    public string OpenAIId { get; private set; } = string.Empty;
    public string? OrganizationId { get; private set; }

    protected OpenAIIdentifier() { }

    public OpenAIIdentifier(Guid key, string openAIId, string? organizationId = null)
        : base(key, IdentifierType.OpenAIPlatform)
    {
        OpenAIId = openAIId;
        OrganizationId = organizationId;
    }

    public OpenAIIdentifier(IdentifierId id, Guid key, string openAIId, string? organizationId = null)
        : base(id, key, IdentifierType.OpenAIPlatform)
    {
        OpenAIId = openAIId;
        OrganizationId = organizationId;
    }
}
```

### 2. Adicionar Identifiers a uma Entidade

```csharp
using Core.Libraries.Domain.Entities;
using Core.LibrariesDomain.Entities.Identifiers;

namespace YourNamespace;

public class User : Entity<EntityId>, IHasIdentifiers<IdentifiersList<OpenAIIdentifier>>
{
    public string Name { get; private set; } = string.Empty;
    
    // Coleção de identificadores externos
    private readonly IdentifiersList<OpenAIIdentifier> _identifiers = new();
    public IdentifiersList<OpenAIIdentifier> Identifiers => _identifiers;

    protected User() { }

    public User(EntityId id, string name) : base(id)
    {
        Name = name;
    }

    // Método para adicionar um identificador externo
    public void AddOpenAIIdentifier(string openAIId, string? organizationId = null)
    {
        var key = AlternateKey.New();
        var identifier = new OpenAIIdentifier(key, openAIId, organizationId);
        _identifiers.Add(identifier);
    }

    // Método para encontrar identificador por tipo
    public OpenAIIdentifier? GetOpenAIIdentifier()
    {
        return _identifiers.FirstOrDefault(i => i.Type == IdentifierType.OpenAIPlatform);
    }
}
```

### 3. Usar para DEPara

```csharp
// Exemplo: Sincronizar usuário com sistema externo
public class UserSyncService
{
    public async Task SyncUserToOpenAI(User user, string openAIId)
    {
        // Verificar se já existe identificador
        var existingIdentifier = user.GetOpenAIIdentifier();
        
        if (existingIdentifier == null)
        {
            // Criar novo identificador para mapeamento DEPara
            user.AddOpenAIIdentifier(openAIId);
        }
        else
        {
            // Atualizar identificador existente se necessário
            // (implementar lógica de atualização conforme necessário)
        }
    }

    public async Task<User?> FindUserByOpenAIId(string openAIId)
    {
        // Buscar usuário pelo identificador externo
        // Isso permite o mapeamento DEPara reverso
        return await _userRepository.FindByOpenAIId(openAIId);
    }
}
```

## Estrutura de Classes

### Identifier (Classe Base)
- `Key: AlternateKey` - Chave alternativa para referência externa
- `Type: IdentifierType` - Tipo de sistema externo
- `Id: IdentifierId` - Identificador interno

### IdentifierType
- `Code: int` - Código numérico único
- `Name: string` - Nome do tipo
- Métodos estáticos:
  - `List()` - Retorna todos os tipos disponíveis
  - `FromCode(int)` - Busca tipo por código
  - `FromName(string)` - Busca tipo por nome

### IdentifiersList<TIdentifier>
- Herda de `EntityList<TIdentifier>`
- Método `Add(TIdentifier)` - Adiciona um identificador
- Implementa `IReadOnlyCollection<TIdentifier>`

## Benefícios para DEPara

1. **Rastreabilidade**: Cada entidade pode ter múltiplos identificadores externos, permitindo rastrear a mesma entidade em diferentes sistemas.

2. **Flexibilidade**: Fácil adicionar novos tipos de sistemas externos através de `IdentifierType`.

3. **Type Safety**: Uso de genéricos garante type safety ao trabalhar com diferentes tipos de identificadores.

4. **Separação de Responsabilidades**: Identificadores externos são separados da lógica de domínio principal.

5. **Mapeamento Bidirecional**: Permite mapear de ID interno → ID externo e vice-versa.

## Exemplo Completo de Integração

```csharp
// 1. Criar entidade com suporte a identifiers
public class Customer : Entity<EntityId>, IHasIdentifiers<IdentifiersList<OpenAIIdentifier>>
{
    private readonly IdentifiersList<OpenAIIdentifier> _identifiers = new();
    public IdentifiersList<OpenAIIdentifier> Identifiers => _identifiers;
    
    // ... outras propriedades
}

// 2. Serviço de integração
public class IntegrationService
{
    public async Task SyncCustomerToOpenAI(Customer customer)
    {
        // Criar/atualizar no OpenAI
        var openAICustomer = await _openAIClient.CreateCustomer(customer);
        
        // Armazenar identificador para DEPara
        var key = AlternateKey.New();
        var identifier = new OpenAIIdentifier(key, openAICustomer.Id);
        customer.Identifiers.Add(identifier);
        
        await _repository.Save(customer);
    }

    public async Task<Customer?> FindCustomerByOpenAIId(string openAIId)
    {
        // Buscar pelo identificador externo (DEPara reverso)
        return await _repository.FindByOpenAIIdentifier(openAIId);
    }
}
```

## Boas Práticas

1. **Sempre use tipos específicos**: Crie classes específicas para cada tipo de sistema externo (ex: `OpenAIIdentifier`, `SalesforceIdentifier`).

2. **Validação**: Valide os dados do identificador externo antes de criar o `Identifier`.

3. **Imutabilidade**: Considere tornar as propriedades do identifier imutáveis após criação.

4. **Repository Pattern**: Implemente métodos no repositório para buscar por identificadores externos.

5. **Eventos de Domínio**: Considere disparar eventos de domínio quando identificadores são adicionados/atualizados.

## Extensibilidade

Para adicionar um novo tipo de sistema externo:

1. Adicione uma nova constante em `IdentifierType`:
```csharp
public static readonly IdentifierType SalesforcePlatform = new(2, "Salesforce Platform");
```

2. Crie uma classe específica de identifier:
```csharp
public class SalesforceIdentifier : Identifier
{
    public string SalesforceId { get; private set; }
    // ... implementação
}
```

3. Use nas entidades que precisam desse tipo de identificação.

