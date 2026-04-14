# 🎯 AT — Prueba Técnica Fullstack Senior

**Sistema CQRS de Gestión de Pedidos, Clientes y Productos**  
Arquitectura limpia, .NET 9, SQL Server, patrones enterprise con librerías custom UBF21.

---

## 📋 Sobre Esta Prueba Técnica

### Objetivo
Demostrar expertise senior en:
- ✅ **Arquitectura limpia** (4 capas independientes)
- ✅ **CQRS** (separación Commands/Queries)
- ✅ **Resiliencia** (Circuit Breaker, Retry, Timeout)
- ✅ **Validación** (fluida y composable)
- ✅ **Persistencia** (EF Core, soft-delete, migrations)
- ✅ **Autenticación** (JWT, BCrypt)
- ✅ **API profesional** (REST, OpenAPI/Swagger)

### Stack Tecnológico

| Componente | Versión | Rol |
|---|---|---|
| **.NET** | 9.0 | Framework |
| **SQL Server** | 2022 | Base de datos |
| **Entity Framework Core** | 9.0 | ORM |
| **Vali-Mediator** | 2.0.1 | ⭐ CQRS + Result<T> |
| **Vali-Validation** | 2.0.1 | ⭐ Validators fluidos |
| **Vali-Mediator.Resilience** | 1.0.1 | ⭐ Políticas Polly |
| **Vali-Flow.Core** | 2.0.1 | ⭐ Query builders |
| **Vali-Flow** | 1.1.0 | ⭐ EF evaluator |
| **System.IdentityModel.Tokens.Jwt** | 7.x | JWT tokens |
| **BCrypt.Net-Core** | 1.x | Password hashing |

**⭐ Tecnologías de Autoría Propia (Felipe Rafael Montenegro Morriberon — UBF21):**
- **Vali-Mediator**: Implementación de patrón Mediator + Result<T> type
  - In-process command/query bus
  - Pipeline behaviors (logging, validation, resilience)
  - Handlers tipados
  
- **Vali-Validation**: Sistema de validación fluido
  - API builder para reglas
  - Validadores composables
  - Async validators
  - Mensajes customizables
  
- **Vali-Mediator.Resilience**: Políticas de resiliencia
  - Circuit Breaker (evita cascadas)
  - Retry con backoff exponencial
  - Timeout enforcement
  - Bulkhead isolation
  
- **Vali-Flow.Core**: Builder de queries LINQ
  - Expression trees type-safe
  - Filters composables
  - Predicates y specifications
  
- **Vali-Flow**: Evaluador para EF Core
  - Traduce Vali-Flow queries a SQL
  - Lazy evaluation
  - Query optimization

---

## 🏗️ Arquitectura

### Clean Architecture (4 Capas)

```
┌─────────────────────────────────────────┐
│  API (Controllers, Middlewares)         │  ← REST endpoints, HTTP
├─────────────────────────────────────────┤
│  Application (CQRS, Behaviors)          │  ← Lógica de negocio
├─────────────────────────────────────────┤
│  Infrastructure (EF Core, JWT, BD)      │  ← Persistencia, externos
├─────────────────────────────────────────┤
│  Domain (Entities, Interfaces, Rules)   │  ← Core, sin dependencias
└─────────────────────────────────────────┘
```

**Principios:**
- 🔵 **Domain** — Sin dependencias externas
- 🟢 **Application** — Depende de Domain, no de Infrastructure
- 🟡 **Infrastructure** — Depende de Domain + Application
- 🔴 **API** — Punto de entrada, depende de todo

### CQRS Pattern

```
Request HTTP
    ↓
┌─────────────────────────────────┐
│  Router → Controller            │
│  (inyecta IMediator)            │
└─────────────────────────────────┘
    ↓
┌─────────────────────────────────┐
│  Command / Query                │
│  (DTO request)                  │
└─────────────────────────────────┘
    ↓
┌──────────────────────────────────────────────┐
│  Mediator Pipeline Behaviors:                │
│  1. ValidationBehavior      ← Fluent rules   │
│  2. LoggingBehavior         ← Auditoría      │
│  3. ResilienceBehavior      ← Polly          │
└──────────────────────────────────────────────┘
    ↓
┌─────────────────────────────────┐
│  Handler<TCommand/Query>        │
│  (lógica de negocio)            │
└─────────────────────────────────┘
    ↓
┌─────────────────────────────────┐
│  Repository (Vali-Flow)         │
│  (abstracción de datos)         │
└─────────────────────────────────┘
    ↓
┌─────────────────────────────────┐
│  DbContext + SQL Server         │
│  (persistencia real)            │
└─────────────────────────────────┘
    ↓
┌─────────────────────────────────┐
│  Result<T>                      │
│  (Success: 200, Error: 4xx/5xx) │
└─────────────────────────────────┘
    ↓
Response HTTP
```

### Folder Structure

```
at-prueba-tecnica-backend/
│
├── at-prueba-tecnica-backend.Domain/
│   ├── Entities/
│   │   ├── Usuario.cs              ← Entidad (usuario + autenticación)
│   │   ├── Pedido.cs               ← Entidad (order + items)
│   │   ├── Cliente.cs              ← Entidad (customer)
│   │   └── Producto.cs             ← Entidad (product)
│   ├── Enums/
│   │   └── EstadoPedido.cs         ← Estados: Pending, Processing, etc
│   └── Interfaces/
│       ├── IRepository<T>.cs       ← Abstracción de persistencia
│       ├── IPedidoRepository.cs    ← Interfaz específica
│       └── ...
│
├── at-prueba-tecnica-backend.Application/
│   ├── Auth/
│   │   ├── Commands/
│   │   │   └── LoginCommand.cs     ← DTO + validación
│   │   ├── Handlers/
│   │   │   └── LoginCommandHandler.cs  ← Lógica autenticación
│   │   └── Validators/
│   │       └── LoginCommandValidator.cs ← Reglas fluidas
│   ├── Pedidos/
│   │   ├── Commands/
│   │   │   ├── CreatePedidoCommand.cs
│   │   │   ├── UpdatePedidoCommand.cs
│   │   │   └── DeletePedidoCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetPedidosQuery.cs
│   │   │   ├── GetPedidoByIdQuery.cs
│   │   │   └── SearchPedidosQuery.cs
│   │   ├── Handlers/ (6 handlers)
│   │   ├── Validators/ (validaciones)
│   │   ├── Filters/ (Vali-Flow query builders)
│   │   └── DTOs/
│   │       ├── PedidoDto.cs
│   │       ├── CreatePedidoRequest.cs
│   │       └── UpdatePedidoRequest.cs
│   ├── Clientes/          ← Estructura similar
│   ├── Productos/         ← Estructura similar
│   ├── Behaviors/
│   │   ├── ValidationBehavior.cs  ← Pre-ejecución validators
│   │   └── LoggingBehavior.cs     ← Auditoría
│   ├── Mappings/
│   │   └── AutoMapper profiles
│   └── Marker/
│       └── ApplicationAssemblyMarker.cs ← Para DI reflection
│
├── at-prueba-tecnica-backend.Infrastructure/
│   ├── Persistence/
│   │   ├── AppDbContext.cs         ← EF DbContext
│   │   ├── AppDbContextFactory.cs  ← Design-time factory
│   │   ├── Configurations/
│   │   │   ├── UsuarioConfiguration.cs  ← Fluent API
│   │   │   ├── PedidoConfiguration.cs
│   │   │   ├── ClienteConfiguration.cs
│   │   │   └── ProductoConfiguration.cs
│   │   ├── Repositories/
│   │   │   ├── UsuarioRepository.cs    ← Hereda DbRepositoryAsync<T>
│   │   │   ├── PedidoRepository.cs     ← Query evaluation con Vali-Flow
│   │   │   ├── ClienteRepository.cs
│   │   │   └── ProductoRepository.cs
│   │   ├── Seeds/
│   │   │   └── DataSeeding.cs      ← Datos iniciales (admin user, products)
│   │   └── Migrations/
│   │       └── [Auto-generadas por EF]
│   ├── Auth/
│   │   ├── JwtTokenService.cs      ← Generación tokens JWT
│   │   ├── PasswordHasher.cs       ← BCrypt
│   │   └── JwtSettings.cs          ← Config
│   └── DependencyInjection.cs      ← Registro servicios
│
├── at-prueba-tecnica-backend.Api/
│   ├── appsettings.json            ← Conexión BD, JWT, logging
│   ├── appsettings.Development.json
│   ├── Program.cs                  ← DI, middlewares, hosts
│   ├── Controllers/
│   │   ├── AuthController.cs       ← POST /auth/login
│   │   ├── PedidosController.cs    ← CRUD /pedidos
│   │   ├── ClientesController.cs   ← CRUD /clientes
│   │   └── ProductosController.cs  ← CRUD /productos
│   ├── Middlewares/
│   │   ├── GlobalExceptionMiddleware.cs  ← Centralización errores
│   │   └── ExceptionHandlingExtensions.cs
│   └── Extensions/
│       ├── MigrationExtensions.cs  ← Auto-apply migrations
│       └── AuthenticationExtensions.cs ← JWT setup
│
├── docker-compose.yml              ← SQL Server 2022 + Backend
├── Dockerfile                       ← Imagen .NET multistage
├── .env                           ← Variables (NO commitear)
├── .env.example                   ← Template
├── README.md                      ← Este archivo
├── PROJECT_STRUCTURE.md           ← Detalles proyecto
├── SETUP.md                       ← Pasos instalación
└── scripts/
    └── migration.sql              ← SQL generado (opcional)
```

---

## 🚀 Quick Start

### 1️⃣ Con Docker Compose (Recomendado)

```bash
cd ~/RiderProjects/at-prueba-tecnica-backend

# Levantar SQL Server + Backend
docker-compose up -d --build

# Esperar ~15 segundos a inicialización
sleep 15

# Backend en: http://localhost:5001
# Swagger en: http://localhost:5001/swagger
```

### 2️⃣ Local con .NET CLI

```bash
cd ~/RiderProjects/at-prueba-tecnica-backend

# Instalar dependencias
dotnet restore

# Asegurar SQL Server en docker (solo BD)
docker-compose up -d sqlserver

# Ejecutar API
dotnet run --project at-prueba-tecnica-backend.Api

# Backend en: http://localhost:5000
```

### 3️⃣ Verificar con curl

```bash
# Login y obtener token
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@retopedidos.com","password":"Admin123!"}'

# Respuesta esperada:
# {
#   "success": true,
#   "data": {
#     "token": "eyJhbGciOi...",
#     "expiresIn": 3600,
#     ...
#   }
# }

# Usar token para listar pedidos
TOKEN="<token_del_login>"
curl -X GET http://localhost:5001/api/pedidos?page=1&pageSize=10 \
  -H "Authorization: Bearer $TOKEN"
```

---

## 🔐 Credenciales & Autenticación

### Usuario por Defecto

```
Email:    admin@retopedidos.com
Password: Admin123!
```

Creado automáticamente en `DataSeeding.cs` al iniciar.

### JWT Token

- **Algoritmo**: HS256
- **Duración**: 60 minutos (configurable)
- **Claims**: sub (userId), email, iat, exp
- **Secret**: Almacenado en appsettings.json (variable env en prod)

### Password Hashing

```csharp
// BCrypt.Net-Core
var hashed = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
bool isValid = BCrypt.Net.BCrypt.Verify(password, hashed);
```

---

## 📊 Base de Datos

### Diagrama Entidades

```
┌─────────────────────┐
│      Usuario        │
├─────────────────────┤
│ id (PK, GUID)       │
│ email (unique)      │
│ passwordHash        │
│ createdAt           │
│ deletedAt (soft)    │
└─────────────────────┘
         │
         │ 1:N
         ↓
┌─────────────────────────────┐
│        Pedido               │
├─────────────────────────────┤
│ id (PK, GUID)               │
│ usuarioId (FK)              │
│ orderNumber (unique)        │
│ status (Pending, etc)       │
│ total (decimal)             │
│ createdAt                   │
│ deletedAt (soft)            │
└─────────────────────────────┘

┌─────────────────────┐
│      Cliente        │
├─────────────────────┤
│ id (PK, GUID)       │
│ code (auto)         │
│ name                │
│ email               │
│ phone (opcional)    │
│ address (opcional)  │
│ createdAt           │
│ deletedAt (soft)    │
└─────────────────────┘

┌─────────────────────┐
│     Producto        │
├─────────────────────┤
│ id (PK, GUID)       │
│ name                │
│ description         │
│ unitPrice           │
│ stock               │
│ createdAt           │
│ deletedAt (soft)    │
└─────────────────────┘
```

### Soft Delete

Todos los agregados soportan soft-delete:
```csharp
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }  ← Soft delete
    
    public bool IsDeleted => DeletedAt != null;
}
```

Queries automáticamente filtran `WHERE DeletedAt IS NULL`:
```csharp
// En Repository.GetAsync()
public async Task<TEntity?> GetAsync(Guid id, CancellationToken ct)
{
    return await _dbSet
        .Where(e => e.Id == id && e.DeletedAt == null)
        .FirstOrDefaultAsync(ct);
}
```

### Migraciones

```bash
# Crear nueva migración
dotnet ef migrations add AddTablaProducto \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api

# Ver migraciones pendientes
dotnet ef migrations list

# Aplicar al destino
dotnet ef database update \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api

# Generar SQL
dotnet ef migrations script \
  --project at-prueba-tecnica-backend.Infrastructure \
  --startup-project at-prueba-tecnica-backend.Api \
  --output scripts/migration.sql --idempotent
```

---

## 📡 REST API

### Autenticación (sin JWT)

```
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@retopedidos.com",
  "password": "Admin123!"
}

Response 200:
{
  "success": true,
  "data": {
    "token": "eyJhbGc...",
    "expiresIn": 3600
  }
}
```

### Pedidos (requiere JWT)

```
GET /api/pedidos?page=1&pageSize=10&status=Pending
Authorization: Bearer <token>

GET /api/pedidos/{id}
Authorization: Bearer <token>

POST /api/pedidos
Content-Type: application/json
Authorization: Bearer <token>
{
  "orderNumber": "ORD-001",
  "clienteId": "...",
  "items": []
}

PUT /api/pedidos/{id}
Authorization: Bearer <token>
{
  "orderNumber": "ORD-002",
  "status": "Processing"
}

DELETE /api/pedidos/{id}
Authorization: Bearer <token>
```

### Clientes (requiere JWT)

```
GET /api/clientes?page=1&pageSize=10
Authorization: Bearer <token>

POST /api/clientes
Authorization: Bearer <token>
{
  "name": "Empresa XYZ",
  "email": "contact@xyz.com",
  "phone": "+56912345678",
  "address": "..."
}

PUT /api/clientes/{id}
Authorization: Bearer <token>
{...}

DELETE /api/clientes/{id}
Authorization: Bearer <token>
```

### Productos (requiere JWT)

```
GET /api/productos?page=1&pageSize=10
Authorization: Bearer <token>

POST /api/productos
Authorization: Bearer <token>
{
  "name": "Laptop",
  "description": "...",
  "unitPrice": 999.99,
  "stock": 10
}

PUT /api/productos/{id}
Authorization: Bearer <token>
{...}

DELETE /api/productos/{id}
Authorization: Bearer <token>
```

---

## 🛡️ Patrones & Principios

### SOLID

- **S** — Single Responsibility: Cada handler, validator, repository tiene 1 responsabilidad
- **O** — Open/Closed: Behaviors extensibles sin modificar existentes
- **L** — Liskov: IRepository<T> substituible, BaseRepository implementa contrato
- **I** — Interface Segregation: IPedidoRepository solo métodos de Pedido
- **D** — Dependency Inversion: DI container, abstracciones inyectables

### Patrones

| Patrón | Implementación |
|---|---|
| **CQRS** | Commands, Queries, Handlers, Mediator |
| **Repository** | DbRepositoryAsync<T>, implementaciones concretas |
| **Validator** | FluentValidator, comportamiento en pipeline |
| **Behavior** | Middlewares en mediator (logging, validation, resilience) |
| **DTO** | Request/Response objects (desacoplados de Domain) |
| **Factory** | AppDbContextFactory (design-time) |
| **Soft Delete** | DeletedAt column, filtrado automático |

### Resiliencia (Polly via Vali-Mediator.Resilience)

```csharp
// En ResilienceBehavior.cs
var policy = Policy.Handle<Exception>()
    .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30))
    .WrapAsync(
        Policy.Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt))
            )
    )
    .WrapAsync(
        Policy.TimeoutAsync(TimeSpan.FromSeconds(5))
    );

await policy.ExecuteAsync(cancellationToken => handler.Handle(...));
```

Protege contra:
- ✅ **Cascadas de fallos** (Circuit Breaker)
- ✅ **Fallos transitorios** (Retry exponencial)
- ✅ **Cuellos de botella** (Timeout)

---

## 🧪 Testing

### Login & Obtener Token

```bash
# Terminal 1: Levantar backend
cd ~/RiderProjects/at-prueba-tecnica-backend
docker-compose up -d --build

# Terminal 2: Login
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@retopedidos.com",
    "password": "Admin123!"
  }' | jq .data.token -r > /tmp/token.txt

# Guardar token
TOKEN=$(cat /tmp/token.txt)
echo $TOKEN
```

### Crear Pedido

```bash
curl -X POST http://localhost:5001/api/pedidos \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "orderNumber": "TEST-001",
    "clienteId": "00000000-0000-0000-0000-000000000001"
  }' | jq .
```

### Listar Pedidos

```bash
curl -X GET "http://localhost:5001/api/pedidos?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN" | jq .
```

### Soft Delete

```bash
curl -X DELETE "http://localhost:5001/api/pedidos/abc123" \
  -H "Authorization: Bearer $TOKEN"

# Respuesta 200 OK → Eliminado lógicamente
# GET siguiente no lo muestra
```

---

## 🐳 Docker Compose

### Servicios

```yaml
# Backend .NET
services:
  backend:
    build: .
    ports:
      - "5001:8080"
    depends_on:
      - sqlserver
    environment:
      ASPNETCORE_ENVIRONMENT: Docker
      ConnectionStrings__DefaultConnection: "..."

  # SQL Server 2022
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
      - "1433:1433"
    environment:
      SA_PASSWORD: SqlServer123!
    volumes:
      - sqlserver_data:/var/opt/mssql
```

### Comandos

```bash
# Levantar
docker-compose up -d --build

# Logs
docker-compose logs -f backend
docker-compose logs -f sqlserver

# Estado
docker-compose ps

# Detener
docker-compose down

# Limpiar volúmenes (BD)
docker-compose down -v
```

---

## ⚙️ Configuración

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=RetoPedidos;User Id=sa;Password=SqlServer123!;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "your-secret-key-at-least-32-characters-long-please",
    "Issuer": "RetoPedidos",
    "Audience": "RetoPedidosClient",
    "ExpiresInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

### Variables de Entorno (production)

Usar Azure Key Vault o similar:
```
ConnectionStrings__DefaultConnection=...
JwtSettings__Secret=...
ASPNETCORE_ENVIRONMENT=Production
```

---

## 📚 Librerías Custom (UBF21)

### Vali-Mediator v2.0.1

In-process mediator con CQRS:
```csharp
// Command (con Result<T>)
var result = await mediator.SendAsync(new CreatePedidoCommand(...));
if (result.IsSuccess)
    return Ok(result.Value);
else
    return BadRequest(result.Error);

// Query (tipado)
var pedidos = await mediator.SendAsync(new GetPedidosQuery(page: 1));
```

### Vali-Validation v2.0.1

Validators fluidos:
```csharp
public class CreatePedidoCommandValidator : Validator<CreatePedidoCommand>
{
    public CreatePedidoCommandValidator()
    {
        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Required")
            .MaximumLength(50).WithMessage("Max 50 chars");
            
        RuleFor(x => x.ClienteId)
            .NotEmpty();
    }
}

// Usado automáticamente en ValidationBehavior
```

### Vali-Mediator.Resilience v1.0.1

Políticas Polly integradas:
```csharp
// Registrado en DI
services.AddMediatorResiliencePolicy(options =>
{
    options.CircuitBreakerThreshold = 5;
    options.RetryAttempts = 3;
    options.TimeoutSeconds = 5;
});

// Ejecutado automáticamente en pipeline
```

### Vali-Flow.Core v2.0.1

Query builders type-safe:
```csharp
var filter = new PedidoFilter()
    .ByStatus(EstadoPedido.Pending)
    .ByClienteId(clienteId)
    .Build();

var pedidos = await repository.GetAsync(filter, cancellationToken);
```

### Vali-Flow v1.1.0

Evaluador EF Core:
```csharp
// En PedidoRepository
public async Task<IEnumerable<Pedido>> GetAsync(PedidoFilter filter, CancellationToken ct)
{
    return await _dbSet
        .Where(filter.Expression)  ← Vali-Flow evaluates to SQL
        .ToListAsync(ct);
}
```

---

## 🔄 Request/Response Flow Completo

```
[Frontend: React + TypeScript]
       ↓ POST /api/pedidos
       ↓ {orderNumber: "ORD-001", clienteId: "..."}
       ↓
[API Layer: Controllers]
       ↓ new CreatePedidoCommand(...)
       ↓ await mediator.SendAsync(command)
       ↓
[Mediator Pipeline]
       ├→ ValidationBehavior
       │  └→ CreatePedidoCommandValidator.ValidateAsync()
       │     └→ RuleFor(x => x.OrderNumber).NotEmpty()...
       │
       ├→ LoggingBehavior
       │  └→ logger.LogInformation("Executing CreatePedidoCommand...")
       │
       └→ ResilienceBehavior
          └→ Polly Policy (CircuitBreaker, Retry, Timeout)
             ↓
[Handler]
       ├→ Verify cliente exists
       ├→ Create Pedido aggregate
       ├→ repository.AddAsync(pedido)
       └→ unitOfWork.SaveChangesAsync()
             ↓
[Infrastructure Layer]
       ├→ DbContext.Pedidos.Add(pedido)
       ├→ DbContext.SaveChangesAsync()
       └→ SQL Server transaction
             ↓
[Database]
       └→ INSERT INTO Pedidos (id, orderNumber, ...) VALUES (...)
             ↓
[Response Pipeline]
       ├→ Result<CreatePedidoResponse>.Success(dto)
       ├→ HTTP 201 Created
       └→ {success: true, data: {...}}
             ↓
[Frontend]
       └→ Toast success + Refetch table
```

---

## 🐛 Troubleshooting

| Problema | Causa | Solución |
|---|---|---|
| "Invalid object name 'Pedidos'" | Migrations no aplicadas | Esperar 15s, revisar logs: `docker-compose logs backend` |
| CORS error | Frontend no permitido | `Program.cs` línea ~60, agregar origen |
| Login 401 | Credenciales inválidas | Usar `admin@retopedidos.com` / `Admin123!` |
| Token expirado | JWT vencido (60 min) | Login nuevamente |
| "Connection timeout" | SQL Server no está listo | `docker-compose ps`, esperar ~10s |
| Port 5001 en uso | Otro proceso | `lsof -i :5001`, matar proceso |

---

## 📖 Documentación Adicional

- 📄 [PROJECT_STRUCTURE.md](./PROJECT_STRUCTURE.md) — Detalles carpetas/archivos
- 🚀 [SETUP.md](./SETUP.md) — Instrucciones paso a paso
- 🐳 [DOCKER_SETUP.md](./DOCKER_SETUP.md) — Docker Compose guide
- 📝 [MIGRATION_INSTRUCTIONS.md](./MIGRATION_INSTRUCTIONS.md) — EF Core migrations

---

## 📚 Referencias

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core)
- [Polly Resilience](https://github.com/App-vNext/Polly)
- [JWT (RFC 7519)](https://tools.ietf.org/html/rfc7519)
- [OpenAPI Specification](https://spec.openapis.org/oas/v3.1.0)

---

## 👤 Autor

**Felipe Rafael Montenegro Morriberon (UBF21)**

Créditos especiales:
- ⭐ **Vali-Mediator** — CQRS + Result<T> architecture
- ⭐ **Vali-Validation** — Fluent validators
- ⭐ **Vali-Mediator.Resilience** — Polly policies
- ⭐ **Vali-Flow.Core** — Query builders
- ⭐ **Vali-Flow** — EF Core evaluator

---

**Fecha:** Abril 2026  
**Versión:** 1.0.0  
**Licencia:** Privada (Prueba Técnica)
