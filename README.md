# 🎯 AT — Prueba Técnica Fullstack Senior

**Sistema CQRS de Gestión de Órdenes, Clientes y Productos (Order Management System)**  
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
- ✅ **API profesional** (REST, OpenAPI/Scalar)

### Stack Tecnológico

| Componente | Versión | Rol |
|---|---|---|
| **.NET** | 9.0 | Framework |
| **SQL Server** | 2022 | Base de datos |
| **Entity Framework Core** | 9.0 | ORM |
| **Vali-Mediator** | 2.0.1 | ⭐ CQRS + Result<T> |
| **Vali-Validation** | 2.0.1 | ⭐ Validators fluidos |
| **Vali-Mediator.Resilience** | 1.2.0 | ⭐ Políticas de resiliencia (sin Polly) |
| **Vali-Flow.Core** | 2.0.2 | ⭐ Query builders |
| **Vali-Flow** | 1.3.4 | ⭐ EF evaluator |
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
  
- **Vali-Mediator.Resilience**: Políticas de resiliencia (zero-dependency, sin Polly)
  - Circuit Breaker con sliding window (Closed → Open → HalfOpen)
  - Retry con backoff Fixed / Linear / Exponential / ExponentialWithJitter
  - Timeout optimista (CancellationToken) y pesimista (Task.WhenAny)
  - Bulkhead (semáforo de concurrencia + queue)
  - Presets listos para uso: ForDatabase, ForExternalApi, ForCritical, NoResilience
  
- **Vali-Flow.Core**: Builder de queries LINQ
  - Expression trees type-safe
  - Filters composables (IsNull, EqualTo, Contains, etc)
  - Specifications dinámicas con fluent API
  - Soporte para soft delete (DeletedAt.HasValue)
  
- **Vali-Flow**: Evaluador para EF Core
  - Traduce Vali-Flow queries a SQL optimizado
  - Evaluación perezosa (lazy evaluation)
  - Integración con HasQueryFilter para filtros globales
  - Soporte para offset/limit en paginación

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
│   │   ├── User.cs                 ← Entidad (usuario + autenticación)
│   │   ├── Order.cs                ← Entidad (order + items)
│   │   ├── Customer.cs             ← Entidad (customer)
│   │   ├── Product.cs              ← Entidad (product)
│   │   └── OrderItem.cs            ← Entidad (item en orden)
│   ├── Enums/
│   │   ├── Role.cs                 ← Roles: Admin, Customer
│   │   └── OrderStatus.cs          ← Estados: Pending, Processing, etc
│   └── Interfaces/
│       ├── IUserRepository.cs      ← Interfaz User
│       ├── IOrderRepository.cs     ← Interfaz Order
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
│   ├── Orders/
│   │   ├── Commands/
│   │   │   ├── CreateOrderCommand.cs
│   │   │   ├── UpdateOrderCommand.cs
│   │   │   └── DeleteOrderCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetOrdersQuery.cs
│   │   │   ├── GetOrderByIdQuery.cs
│   │   ├── Handlers/ (6+ handlers)
│   │   ├── Validators/ (validaciones fluidas)
│   │   ├── Filters/ (Vali-Flow query builders)
│   │   └── DTOs/ (DTOs y requests)
│   ├── Customers/         ← Estructura similar
│   ├── Products/          ← Estructura similar
│   ├── Auth/              ← Login, JWT
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
│   │   │   ├── UserConfiguration.cs    ← Fluent API con HasQueryFilter
│   │   │   ├── OrderConfiguration.cs
│   │   │   ├── CustomerConfiguration.cs
│   │   │   ├── ProductConfiguration.cs
│   │   │   └── OrderItemConfiguration.cs
│   │   ├── Repositories/
│   │   │   ├── UserRepository.cs       ← Hereda DbRepositoryAsync<T>
│   │   │   ├── OrderRepository.cs      ← Query evaluation con Vali-Flow
│   │   │   ├── CustomerRepository.cs
│   │   │   ├── ProductRepository.cs
│   │   │   └── OrderItemRepository.cs
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
│   │   ├── AuthController.cs       ← POST /api/auth/login
│   │   ├── OrdersController.cs     ← CRUD /api/orders
│   │   ├── CustomersController.cs  ← CRUD /api/customers
│   │   └── ProductsController.cs   ← CRUD /api/products
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
# Scalar en: http://localhost:5001/scalar
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

## PATRONES & PRINCIPIOS ARQUITECTÓNICOS

### SOLID

- **S** — Single Responsibility: Cada handler, validator, repository tiene 1 responsabilidad
- **O** — Open/Closed: Behaviors extensibles sin modificar existentes
- **L** — Liskov: IRepository<T> substituible, BaseRepository implementa contrato
- **I** — Interface Segregation: IPedidoRepository solo métodos de Pedido
- **D** — Dependency Inversion: DI container, abstracciones inyectables

### PATRONES IMPLEMENTADOS

#### 1. CQRS (Command Query Responsibility Segregation)

Separación clara entre operaciones que modifican estado (Commands) y operaciones de lectura (Queries).

Ubicación: Application/Features/[Entity]/{Commands,Queries,Handlers}

Ejemplo de Command:
```csharp
// CreatePedidoCommand.cs
public class CreatePedidoCommand : IRequest<Result<CreatePedidoResponse>>
{
    public string OrderNumber { get; set; }
    public Guid ClienteId { get; set; }
}

// CreatePedidoCommandHandler.cs
public class CreatePedidoCommandHandler : IRequestHandler<CreatePedidoCommand, Result<CreatePedidoResponse>>
{
    public async Task<Result<CreatePedidoResponse>> Handle(CreatePedidoCommand request, CancellationToken ct)
    {
        // Lógica de creación
    }
}
```

Ejemplo de Query:
```csharp
// GetPedidosQuery.cs
public class GetPedidosQuery : IRequest<Result<PaginatedResponse<PedidoDto>>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}

// GetPedidosQueryHandler.cs
public class GetPedidosQueryHandler : IRequestHandler<GetPedidosQuery, Result<PaginatedResponse<PedidoDto>>>
{
    public async Task<Result<PaginatedResponse<PedidoDto>>> Handle(GetPedidosQuery request, CancellationToken ct)
    {
        // Lógica de lectura optimizada
    }
}
```

Beneficios: Separación de responsabilidades clara, escalabilidad diferenciada para lecturas/escrituras, testabilidad mejorada.

#### 2. MEDIATOR PATTERN

Bus in-process que enruta Commands/Queries a sus respectivos handlers.

Ubicación: Vali-Mediator (librería custom)

Uso en Controllers:
```csharp
[HttpPost("pedidos")]
public async Task<IActionResult> CreatePedido([FromBody] CreatePedidoRequest request)
{
    var command = new CreatePedidoCommand 
    { 
        OrderNumber = request.OrderNumber,
        ClienteId = request.ClienteId 
    };
    
    var result = await _mediator.SendAsync(command, cancellationToken);
    return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
}
```

Beneficios: Desacoplamiento entre Controllers y lógica de negocio, pipeline comportamientos centralizados, testeo sin HTTP.

#### 3. REPOSITORY PATTERN

Abstracción de acceso a datos con interfaz genérica.

Ubicación: Infrastructure/Persistence/Repositories/

Interfaz genérica:
```csharp
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<TEntity>> GetAsync(ISpecification<TEntity> spec, CancellationToken ct);
    Task AddAsync(TEntity entity, CancellationToken ct);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
```

Implementación concreta:
```csharp
public class PedidoRepository : DbRepositoryAsync<Pedido>, IPedidoRepository
{
    public PedidoRepository(AppDbContext context) : base(context) { }
    
    public async Task<Pedido?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct)
    {
        return await _dbSet
            .Where(p => p.OrderNumber == orderNumber && p.DeletedAt == null)
            .FirstOrDefaultAsync(ct);
    }
}
```

Beneficios: Independencia de ORM, testing con mocks, queries optimizadas por entidad.

#### 4. DECORATOR/BEHAVIOR PATTERN

Middlewares en el pipeline de mediator que envuelven la ejecución del handler.

Ubicación: Application/Behaviors/

Validación:
```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return FailureResult(validationResult.Errors);
        
        return await next();
    }
}
```

Logging:
```csharp
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        _logger.LogInformation("Ejecutando: {CommandName}", typeof(TRequest).Name);
        var response = await next();
        _logger.LogInformation("Completado: {CommandName}", typeof(TRequest).Name);
        return response;
    }
}
```

Beneficios: Aspectos transversales centralizados (logging, validación, resiliencia), sin contaminar handlers.

#### 5. DATA TRANSFER OBJECT (DTO)

Desacoplamiento entre Domain models y API contracts.

Ubicación: Application/Features/DTOs/

Domain Entity vs DTO:
```csharp
// Domain: Pedido.cs (lógica de negocio, validaciones)
public class Pedido : BaseEntity
{
    public string OrderNumber { get; set; }
    public Guid ClienteId { get; set; }
    public EstadoPedido Status { get; set; }
    public decimal Total { get; set; }
    public void MarkAsShipped() { Status = EstadoPedido.Shipped; }
}

// DTO: PedidoDto.cs (serialización segura)
public class PedidoDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public string ClienteName { get; set; }
    public string Status { get; set; }
    public decimal Total { get; set; }
}
```

Request/Response DTOs:
```csharp
public class CreatePedidoRequest
{
    [Required] public string OrderNumber { get; set; }
    [Required] public Guid ClienteId { get; set; }
}

public class CreatePedidoResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
}
```

Beneficios: Contrato API estable, oculta complejidad interna, versionado independiente.

#### 6. SPECIFICATION PATTERN

Encapsulatión de lógica de filtrado compleja en objetos reutilizables.

Ubicación: Application/Features/[Entity]/Filters/

Ejemplo:
```csharp
public class PedidoFilterSpecification : Specification<Pedido>
{
    public PedidoFilterSpecification(PedidoFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.OrderNumber))
            AddCriteria(p => p.OrderNumber.Contains(filter.OrderNumber));
        
        if (filter.Status.HasValue)
            AddCriteria(p => p.Status == filter.Status);
        
        if (filter.ClienteId.HasValue)
            AddCriteria(p => p.ClienteId == filter.ClienteId);
        
        AddCriteria(p => p.DeletedAt == null);
    }
}
```

Uso en repository:
```csharp
var specification = new PedidoFilterSpecification(filter);
var pedidos = await _repository.GetAsync(specification, ct);
```

Beneficios: DRY, queries reutilizables, testeo de lógica de filtrado.

#### 7. FACTORY PATTERN

Creación de instancias complejas desacoplada.

Ubicación: Infrastructure/Persistence/AppDbContextFactory.cs

```csharp
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .Options;
        
        return new AppDbContext(options);
    }
}
```

Beneficios: Migrations sin runtime, configuración centralizada.

#### 8. SOFT DELETE PATTERN

Eliminación lógica en lugar de física.

Ubicación: Domain/Entities/BaseEntity.cs + Infrastructure/Repositories

Base Entity:
```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public bool IsDeleted => DeletedAt != null;
    
    public void Delete() => DeletedAt = DateTime.UtcNow;
    public void Restore() => DeletedAt = null;
}
```

Filtrado automático en queries:
```csharp
public async Task<TEntity?> GetAsync(Guid id, CancellationToken ct)
{
    return await _dbSet
        .Where(e => e.Id == id && e.DeletedAt == null)
        .FirstOrDefaultAsync(ct);
}
```

Beneficios: Auditoría completa, recuperación de datos, cumplimiento legal.

#### 9. AGGREGATE PATTERN (Domain-Driven Design)

Agrupación de entidades relacionadas como una unidad atómica.

```csharp
// Aggregate Root
public class Pedido : BaseEntity
{
    public string OrderNumber { get; set; }
    public Guid ClienteId { get; set; }
    private List<PedidoItem> _items = new();
    
    public IReadOnlyList<PedidoItem> Items => _items.AsReadOnly();
    
    // Invariantes del agregado
    public void AddItem(Producto producto, int cantidad)
    {
        if (cantidad <= 0) throw new InvalidOperationException("Cantidad debe ser > 0");
        if (Status != EstadoPedido.Pending) throw new InvalidOperationException("No puede agregar items");
        
        _items.Add(new PedidoItem { ProductoId = producto.Id, Cantidad = cantidad });
    }
    
    public void RemoveItem(PedidoItem item)
    {
        _items.Remove(item);
    }
}

// Value Object
public class PedidoItem
{
    public Guid ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
```

Beneficios: Lógica de negocio centralizada, invariantes garantizadas, transacciones atómicas.

#### 10. PERSISTENCIA CON VALI-FLOW

Repositorios con evaluadores de lectura/escritura usando Vali-Flow.

```csharp
// 1️⃣ DEFINIR FILTROS REUTILIZABLES
public static class CustomerFilters
{
    public static ValiFlowQuery<Customer> Active() =>
        new ValiFlowQuery<Customer>().IsNull(c => c.DeletedAt);
    
    public static ValiFlowQuery<Customer> ById(Guid id) =>
        Active().And().EqualTo(c => c.Id, id);
    
    public static ValiFlowQuery<Customer> ByEmail(string email) =>
        Active().And().EqualTo(c => c.Email, email);
}

// 2️⃣ LECTURA PAGINADA (múltiples resultados)
var spec = new QuerySpecification<Customer>()
    .WithFilter(CustomerFilters.Active())
    .WithOrderBy(c => c.Name, ascending: true)
    .WithPagination(page: 1, pageSize: 10)
    .WithAsNoTracking(true);

var queryable = await _repository.EvaluateQueryAsync(spec);
var customers = await queryable.ToListAsync(ct);

// 3️⃣ LECTURA POR ID (un único resultado)
var spec = new BasicSpecification<Customer>()
    .WithFilter(CustomerFilters.ById(customerId))
    .WithAsNoTracking(true);

var customer = await _repository.EvaluateGetFirstAsync(spec, ct);
if (customer is null)
    return Result<CustomerDto>.Fail("Not found", ErrorType.NotFound);

// 4️⃣ ESCRITURA - CREATE
var newCustomer = new Customer { Name = "...", Email = "..." };
await _repository.AddAsync(newCustomer, saveChanges: true, ct);

// 5️⃣ ESCRITURA - UPDATE
customer.Name = "Updated";
await _repository.UpdateAsync(customer, saveChanges: true, ct);

// 6️⃣ ESCRITURA - DELETE (Soft Delete)
customer.DeletedAt = DateTime.UtcNow;
await _repository.UpdateAsync(customer, saveChanges: true, ct);
```

**Características Vali-Flow:**
- ✅ `ValiFlowQuery<T>` - Constructor de expresiones LINQ type-safe
  - `.IsNull(prop)` / `.IsNotNull(prop)` - Validación de nulidad
  - `.EqualTo(prop, value)` - Igualdad
  - `.Contains(prop, text)` - Búsqueda parcial
  - `.And()` / `.Or()` - Combinadores lógicos
  
- ✅ `QuerySpecification<T>` - Para listas paginadas
  - `.WithFilter()` - Aplica el ValiFlowQuery
  - `.WithOrderBy()` - Ordenamiento
  - `.WithPagination()` - Offset/limit
  - `.WithAsNoTracking()` - EF optimización

- ✅ `BasicSpecification<T>` - Para resultados únicos
  - Mismo API que QuerySpecification pero optimizado para `.FirstAsync()`

- ✅ Métodos del repositorio:
  - `EvaluateQueryAsync(spec)` → `IQueryable<T>` (lazy)
  - `EvaluateGetFirstAsync(spec)` → `T | null` (ejecuta directamente)
  - `AddAsync(entity, saveChanges, ct)` - Insertar
  - `UpdateAsync(entity, saveChanges, ct)` - Actualizar
  - `RemoveAsync(entity, saveChanges, ct)` - Eliminar físico

**Sin Unit of Work:**
- EF Core DbContext maneja transacciones automáticamente
- SaveChanges ocurre por operación (no combinado)
- Filtros globales via `HasQueryFilter` excluyen soft-deleted automáticamente

### RESILIENCIA (Vali-Mediator.Resilience)

Esta librería implementa resiliencia sin dependencias externas (no usa Polly).
La integración con el mediator se activa via `config.AddResilienceBehavior()` en `Program.cs` y un
registry singleton (`AddResilienceRegistry`) para compartir el estado del Circuit Breaker entre
comandos que accedan al mismo recurso.

Las políticas se declaran en `Application/DependencyInjection.cs` — desacopladas de los commands
(patrón **Policy Provider**, introducido en v1.2.0 que deprecó `IResilient`):

```csharp
// LoginCommand — rate limit por email (5 req/min) + retry + circuit breaker + timeout
services.AddResiliencePolicy<LoginCommand>(_ =>
    ResiliencePolicy.Create("login")
        .RateLimiter(o =>
        {
            o.Algorithm = RateLimiterAlgorithm.SlidingWindow;
            o.PermitLimit = 5;
            o.Window = TimeSpan.FromMinutes(1);
            o.PartitionKeyResolver = r => ((LoginCommand)r).Email;  // por cuenta, no global
        })
        .Retry(o => { o.MaxRetries = 3; o.BackoffType = BackoffType.ExponentialWithJitter; })
        .CircuitBreaker(o => { o.CircuitKey = "login"; o.FailureThreshold = 5; })
        .Timeout(TimeSpan.FromSeconds(10))
        .Build());

// CreateOrderCommand — bulkhead + retry + circuit breaker + timeout
services.AddResiliencePolicy<CreateOrderCommand>(_ =>
    ResiliencePolicy.Create("create-order")
        .Bulkhead(o => { o.MaxConcurrentCalls = 20; o.MaxQueuedCalls = 10; })
        .Retry(o => { o.MaxRetries = 3; o.BackoffType = BackoffType.ExponentialWithJitter; })
        .CircuitBreaker(o => { o.CircuitKey = "create-order"; o.FailureThreshold = 5; })
        .Timeout(TimeSpan.FromSeconds(30))
        .Build());

// Global fallback — retry + timeout para todos los demás commands/queries
services.AddGlobalResiliencePolicy(
    ResiliencePolicy.Create("global")
        .Retry(o => { o.MaxRetries = 2; o.BackoffType = BackoffType.Linear; })
        .Timeout(TimeSpan.FromSeconds(30))
        .Build());
```

El `ResilienceBehavior<TRequest, TResponse>` intercepta automáticamente en el pipeline:
no requiere ningún cambio en los handlers ni en los commands.

Cada política tiene un propósito específico y actúa en un escenario diferente:

| Política | Qué hace | Cuándo actúa |
|---|---|---|
| **Retry** | Reintenta la operación | Solo si lanza una excepción (BD caída, timeout) |
| **Circuit Breaker** | Corta el circuito | Después de N excepciones consecutivas |
| **Bulkhead** | Limita concurrencia | Solo si hay demasiadas requests simultáneas |
| **Timeout** | Cancela si tarda mucho | Si el handler no responde en X segundos |

Protege contra:
- ✅ **Fallos transitorios de BD** (Retry con backoff)
- ✅ **Cascadas de fallos** (Circuit Breaker — abre después de N fallos consecutivos)
- ✅ **Timeouts** (cancela operaciones que excedan el límite)
- ✅ **Saturación de concurrencia** (Bulkhead — limita llamadas simultáneas)

#### Por qué no se implementaron Circuit Breaker HTTP ni Bulkhead para APIs externas

Este OMS es un **backend autónomo**: no consume servicios HTTP externos ni se comunica con
otros microservicios. Todos los accesos son internos (DB SQL Server vía EF Core).

Los patrones clásicos de resiliencia distribuida aplican cuando:
- Hay llamadas a APIs de terceros (pagos, notificaciones, inventario externo)
- Existe comunicación inter-servicio en una arquitectura de microservicios
- Se usa un message broker externo (RabbitMQ, Kafka) sin cliente local

En ese escenario se agregaría:

```csharp
// Ejemplo ilustrativo — para un hipotético servicio de pagos externo
services.AddHttpClient<IPaymentGatewayClient, PaymentGatewayClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(2)
    });

// Policy dedicada para el cliente HTTP externo
var externalApiPolicy = ResiliencePolicy.Create("payment-gateway")
    .Retry(o =>
    {
        o.MaxRetries = 3;
        o.BackoffType = BackoffType.ExponentialWithJitter;
        o.InitialDelay = TimeSpan.FromMilliseconds(200);
    })
    .CircuitBreaker(o =>
    {
        o.CircuitKey = "payment-gateway";
        o.FailureThreshold = 5;
        o.SamplingDuration = TimeSpan.FromSeconds(30);
        o.BreakDuration = TimeSpan.FromSeconds(60);
    })
    .Timeout(TimeSpan.FromSeconds(10))
    .Build();
```

La resiliencia a nivel de **base de datos** ya está cubierta en dos capas:
1. **EF Core** — `EnableRetryOnFailure(maxRetryCount: 10)` en `DependencyInjection.cs`
2. **Mediator pipeline** — `ResilienceBehavior` con presets `ForDatabase` / `ForCritical` en los comandos críticos

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

### Vali-Mediator.Resilience v1.2.0

Políticas de resiliencia zero-dependency (sin Polly). A partir de v1.2.0 las políticas
se registran en DI desacopladas de los commands (el patrón `IResilient` fue deprecado):

```csharp
// Program.cs — wiring del behavior y registry
builder.Services.AddApplication();         // registra políticas por command + global fallback
builder.Services.AddValiMediator(config =>
{
    config.AddResilienceBehavior();         // activa ResilienceBehavior<,> en el pipeline
});
builder.Services.AddResilienceRegistry();  // estado de Circuit Breaker compartido (singleton)

// Application/DependencyInjection.cs — políticas declaradas por command
services.AddResiliencePolicy<LoginCommand>(_ => ResiliencePolicy.Create("login")
    .RateLimiter(o =>
    {
        o.Algorithm = RateLimiterAlgorithm.SlidingWindow;
        o.PermitLimit = 5;
        o.Window = TimeSpan.FromMinutes(1);
        o.PartitionKeyResolver = r => ((LoginCommand)r).Email;  // aislado por cuenta
    })
    .Retry(o => { o.MaxRetries = 3; o.BackoffType = BackoffType.ExponentialWithJitter; })
    .CircuitBreaker(o => { o.CircuitKey = "login"; o.FailureThreshold = 5; })
    .Timeout(TimeSpan.FromSeconds(10))
    .Build());

// Global fallback para todos los commands sin política explícita
services.AddGlobalResiliencePolicy(
    ResiliencePolicy.Create("global").Retry(2).Timeout(TimeSpan.FromSeconds(30)).Build());
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
