# Setup — Guía Paso a Paso

## ✅ Checklist previo

- [ ] .NET 9 SDK instalado — Descargar de https://dotnet.microsoft.com/download
- [ ] Docker Desktop instalado y corriendo
- [ ] Terminal/CMD en `~/RiderProjects/at-prueba-tecnica-backend/`

---

## 📋 Pasos

### 1️⃣ Verificar .NET 9

```bash
dotnet --version
# Debe mostrar: 9.0.x o superior
```

### 2️⃣ Crear estructura .NET

```bash
bash init-project.sh
```

Esto crea:
- Solución `.sln`
- 4 proyectos (Domain, Application, Infrastructure, Api)
- Referencias entre capas
- Todos los NuGet packages necesarios

**O manual si prefieres:**

```bash
dotnet new sln -n at-prueba-tecnica-backend
dotnet new classlib -n at-prueba-tecnica-backend.Domain -f net9.0
dotnet new classlib -n at-prueba-tecnica-backend.Application -f net9.0
dotnet new classlib -n at-prueba-tecnica-backend.Infrastructure -f net9.0
dotnet new webapi -n at-prueba-tecnica-backend.Api -f net9.0 --no-openapi

# Agregar a solución
dotnet sln add **/*.csproj

# Agregar referencias
dotnet add Application/... reference Domain/...
dotnet add Infrastructure/... reference Application/...
dotnet add Api/... reference Infrastructure/...

# Instalar paquetes (ver init-project.sh para las versiones exactas)
```

### 3️⃣ Verificar que compila

```bash
dotnet build
# Debe compilar sin errores
```

### 4️⃣ Crear .env (nunca commitear)

```bash
cp .env.example .env
# Editar .env con tus valores (o dejar los defaults)
```

### 5️⃣ Levantar SQL Server

```bash
docker-compose up -d sqlserver
# Esperar 30s a que inicie

# Verificar que está corriendo
docker ps | grep reto_sqlserver
# Debe mostrar: reto_sqlserver ... Up
```

### 6️⃣ Verificar conexión a BD

```bash
# Desde el terminal, conectar a SQL Server
docker exec -it reto_sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "SqlServer123!" -Q "SELECT 'Connection OK'"
# Debe mostrar: Connection OK
```

---

## 🛠️ El proyecto ya está implementado

**No necesitas crear archivos ni ejecutar migraciones.** El proyecto incluye:

- ✅ Todas las entidades, commands, queries, handlers
- ✅ Validación fluida con Vali-Validation
- ✅ Persistencia con EF Core y Vali-Flow
- ✅ Base de datos se recrea automáticamente en cada inicio (sin migraciones)
- ✅ Autenticación JWT

Solo ejecuta:

```bash
# Opción 1: Con Docker Compose (recomendado)
docker-compose up -d --build
# Esperar ~15 segundos
# API en http://localhost:5001
# Scalar en http://localhost:5001/scalar

# Opción 2: Local con .NET CLI
docker-compose up -d sqlserver  # Solo BD
dotnet run --project at-prueba-tecnica-backend.Api
# API en http://localhost:5000
# Scalar en http://localhost:5000/scalar
```

---

## 🐛 Si algo falla

### "command not found: dotnet"
→ .NET no está instalado o no está en PATH  
→ Instalar de https://dotnet.microsoft.com/download

### "SQL Server no conecta"
→ `docker ps` — verificar que `at_sqlserver` está corriendo  
→ `docker logs at_sqlserver` — ver logs del contenedor

### "Compilation errors"
→ `dotnet clean` → `dotnet build`  
→ Eliminar `obj/` y `bin/` si persiste

### "EF Core migrations fallan"
→ Asegurar que `appsettings.json` tiene la conexión correcta  
→ Verificar `AppDbContextFactory` está en Infrastructure  
→ Ejecutar desde el directorio raíz de la solución

---

## 📖 Documentación

- README: `./README.md` — Arquitectura general y patrones
- PROJECT_STRUCTURE: `./PROJECT_STRUCTURE.md` — Estructura de directorios
- SETUP: `./SETUP.md` — Este archivo
- DOCKER_SETUP: `./DOCKER_SETUP.md` — Guía Docker Compose

---

**¡Todo listo para usar!** 🚀
