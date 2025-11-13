# 1. Iniciar tudo (banco + API)
cd example
docker-compose up --build

# 2. Executar migrations
docker-compose --profile migrations up migrations

# 3. Acessar a API
# http://localhost:8080
# Swagger: http://localhost:8080/swagger

# 4. Parar tudo
docker-compose down

# 5. Limpar volumes (remove dados do banco)
docker-compose down -v