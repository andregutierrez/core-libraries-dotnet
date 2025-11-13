# Docker Setup for People API

Este documento descreve como executar o projeto People usando Docker Compose.

## Pré-requisitos

- Docker Desktop ou Docker Engine instalado
- Docker Compose v3.8 ou superior
- .NET 9.0 SDK (para executar migrations localmente, se necessário)
- EF Core Tools instalado globalmente (para executar migrations localmente):
  ```bash
  dotnet tool install --global dotnet-ef --version 9.0.0
  export PATH="$PATH:$HOME/.dotnet/tools"
  ```
  
  Ou execute o script de instalação:
  ```bash
  chmod +x install-ef-tools.sh
  ./install-ef-tools.sh
  ```

## Estrutura

O `docker-compose.yml` inclui:

1. **PostgreSQL**: Banco de dados PostgreSQL 16
2. **Migrations**: Serviço para executar migrations do EF Core
3. **API**: Aplicação ASP.NET Core

## Como Usar

### 1. Iniciar todos os serviços (incluindo migrations)

```bash
docker-compose up --build
```

### 2. Iniciar apenas o banco de dados e a API (sem migrations automáticas)

```bash
docker-compose up postgres api
```

### 3. Executar migrations manualmente

```bash
# Opção 1: Usando o serviço de migrations
docker-compose --profile migrations up migrations

# Opção 2: Executar migrations localmente (requer .NET SDK)
./run-migrations.sh

# Opção 3: Usando dotnet ef diretamente
dotnet ef database update \
  --project example/People.Infra.Data/People.Infra.Data.csproj \
  --startup-project example/People.Api/People.Api.csproj \
  --context PeopleDbContext
```

### 4. Parar todos os serviços

```bash
docker-compose down
```

### 5. Parar e remover volumes (limpar dados do banco)

```bash
docker-compose down -v
```

## Variáveis de Ambiente

As variáveis de ambiente podem ser configuradas no `docker-compose.yml` ou através de um arquivo `.env`:

```env
POSTGRES_DB=PeopleDb
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
ASPNETCORE_ENVIRONMENT=Development
```

## Portas

- **API**: http://localhost:8080
- **PostgreSQL**: localhost:5432

## Connection String

A connection string padrão é:

```
Host=postgres;Port=5432;Database=PeopleDb;Username=postgres;Password=postgres;Pooling=true;
```

## Troubleshooting

### Verificar logs

```bash
# Logs de todos os serviços
docker-compose logs

# Logs de um serviço específico
docker-compose logs api
docker-compose logs postgres
docker-compose logs migrations
```

### Verificar status dos containers

```bash
docker-compose ps
```

### Acessar o banco de dados

```bash
docker-compose exec postgres psql -U postgres -d PeopleDb
```

### Rebuild completo

```bash
docker-compose down -v
docker-compose build --no-cache
docker-compose up
```

