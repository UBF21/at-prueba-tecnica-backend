#!/bin/bash

# Script para verificar que todos los prerequisites están instalados

echo "🔍 Verificando prerequisites para AT — Prueba Técnica..."
echo ""

MISSING=0

# Verificar .NET 9
echo -n "✓ .NET 9 SDK... "
if command -v dotnet &> /dev/null; then
    VERSION=$(dotnet --version)
    if [[ $VERSION == 9* ]]; then
        echo "✅ $VERSION"
    else
        echo "❌ (Tienes $VERSION, necesitas 9.x)"
        MISSING=$((MISSING + 1))
    fi
else
    echo "❌ NO INSTALADO"
    echo "   Descargar de: https://dotnet.microsoft.com/download"
    MISSING=$((MISSING + 1))
fi

# Verificar Docker
echo -n "✓ Docker... "
if command -v docker &> /dev/null; then
    DOCKER_VERSION=$(docker --version)
    echo "✅ $DOCKER_VERSION"
else
    echo "❌ NO INSTALADO"
    echo "   Descargar de: https://www.docker.com/products/docker-desktop"
    MISSING=$((MISSING + 1))
fi

# Verificar Docker Compose
echo -n "✓ Docker Compose... "
if command -v docker compose &> /dev/null; then
    COMPOSE_VERSION=$(docker compose version)
    echo "✅ $COMPOSE_VERSION"
else
    echo "❌ NO INSTALADO"
    MISSING=$((MISSING + 1))
fi

# Verificar archivos clave
echo ""
echo "📁 Verificando archivos del proyecto..."
echo -n "  .env... "
if [ -f ".env" ]; then
    echo "✅"
else
    echo "❌ (crear: cp .env.example .env)"
    MISSING=$((MISSING + 1))
fi

echo -n "  docker-compose.yml... "
if [ -f "docker-compose.yml" ]; then
    echo "✅"
else
    echo "❌"
    MISSING=$((MISSING + 1))
fi

echo -n "  init-project.sh... "
if [ -f "init-project.sh" ]; then
    echo "✅"
else
    echo "❌"
    MISSING=$((MISSING + 1))
fi

echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

if [ $MISSING -eq 0 ]; then
    echo "✅ ¡Todos los prerequisites están OK!"
    echo ""
    echo "📋 Próximos pasos:"
    echo ""
    echo "1. Ejecutar init-project.sh:"
    echo "   bash init-project.sh"
    echo ""
    echo "2. Esperar a que termine (instala NuGet packages)"
    echo ""
    echo "3. Levantar SQL Server:"
    echo "   docker compose up -d sqlserver"
    echo ""
    echo "4. Ver SETUP.md para instrucciones completas"
    echo ""
else
    echo "❌ Faltan $MISSING cosas"
    echo ""
    echo "📖 Ver SETUP.md para instrucciones detalladas"
    echo ""
    exit 1
fi
