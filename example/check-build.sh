#!/bin/bash

# Script to check build errors
set -e

echo "=========================================="
echo "Checking Build Errors"
echo "=========================================="
echo ""

cd "$(dirname "$0")/.."

echo "1. Restoring dependencies..."
dotnet restore example/People.Api/People.Api.csproj 2>&1 | tail -20
echo ""

echo "2. Building People.Domain..."
dotnet build example/People.Domain/People.Domain.csproj --no-restore 2>&1 | grep -E "(error|warning CS|Build succeeded|Build FAILED)" || echo "No errors found"
echo ""

echo "3. Building People.Application..."
dotnet build example/People.Application/People.Application.csproj --no-restore 2>&1 | grep -E "(error|warning CS|Build succeeded|Build FAILED)" || echo "No errors found"
echo ""

echo "4. Building People.Infra.Data..."
dotnet build example/People.Infra.Data/People.Infra.Data.csproj --no-restore 2>&1 | grep -E "(error|warning CS|Build succeeded|Build FAILED)" || echo "No errors found"
echo ""

echo "5. Building People.IoC..."
dotnet build example/People.IoC/People.IoC.csproj --no-restore 2>&1 | grep -E "(error|warning CS|Build succeeded|Build FAILED)" || echo "No errors found"
echo ""

echo "6. Building People.Api..."
dotnet build example/People.Api/People.Api.csproj --no-restore 2>&1 | grep -E "(error|warning CS|Build succeeded|Build FAILED)" || echo "No errors found"
echo ""

echo "=========================================="
echo "Build check completed!"
echo "=========================================="

