#!/bin/bash

# Script para inicializar la solución .NET 9
# Ejecutar desde el directorio raíz: bash init-project.sh

set -e

echo "🚀 Inicializando solución .NET 9: at-prueba-tecnica-backend"
echo ""

# Verificar .NET 9
echo "✓ Verificando .NET 9..."
DOTNET_VERSION=$(dotnet --version)
echo "  .NET version: $DOTNET_VERSION"
echo ""

# Crear la solución
echo "📋 Creando solución..."
dotnet new sln -n at-prueba-tecnica-backend

echo ""
echo "📦 Creando proyectos..."

# Domain
dotnet new classlib -n at-prueba-tecnica-backend.Domain -f net9.0
echo "  ✓ Domain"

# Application
dotnet new classlib -n at-prueba-tecnica-backend.Application -f net9.0
echo "  ✓ Application"

# Infrastructure
dotnet new classlib -n at-prueba-tecnica-backend.Infrastructure -f net9.0
echo "  ✓ Infrastructure"

# Api
dotnet new webapi -n at-prueba-tecnica-backend.Api -f net9.0 --no-openapi
echo "  ✓ Api"

echo ""
echo "🔗 Agregando proyectos a la solución..."
dotnet sln at-prueba-tecnica-backend.sln add \
  at-prueba-tecnica-backend.Domain/at-prueba-tecnica-backend.Domain.csproj \
  at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj \
  at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj \
  at-prueba-tecnica-backend.Api/at-prueba-tecnica-backend.Api.csproj

echo ""
echo "🏗️  Agregando referencias entre capas..."

# Application → Domain
dotnet add at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj \
  reference at-prueba-tecnica-backend.Domain/at-prueba-tecnica-backend.Domain.csproj
echo "  ✓ Application → Domain"

# Infrastructure → Application
dotnet add at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj \
  reference at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj
echo "  ✓ Infrastructure → Application"

# Api → Infrastructure
dotnet add at-prueba-tecnica-backend.Api/at-prueba-tecnica-backend.Api.csproj \
  reference at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj
echo "  ✓ Api → Infrastructure"

echo ""
echo "📚 Instalando NuGet packages..."

# CQRS + Mediator
dotnet add at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj package Vali-Mediator --version 2.0.1
echo "  ✓ Vali-Mediator"

# Validation
dotnet add at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj package Vali-Validation --version 2.0.1
echo "  ✓ Vali-Validation"

# Validation + Mediator Integration
dotnet add at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj package Vali-Validation.ValiMediator --version 1.0.1
echo "  ✓ Vali-Validation.ValiMediator"

# Resilience (Circuit Breaker, Retry, etc)
dotnet add at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj package Vali-Mediator.Resilience --version 1.0.1
echo "  ✓ Vali-Mediator.Resilience"

# Mediator + AspNetCore
dotnet add at-prueba-tecnica-backend.Api/at-prueba-tecnica-backend.Api.csproj package Vali-Mediator.AspNetCore --version 1.0.1
echo "  ✓ Vali-Mediator.AspNetCore"

# Query builder expresiones
dotnet add at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj package Vali-Flow.Core --version 2.0.1
echo "  ✓ Vali-Flow.Core"

# EF Core Evaluator
dotnet add at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj package Vali-Flow --version 1.1.0
echo "  ✓ Vali-Flow"

# EF Core SQL Server
dotnet add at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
echo "  ✓ Microsoft.EntityFrameworkCore.SqlServer"

# EF Core Tools
dotnet add at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
echo "  ✓ Microsoft.EntityFrameworkCore.Tools"

# JWT
dotnet add at-prueba-tecnica-backend.Api/at-prueba-tecnica-backend.Api.csproj package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.0
echo "  ✓ Microsoft.AspNetCore.Authentication.JwtBearer"

# BCrypt para hashing de contraseñas
dotnet add at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj package BCrypt.Net-Next --version 4.0.3
echo "  ✓ BCrypt.Net-Next"

echo ""
echo "✅ ¡Solución creada exitosamente!"
echo ""
echo "📍 Próximos pasos:"
echo ""
echo "1. Restaurar dependencias:"
echo "   dotnet restore"
echo ""
echo "2. Verificar que compila:"
echo "   dotnet build"
echo ""
echo "3. Levantar SQL Server con Docker:"
echo "   docker compose up -d sqlserver"
echo ""
echo "4. Aplicar migraciones (después de implementar modelos):"
echo "   dotnet ef migrations add InitialCreate --project at-prueba-tecnica-backend.Infrastructure --startup-project at-prueba-tecnica-backend.Api"
echo "   dotnet ef database update --project at-prueba-tecnica-backend.Infrastructure --startup-project at-prueba-tecnica-backend.Api"
echo ""
echo "5. Ejecutar la API:"
echo "   dotnet run --project at-prueba-tecnica-backend.Api"
echo ""
