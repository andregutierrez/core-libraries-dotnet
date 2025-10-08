# Documentação - Pasta DomainEvents

A pasta `DomainEvents` contém a implementação do padrão Domain Events, que permite que entidades e agregados publiquem eventos que ocorrem durante seu ciclo de vida. Esses eventos podem ser processados de forma assíncrona para disparar efeitos colaterais, integrações ou notificações.

## Visão Geral

Os eventos de domínio são uma parte fundamental do Domain-Driven Design (DDD) que permite:
- **Desacoplamento**: Separação entre lógica de negócio e efeitos colaterais
- **Integração**: Comunicação entre bounded contexts
- **Auditoria**: Rastreamento de mudanças e operações
- **Notificações**: Disparo de alertas e notificações
- **Saga Pattern**: Coordenação de operações distribuídas

## Estrutura de Arquivos

```
src/Core.Libraries.Domain/Entities/DomainEvents/
├── DomainEvent.cs        # Classe que representa um evento de domínio
├── DomainEventList.cs    # Lista gerenciadora de eventos
├── IDomainEventList.cs   # Interface para gerenciamento de eventos
└── IHasDomainEvents.cs   # Interface para entidades com eventos
```

## Classes e Interfaces

### 1. DomainEvent

**Arquivo:** `DomainEvent.cs`

Classe que representa um evento de domínio que ocorreu no sistema. Cada evento contém dados sobre a ocorrência e mantém uma ordem sequencial.

#### Características:
- **Ordenação**: Cada evento possui uma ordem sequencial única
- **Thread-safe**: Geração de ordem usando `Interlocked.Increment`
- **Flexível**: Aceita qualquer tipo de dados como payload
- **Rastreável**: Mantém referência à entidade que gerou o evento

#### Propriedades:
- `EventData`: Os dados do evento de domínio (object)
- `Entity`: A entidade relacionada ao evento (object)
- `Order`: A ordem sequencial do evento (long)

#### Métodos Estáticos:
```csharp
public static DomainEvent Create(object entity, object eventData)
```

#### Exemplo de uso:
```csharp
// Criar um evento de domínio
var evento = DomainEvent.Create(usuario, new UsuarioCriadoEvent(usuario.Nome, usuario.Email));

// Acessar propriedades
var dadosEvento = evento.EventData;
var entidade = evento.Entity;
var ordem = evento.Order;
```

### 2. DomainEventList

**Arquivo:** `DomainEventList.cs`

Implementação padrão de `IDomainEventList` usada para gerenciar e coletar eventos de domínio gerados por um agregado. Herda de `EntityList<DomainEvent>` para aproveitar funcionalidades de coleção.

#### Características:
- **Coleção especializada**: Herda de `EntityList<DomainEvent>`
- **Thread-safe**: Operações seguras para múltiplas threads
- **Flexível**: Aceita qualquer tipo de dados como evento
- **Integração**: Funciona com agregados através de `IHasDomainEvents`

#### Métodos:
- `RegisterEvent(object eventData)`: Registra um novo evento
- `Clear()`: Limpa todos os eventos registrados
- `GetAll()`: Retorna todos os eventos registrados

#### Exemplo de uso:
```csharp
var listaEventos = new DomainEventList();

// Registrar eventos
listaEventos.RegisterEvent(new UsuarioCriadoEvent("João", "joao@email.com"));
listaEventos.RegisterEvent(new UsuarioAtualizadoEvent(usuarioId, "Novo Nome"));

// Obter todos os eventos
var eventos = listaEventos.GetAll();

// Limpar eventos após processamento
listaEventos.Clear();
```

### 3. IDomainEventList

**Arquivo:** `IDomainEventList.cs`

Interface que define um contrato para gerenciar eventos de domínio dentro de um agregado ou entidade. Permite coleta, inspeção e limpeza explícita de eventos.

#### Características:
- **Contrato simples**: Interface mínima para gerenciamento de eventos
- **Flexibilidade**: Pode ser implementada de diferentes formas
- **Padrões DDD**: Suporta padrões como event dispatching e transactional outbox

#### Métodos:
- `Clear()`: Limpa todos os eventos registrados
- `GetAll()`: Retorna todos os eventos registrados

### 4. IHasDomainEvents

**Arquivo:** `IHasDomainEvents.cs`

Interface que representa uma entidade que expõe eventos de domínio gerados durante seu ciclo de vida. Permite que agregados publiquem eventos para processamento assíncrono.

#### Características:
- **Contrato de eventos**: Define que a entidade pode gerar eventos
- **Integração**: Funciona com agregados e entidades
- **Flexibilidade**: Permite diferentes implementações de gerenciamento

#### Propriedades:
- `Events`: Coleção de eventos de domínio (IDomainEventList)

## Padrões de Uso

### Implementando Eventos em Agregado

```csharp
public class Pedido : Aggregate<EntityId<Guid>>
{
    public string NumeroPedido { get; set; }
    public string NomeCliente { get; set; }
    public decimal Total { get; set; }
    public StatusPedido Status { get; set; }
    
    public Pedido(EntityId<Guid> pedidoId, string numeroPedido, string nomeCliente) 
        : base(pedidoId)
    {
        NumeroPedido = numeroPedido;
        NomeCliente = nomeCliente;
        Status = StatusPedido.Criado;
        
        // Registrar evento de criação
        RegisterEvent(new PedidoCriadoEvent(Id, NumeroPedido, NomeCliente));
    }
    
    public void ProcessarPedido()
    {
        if (Status != StatusPedido.Criado)
            throw new InvalidOperationException("Pedido já foi processado");
            
        Status = StatusPedido.Processando;
        
        // Registrar evento de processamento
        RegisterEvent(new PedidoProcessadoEvent(Id, NumeroPedido));
    }
    
    public void FinalizarPedido()
    {
        Status = StatusPedido.Finalizado;
        
        // Registrar evento de finalização
        RegisterEvent(new PedidoFinalizadoEvent(Id, NumeroPedido, Total));
    }
}
```

### Criando Classes de Evento

```csharp
// Evento de criação de usuário
public class UsuarioCriadoEvent
{
    public EntityId<int> UsuarioId { get; }
    public string Nome { get; }
    public string Email { get; }
    public DateTime DataCriacao { get; }
    
    public UsuarioCriadoEvent(EntityId<int> usuarioId, string nome, string email)
    {
        UsuarioId = usuarioId;
        Nome = nome;
        Email = email;
        DataCriacao = DateTime.UtcNow;
    }
}

// Evento de atualização de usuário
public class UsuarioAtualizadoEvent
{
    public EntityId<int> UsuarioId { get; }
    public string NomeAnterior { get; }
    public string NomeNovo { get; }
    public DateTime DataAtualizacao { get; }
    
    public UsuarioAtualizadoEvent(EntityId<int> usuarioId, string nomeAnterior, string nomeNovo)
    {
        UsuarioId = usuarioId;
        NomeAnterior = nomeAnterior;
        NomeNovo = nomeNovo;
        DataAtualizacao = DateTime.UtcNow;
    }
}
```

### Processando Eventos de Domínio

```csharp
public class ProcessadorEventosDominio
{
    private readonly IServiceProvider _serviceProvider;
    
    public ProcessadorEventosDominio(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task ProcessarEventosAsync(IEnumerable<DomainEvent> eventos)
    {
        foreach (var evento in eventos.OrderBy(e => e.Order))
        {
            await ProcessarEventoAsync(evento);
        }
    }
    
    private async Task ProcessarEventoAsync(DomainEvent evento)
    {
        switch (evento.EventData)
        {
            case UsuarioCriadoEvent usuarioCriado:
                await ProcessarUsuarioCriadoAsync(usuarioCriado);
                break;
                
            case PedidoProcessadoEvent pedidoProcessado:
                await ProcessarPedidoProcessadoAsync(pedidoProcessado);
                break;
                
            case PedidoFinalizadoEvent pedidoFinalizado:
                await ProcessarPedidoFinalizadoAsync(pedidoFinalizado);
                break;
        }
    }
    
    private async Task ProcessarUsuarioCriadoAsync(UsuarioCriadoEvent evento)
    {
        // Enviar email de boas-vindas
        var emailService = _serviceProvider.GetService<IEmailService>();
        await emailService.EnviarEmailBoasVindasAsync(evento.Email, evento.Nome);
        
        // Registrar em sistema de auditoria
        var auditoriaService = _serviceProvider.GetService<IAuditoriaService>();
        await auditoriaService.RegistrarEventoAsync("UsuarioCriado", evento.UsuarioId);
    }
}
```

### Usando com Repositórios

```csharp
public class PedidoRepository : IPedidoRepository
{
    private readonly DbContext _context;
    private readonly ProcessadorEventosDominio _processadorEventos;
    
    public PedidoRepository(DbContext context, ProcessadorEventosDominio processadorEventos)
    {
        _context = context;
        _processadorEventos = processadorEventos;
    }
    
    public async Task SalvarAsync(Pedido pedido)
    {
        // Salvar o agregado
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
        
        // Processar eventos de domínio
        var eventos = pedido.Events.GetAll();
        await _processadorEventos.ProcessarEventosAsync(eventos);
        
        // Limpar eventos após processamento
        pedido.Events.Clear();
    }
}
```

## Benefícios

### 1. **Desacoplamento**
- Separa lógica de negócio de efeitos colaterais
- Facilita manutenção e testes
- Permite evolução independente de componentes

### 2. **Integração**
- Comunicação entre bounded contexts
- Facilita implementação de microserviços
- Suporte a padrões de integração

### 3. **Auditoria e Rastreamento**
- Histórico completo de operações
- Facilita debugging e troubleshooting
- Suporte a compliance e auditoria

### 4. **Flexibilidade**
- Fácil adição de novos handlers
- Suporte a diferentes tipos de eventos
- Configuração flexível de processamento

## Considerações de Implementação

### 1. **Quando Usar Eventos de Domínio**
- **Efeitos colaterais**: Quando operações de negócio têm efeitos colaterais
- **Integração**: Para comunicação entre bounded contexts
- **Auditoria**: Para rastreamento de mudanças importantes
- **Notificações**: Para disparar alertas e notificações

### 2. **Boas Práticas**
- **Eventos imutáveis**: Eventos devem ser imutáveis após criação
- **Nomes descritivos**: Use nomes que descrevam o que aconteceu
- **Dados necessários**: Inclua apenas dados necessários no evento
- **Processamento assíncrono**: Processe eventos de forma assíncrona quando possível

### 3. **Performance**
- **Processamento em lote**: Processe múltiplos eventos juntos
- **Limpeza de eventos**: Limpe eventos após processamento
- **Ordenação**: Mantenha ordem dos eventos para consistência

### 4. **Tratamento de Erros**
- **Retry logic**: Implemente lógica de retry para eventos falhados
- **Dead letter queue**: Use fila de mensagens mortas para eventos problemáticos
- **Logging**: Registre falhas para debugging

## Dependências

Esta pasta depende de:
- **System.Threading**: Para `Interlocked.Increment`
- **Core.Libraries.Domain.Entities**: Para `EntityList<T>`

## Exemplos de Casos de Uso

### 1. **E-commerce - Processamento de Pedido**
```csharp
public class Pedido : Aggregate<EntityId<Guid>>
{
    public void ProcessarPagamento()
    {
        // Lógica de processamento
        RegisterEvent(new PagamentoProcessadoEvent(Id, Valor, MetodoPagamento));
    }
    
    public void EnviarParaFulfillment()
    {
        RegisterEvent(new PedidoEnviadoParaFulfillmentEvent(Id, Itens));
    }
}
```

### 2. **Sistema de Notificações**
```csharp
public class Usuario : Aggregate<EntityId<int>>
{
    public void AtualizarPerfil(string novoNome)
    {
        var nomeAnterior = Nome;
        Nome = novoNome;
        
        RegisterEvent(new PerfilAtualizadoEvent(Id, nomeAnterior, novoNome));
    }
}
```

### 3. **Auditoria de Sistema**
```csharp
public class ContaBancaria : Aggregate<EntityId<Guid>>
{
    public void RealizarSaque(decimal valor)
    {
        if (Saldo < valor)
            throw new SaldoInsuficienteException();
            
        Saldo -= valor;
        
        RegisterEvent(new SaqueRealizadoEvent(Id, valor, Saldo));
    }
}
```

## Próximos Passos

Para completar a implementação, considere:
1. **Event Handlers**: Implementar handlers específicos para cada tipo de evento
2. **Event Store**: Persistir eventos para auditoria e replay
3. **Event Sourcing**: Usar eventos como fonte única da verdade
4. **Message Queue**: Integrar com sistemas de mensageria
5. **Testes**: Criar testes unitários para eventos e handlers
6. **Monitoramento**: Implementar métricas e alertas para eventos
