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
docker compose up -d sqlserver
# Esperar 30s a que inicie

# Verificar que está corriendo
docker ps | grep sqlserver
# Debe mostrar: at_sqlserver ... Up
```

### 6️⃣ Verificar conexión a BD

```bash
# Desde el terminal, conectar a SQL Server
docker exec -it at_sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "RetoPedidos_Pass123!" -Q "SELECT 'Connection OK'"
# Debe mostrar: Connection OK
```

---

## 🛠️ Ahora ya estás listo para implementar

Siguiendo el plan en `/claude/plans/delightful-sauteeing-goblet.md`:

1. **FASE 1: Domain Layer** → Entidades, interfaces, enums
2. **FASE 2: Application Layer** → Commands, queries, validators, behaviors
3. **FASE 3: Infrastructure Layer** → DbContext, repositories, JWT
4. **FASE 4: API Layer** → Controllers, middlewares, Program.cs

Luego aplicar migraciones:

```bash
# Crear migración
dotnet ef migrations add InitialCreate \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api

# Aplicar
dotnet ef database update \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api
```

Finalmente:

```bash
dotnet run --project at-prueba-tecnica-backend.Api
# API en https://localhost:5000
# Swagger en https://localhost:5000/swagger
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

- Plan completo: `/Users/feliperafaelmontenegro/.claude/plans/delightful-sauteeing-goblet.md`
- README: `./README.md`
- Este setup: `./SETUP.md`

---

**¡Listo para comenzar!** 🚀
