# Migraciones de Entity Framework Core

## Prerequisitos

- SQL Server debe estar ejecutándose en `localhost,1433` con contraseña `RetoPedidos_Pass123!`
- .NET 9 SDK instalado

## Generar e Aplicar Migraciones

### 1. Crear la migración inicial (desde la raíz del proyecto)

```bash
dotnet ef migrations add InitialCreate \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api \
  --context AppDbContext \
  --output-dir Persistence/Migrations
```

### 2. Aplicar la migración a la base de datos

```bash
dotnet ef database update \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api \
  --context AppDbContext
```

### 3. (Opcional) Generar script SQL idempotente

```bash
dotnet ef migrations script \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api \
  --context AppDbContext \
  --output scripts/migration.sql \
  --idempotent
```

## Verificar el estado de migraciones

```bash
dotnet ef migrations list \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api
```

## Revertir a una migración anterior

```bash
dotnet ef database update <MigrationName> \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api
```

## Resolver conflictos de migraciones

Si hay conflictos entre migraciones locales, ejecutar:

```bash
dotnet ef migrations remove \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api

# Luego volver a crear:
dotnet ef migrations add InitialCreate ...
```
