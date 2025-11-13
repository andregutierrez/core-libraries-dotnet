#!/bin/bash

# Script to run EF Core migrations
# Usage: ./run-migrations.sh

set -e

echo "=========================================="
echo "Running EF Core Migrations"
echo "=========================================="

# Check if PostgreSQL is ready
echo "Waiting for PostgreSQL to be ready..."
until pg_isready -h localhost -p 5432 -U postgres; do
  echo "PostgreSQL is unavailable - sleeping"
  sleep 2
done

echo "PostgreSQL is up - executing migrations..."

# Run migrations
dotnet ef database update \
  --project example/People.Infra.Data/People.Infra.Data.csproj \
  --startup-project example/People.Api/People.Api.csproj \
  --context PeopleDbContext \
  --verbose

echo "=========================================="
echo "Migrations completed successfully!"
echo "=========================================="

