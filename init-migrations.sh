#!/bin/bash
set -e

# Esperar a que SQL Server esté listo
echo "Esperando SQL Server..."
for i in {1..30}; do
  if /opt/mssql-tools18/bin/sqlcmd -S sqlserver,1433 -U sa -P "$DB_PASSWORD" -Q "SELECT 1" -No -C 2>/dev/null; then
    echo "SQL Server está listo"
    break
  fi
  echo "Intento $i/30..."
  sleep 2
done

# Generar migraciones si no existen
if [ ! -d "/src/at-prueba-tecnica-backend.Infrastructure/Persistence/Migrations" ]; then
  echo "Generando migraciones iniciales..."
  cd /src
  dotnet ef migrations add InitialCreate \
    --project at-prueba-tecnica-backend.Infrastructure \
    --startup-project at-prueba-tecnica-backend.Api \
    --context AppDbContext \
    --output-dir Persistence/Migrations || true
fi

# Aplicar migraciones
echo "Aplicando migraciones..."
cd /src
dotnet ef database update \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api || true

echo "Migraciones completadas"
