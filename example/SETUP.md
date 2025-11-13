# Setup Guide - People API

Este guia explica como configurar o ambiente de desenvolvimento para o projeto People API.

## Pré-requisitos

1. **.NET 9.0 SDK**
   ```bash
   dotnet --version
   # Deve retornar 9.0.x ou superior
   ```

2. **Docker e Docker Compose** (para executar o banco de dados)
   ```bash
   docker --version
   docker-compose --version
   ```

3. **EF Core Tools** (para gerenciar migrations)
   ```bash
   # Instalar globalmente
   dotnet tool install --global dotnet-ef --version 9.0.0
   
   # Adicionar ao PATH (adicione ao seu ~/.bashrc ou ~/.zshrc)
   export PATH="$PATH:$HOME/.dotnet/tools"
   
   # Verificar instalação
   dotnet ef --version
   ```

   Ou use o script fornecido:
   ```bash
   chmod +x install-ef-tools.sh
   ./install-ef-tools.sh
   ```

## Configuração Inicial

### 1. Instalar EF Core Tools

Se você ainda não tem o `dotnet-ef` instalado:

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
export PATH="$PATH:$HOME/.dotnet/tools"
```

**Importante**: Se você receber "command not found" após instalar, adicione esta linha ao seu `~/.bashrc` ou `~/.zshrc`:

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

Depois, recarregue o shell:
```bash
source ~/.bashrc  # ou source ~/.zshrc
```

### 2. Iniciar o Banco de Dados

```bash
cd example
docker-compose up postgres -d
```

Aguarde alguns segundos para o PostgreSQL inicializar.

### 3. Criar a Primeira Migration

```bash
cd example
dotnet ef migrations add InitialCreate \
  --project People.Infra.Data/People.Infra.Data.csproj \
  --startup-project People.Api/People.Api.csproj \
  --context PeopleDbContext
```

### 4. Aplicar Migrations

**Opção 1: Usando Docker Compose (recomendado)**
```bash
cd example
docker-compose --profile migrations up migrations
```

**Opção 2: Localmente (requer PostgreSQL rodando)**
```bash
cd example
dotnet ef database update \
  --project People.Infra.Data/People.Infra.Data.csproj \
  --startup-project People.Api/People.Api.csproj \
  --context PeopleDbContext
```

**Opção 3: Usando o script**
```bash
cd example
chmod +x run-migrations.sh
./run-migrations.sh
```

### 5. Executar a API

**Opção 1: Usando Docker Compose**
```bash
cd example
docker-compose up api
```

**Opção 2: Localmente**
```bash
cd example/People.Api
dotnet run
```

A API estará disponível em:
- HTTP: http://localhost:8080
- Swagger: http://localhost:8080/swagger
- Health Check: http://localhost:8080/health

## Comandos Úteis

### Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration \
  --project example/People.Infra.Data/People.Infra.Data.csproj \
  --startup-project example/People.Api/People.Api.csproj \
  --context PeopleDbContext

# Aplicar migrations
dotnet ef database update \
  --project example/People.Infra.Data/People.Infra.Data.csproj \
  --startup-project example/People.Api/People.Api.csproj \
  --context PeopleDbContext

# Remover última migration (antes de aplicar)
dotnet ef migrations remove \
  --project example/People.Infra.Data/People.Infra.Data.csproj \
  --startup-project example/People.Api/People.Api.csproj \
  --context PeopleDbContext

# Listar migrations
dotnet ef migrations list \
  --project example/People.Infra.Data/People.Infra.Data.csproj \
  --startup-project example/People.Api/People.Api.csproj \
  --context PeopleDbContext
```

### Docker

```bash
# Iniciar todos os serviços
docker-compose up

# Iniciar em background
docker-compose up -d

# Parar serviços
docker-compose down

# Parar e remover volumes (limpar banco)
docker-compose down -v

# Ver logs
docker-compose logs -f api
docker-compose logs -f postgres

# Rebuild
docker-compose build --no-cache
```

## Troubleshooting

### Problema: "dotnet-ef: command not found"

**Solução:**
1. Instale a ferramenta:
   ```bash
   dotnet tool install --global dotnet-ef --version 9.0.0
   ```

2. Adicione ao PATH:
   ```bash
   export PATH="$PATH:$HOME/.dotnet/tools"
   ```

3. Adicione permanentemente ao seu `~/.bashrc` ou `~/.zshrc`:
   ```bash
   echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
   source ~/.bashrc
   ```

### Problema: "Connection refused" ao conectar ao PostgreSQL

**Solução:**
1. Verifique se o container está rodando:
   ```bash
   docker-compose ps
   ```

2. Verifique os logs:
   ```bash
   docker-compose logs postgres
   ```

3. Aguarde alguns segundos após iniciar o container para o PostgreSQL inicializar completamente.

### Problema: "Migration already applied"

**Solução:**
Se você precisa recriar o banco do zero:
```bash
docker-compose down -v
docker-compose up postgres -d
# Aguarde alguns segundos
dotnet ef database update ...
```

## Estrutura de Pastas

```
example/
├── People.Api/              # API REST
├── People.Application/      # Camada de aplicação (Use Cases)
├── People.Domain/           # Camada de domínio
├── People.Infra.Data/       # Camada de infraestrutura (EF Core)
├── People.IoC/              # Injeção de dependência
├── docker-compose.yml       # Configuração Docker
├── install-ef-tools.sh      # Script de instalação EF Tools
└── run-migrations.sh        # Script para executar migrations
```

