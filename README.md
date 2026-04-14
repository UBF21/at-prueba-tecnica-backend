# AT — Prueba Técnica Fullstack Senior

## 🎯 Objetivo

Sistema de gestión de pedidos con autenticación JWT, validaciones, resiliencia y arquitectura limpia.

**Stack:** .NET 9 | React 18 | SQL Server | Clean Architecture | CQRS | JWT

---

## ⚡ Inicio Rápido en 5 Pasos

```bash
# 1. Navegar al directorio
cd ~/RiderProjects/at-prueba-tecnica-backend

# 2. Levantar backend y BD con Docker
docker-compose up -d --build

# 3. Esperar 15 segundos a que se inicialice
sleep 15

# 4. Hacer login desde terminal
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@retopedidos.com","password":"Admin123!"}'

# 5. O abrir Swagger: http://localhost:5001/swagger
```

✅ **¡Listo!** El backend está corriendo en `http://localhost:5001`

---

## 🏗️ Estructura

```
at-prueba-tecnica-backend/
├── at-prueba-tecnica-backend.Domain/           (Entidades, interfaces)
├── at-prueba-tecnica-backend.Application/      (CQRS, validators, comportamientos)
├── at-prueba-tecnica-backend.Infrastructure/   (EF Core, repositorios, JWT, BD)
└── at-prueba-tecnica-backend.Api/              (Controllers, middlewares, config)
```

---

## 📋 Requisitos Previos

- **.NET 9 SDK** — Descargar de https://dotnet.microsoft.com/download
- **Docker & Docker Compose** — Para SQL Server
- **Node.js 20+** — Para frontend (en repo separado)

---

## 🚀 Inicio Rápido

### Opción 1: Docker Compose (Recomendado para desarrollo)

```bash
cd ~/RiderProjects/at-prueba-tecnica-backend

# Crear archivo .env basado en .env.example (si no existe)
cp .env.example .env

# Levantar todos los servicios (SQL Server + Backend)
docker-compose down && docker-compose up -d --build

# Ver logs del backend
docker-compose logs -f backend

# Esperar ~15 segundos a que se inicialice la BD
```

**Servicios disponibles:**
- 🔵 **Backend API**: http://localhost:5001
- 📊 **Swagger UI**: http://localhost:5001/swagger
- 🗄️ **SQL Server**: localhost:1433 (usuario: `sa`, password: `SqlServer123!`)

### Opción 2: Ejecución Local (requiere SQL Server corriendo)

```bash
cd ~/RiderProjects/at-prueba-tecnica-backend

# 1. Restaurar dependencias
dotnet restore

# 2. Asegurar que SQL Server está corriendo
# (opcionalmente, levanta solo SQL Server: docker-compose up -d sqlserver)

# 3. Ejecutar API
dotnet run --project at-prueba-tecnica-backend.Api
```

API disponible en: `http://localhost:5000`

---

## 🔐 Credenciales por Defecto

```
Email:    admin@retopedidos.com
Password: Admin123!
```

---

## 📦 Librerías Clave

| Librería | Versión | Uso |
|---|---|---|
| `Vali-Mediator` | 2.0.1 | CQRS + `Result<T>` |
| `Vali-Validation` | 2.0.1 | Validators fluidos |
| `Vali-Mediator.Resilience` | 1.0.1 | Circuit Breaker, Retry, Timeout |
| `Vali-Flow.Core` | 2.0.1 | Query builder expresiones |
| `Vali-Flow` | 1.1.0 | EF Core evaluator |

---

## 🗄️ Base de Datos

### Tablas

- **Usuarios** — Autenticación con BCrypt
- **Pedidos** — CRUD completo con soft-delete

### Migraciones

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api

# Aplicar migraciones
dotnet ef database update \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api

# Generar script SQL
dotnet ef migrations script \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api \
  --output scripts/migration.sql \
  --idempotent
```

---

## 🧪 Testing

### 1️⃣ Login desde Terminal

```bash
# Obtener JWT token
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@retopedidos.com","password":"Admin123!"}' | jq .

# Respuesta esperada:
# {
#   "success": true,
#   "data": {
#     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
#     "expiresIn": 3600,
#     ...
#   }
# }
```

### 2️⃣ Usar JWT en siguientes requests

```bash
# Guardar el token
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Listar órdenes (requiere JWT)
curl -X GET http://localhost:5001/api/orders?page=1&pageSize=10 \
  -H "Authorization: Bearer $TOKEN" | jq .

# Crear orden
curl -X POST http://localhost:5001/api/orders \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"orderNumber":"ORD-001","customerId":1}' | jq .
```

### 3️⃣ Scalar Docs (recomendado para desarrollo)

1. Abrir `http://localhost:5001/scalar/v1`
2. Click en `POST /api/auth/login`
3. Escribir credenciales:
   ```json
   {
     "email": "admin@retopedidos.com",
     "password": "Admin123!"
   }
   ```
4. Click **Send** → copiar el `token` de la respuesta
5. Usar el token en próximos requests (header `Authorization: Bearer <token>`)

### 4️⃣ Validaciones clave

```bash
# OrderNumber único
POST /api/orders { "orderNumber": "DUP-001" }  # 409 Conflict (si existe)

# Soft delete
DELETE /api/orders/1  # 200 OK, marca como eliminado
GET /api/orders       # No retorna registros eliminados
```

---

## 🐳 Docker Compose

### Levantar todos los servicios

```bash
cd ~/RiderProjects/at-prueba-tecnica-backend

# Reconstruir imágenes y levantar
docker-compose up -d --build

# Ver estado de los contenedores
docker-compose ps

# Ver logs
docker-compose logs -f backend      # Logs del backend
docker-compose logs -f sqlserver    # Logs de SQL Server

# Detener servicios
docker-compose down

# Detener y eliminar volúmenes (limpia la BD)
docker-compose down -v
```

**Configuración:**
- **sqlserver** → localhost:1433 (sa / SqlServer123!)
- **backend** → localhost:5001 (http)
- **CORS** → Permite localhost:5173 y localhost:5174 (frontend)
- **Migrations** → Automáticas al iniciar (EnsureCreatedAsync)

---

## 📝 Endpoints Principales

### Autenticación (sin JWT)
- `POST /api/auth/login` — Obtener JWT token

### Órdenes (requiere JWT)
- `GET /api/orders?page=1&pageSize=10&status=Pending` — Listar órdenes
- `GET /api/orders/{id}` — Obtener orden por ID
- `POST /api/orders` — Crear orden
- `PUT /api/orders/{id}` — Actualizar orden
- `DELETE /api/orders/{id}` — Eliminar orden (soft delete)
- `POST /api/orders/{id}/items` — Agregar item a orden
- `PUT /api/orders/{id}/status` — Cambiar estado de orden

### Productos (requiere JWT)
- `GET /api/products?page=1&pageSize=10` — Listar productos
- `GET /api/products/{id}` — Obtener producto por ID
- `POST /api/products` — Crear producto
- `PUT /api/products/{id}` — Actualizar producto
- `DELETE /api/products/{id}` — Eliminar producto (soft delete)

---

## 🎨 Frontend

Repositorio separado: `~/Documents/Proyectos/at-prueba-tecnica-frontend/`

```bash
cd ~/Documents/Proyectos/at-prueba-tecnica-frontend
npm install
npm run dev
```

---

## 📊 Arquitectura

### Clean Architecture (4 capas)

```
┌─────────────────────────────────────┐
│         API (Controllers)            │  ← REST endpoints
├─────────────────────────────────────┤
│     Application (CQRS, Validation)   │  ← Business logic
├─────────────────────────────────────┤
│  Infrastructure (EF Core, JWT, BD)   │  ← Data access
├─────────────────────────────────────┤
│    Domain (Entities, Interfaces)     │  ← Core rules
└─────────────────────────────────────┘
```

### Patrón CQRS

- **Commands** — Cambios de estado (Create, Update, Delete)
- **Queries** — Solo lectura (Get, Search)
- **Handlers** — Lógica de cada comando/query
- **Validators** — Validación fluida pre-ejecución

### Resiliencia

- **Circuit Breaker** — Evita cascadas de fallos
- **Retry** — Reintentos exponenciales
- **Timeout** — Límite de tiempo de ejecución
- **Bulkhead** — Control de concurrencia

---

## 🔄 Flujo típico de un comando

```
Request HTTP
    ↓
ValidationBehavior (Vali-Validation)
    ↓
ResilienceBehavior (Vali-Mediator.Resilience)
    ↓
LoggingBehavior (custom)
    ↓
Handler (lógica de negocio)
    ↓
Repository (Vali-Flow EvaluateAsync)
    ↓
DbContext / SQL Server
    ↓
Result<T> → HTTP Status Code
```

---

## ⚙️ Configuración

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=RetoPedidos;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "your-secret-key-at-least-32-chars",
    "Issuer": "RetoPedidos",
    "Audience": "RetoPedidosClient",
    "ExpiresInMinutes": 60
  }
}
```

---

## 🐛 Troubleshooting

### Backend no arranca en Docker
```bash
# Limpieza completa
docker system prune -af --volumes

# Rebuildar desde cero
docker-compose down -v && docker-compose up -d --build
```

### "Invalid object name 'Users'" en login
→ Las tablas se crean automáticamente al iniciar (EnsureCreatedAsync)  
→ Esperar ~15 segundos a que la BD se inicialice  
→ Revisar logs: `docker-compose logs backend`

### CORS error en frontend
→ Backend permite: `http://localhost:5173` y `http://localhost:5174`  
→ Verificar en `Program.cs` línea 60: `policy.WithOrigins(...)`

### "dotnet command not found"
→ Instalar .NET 9 SDK desde https://dotnet.microsoft.com/download

### SQL Server no conecta (local)
→ Verificar que Docker está corriendo: `docker ps`  
→ Chequear que el puerto 1433 está disponible

---

## 📖 Documentación

- 📚 **Scalar API Docs**: http://localhost:5001/scalar/v1
- 🔍 **OpenAPI Spec**: http://localhost:5001/openapi/v1.json
- 📋 [Migraciones SQL](./scripts/migration.sql)
- 🏗️ [Estructura del Proyecto](./PROJECT_STRUCTURE.md)

---

**Autor:** Felipe Rafael Montenegro Morriberon (UBF21)  
**Fecha:** 2026-04-13
