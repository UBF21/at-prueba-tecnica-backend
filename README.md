# AT — Prueba Técnica Fullstack Senior

## 🎯 Objetivo

Sistema de gestión de pedidos con autenticación JWT, validaciones, resiliencia y arquitectura limpia.

**Stack:** .NET 9 | React 18 | SQL Server | Clean Architecture | CQRS | JWT

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

### 1. Instalar dependencias .NET

```bash
cd ~/RiderProjects/at-prueba-tecnica-backend

# Restaurar NuGet packages
dotnet restore
```

### 2. Levantar SQL Server con Docker

```bash
# Crear archivo .env basado en .env.example
cp .env.example .env

# Levantar SQL Server
docker compose up -d sqlserver
```

### 3. Aplicar migraciones EF Core

```bash
dotnet ef database update \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api
```

### 4. Ejecutar API

```bash
dotnet run --project at-prueba-tecnica-backend.Api
```

API disponible en: `https://localhost:5000`  
Swagger UI: `https://localhost:5000/swagger`

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

### Swagger (recomendado para desarrollo)

1. Ir a `https://localhost:5000/swagger`
2. `POST /auth/login` con credenciales por defecto → obtener JWT
3. Click en botón **Authorize** → pegar Bearer token
4. Probar endpoints con autenticación

### Validaciones clave

```bash
# Total debe ser > 0
POST /api/pedidos { "total": 0 }  # 400 Bad Request

# NumeroPedido único
POST /api/pedidos { "numeroPedido": "DUP-001" }  # 409 Conflict (si existe)

# Soft delete
DELETE /api/pedidos/1  # 200 OK, marca como eliminado
GET /api/pedidos       # No retorna registros eliminados
```

---

## 🐳 Docker (Producción)

```bash
# Construir y levantar todo
docker compose up --build

# Servicios:
# - sqlserver:   localhost:1433
# - backend:     localhost:5000
# - frontend:    localhost:5173
```

---

## 📝 Endpoints Principales

### Autenticación
- `POST /auth/login` — Obtener JWT

### Pedidos (requiere JWT)
- `GET /api/pedidos?page=1&pageSize=10` — Listar pedidos
- `GET /api/pedidos/{id}` — Obtener pedido por ID
- `POST /api/pedidos` — Crear pedido
- `PUT /api/pedidos/{id}` — Actualizar pedido
- `DELETE /api/pedidos/{id}` — Eliminar pedido (soft delete)

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

### "dotnet command not found"
→ Instalar .NET 9 SDK desde https://dotnet.microsoft.com/download

### SQL Server no conecta
→ Verificar que Docker está corriendo: `docker ps`  
→ Chequear `.env` tiene la contraseña correcta

### Migrations fallan
→ Eliminar `/Migrations` y crear de nuevo  
→ Asegurar que `appsettings.json` tiene la conexión correcta

---

## 📖 Recursos

- [Plan de implementación](./docs/PLAN.md)
- [Migraciones SQL](./scripts/migration.sql)
- [Swagger/OpenAPI](https://localhost:5000/swagger)

---

**Autor:** Felipe Rafael Montenegro Morriberon (UBF21)  
**Fecha:** 2026-04-13
