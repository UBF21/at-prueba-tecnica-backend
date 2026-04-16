# Estructura del Proyecto

## 📁 Árbol de directorios (después de `init-project.sh`)

```
at-prueba-tecnica-backend/
│
├── at-prueba-tecnica-backend.sln           ← Solución .NET 9
│
├── at-prueba-tecnica-backend.Domain/       ← Capa de Dominio
│   ├── bin/
│   ├── obj/
│   ├── Properties/
│   ├── at-prueba-tecnica-backend.Domain.csproj
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── Customer.cs
│   │   ├── Product.cs
│   │   └── AuditableEntity.cs
│   ├── Enums/
│   │   ├── Role.cs
│   │   └── OrderStatus.cs
│   └── Interfaces/
│       ├── IUserRepository.cs
│       ├── IOrderRepository.cs
│       ├── IOrderItemRepository.cs
│       ├── ICustomerRepository.cs
│       └── IProductRepository.cs
│
├── at-prueba-tecnica-backend.Application/  ← Capa de Aplicación (CQRS)
│   ├── bin/
│   ├── obj/
│   ├── Properties/
│   ├── at-prueba-tecnica-backend.Application.csproj
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── Commands/
│   │   │   │   └── LoginCommand.cs
│   │   │   ├── Filters/
│   │   │   │   └── UserFilters.cs           ← Vali-Flow query builders
│   │   │   ├── Handlers/
│   │   │   │   └── LoginCommandHandler.cs
│   │   │   └── Validators/
│   │   │       └── LoginCommandValidator.cs
│   │   ├── Orders/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateOrderCommand.cs
│   │   │   │   ├── UpdateOrderCommand.cs
│   │   │   │   └── DeleteOrderCommand.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetOrdersQuery.cs
│   │   │   │   └── GetOrderByIdQuery.cs
│   │   │   ├── Handlers/ (6+ handlers)
│   │   │   ├── Filters/
│   │   │   │   ├── OrderFilters.cs
│   │   │   │   └── OrderItemFilters.cs
│   │   │   ├── Validators/
│   │   │   └── DTOs/
│   │   ├── Customers/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   ├── Handlers/ (CRUD handlers)
│   │   │   ├── Filters/
│   │   │   │   └── CustomerFilters.cs
│   │   │   ├── Validators/
│   │   │   └── DTOs/
│   │   ├── Products/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   ├── Handlers/ (CRUD handlers)
│   │   │   ├── Filters/
│   │   │   │   └── ProductFilters.cs
│   │   │   ├── Validators/
│   │   │   └── DTOs/
│   │   ├── DTOs/ (Shared DTOs)
│   │   └── Mappings/ (AutoMapper profiles)
│   ├── Behaviors/
│   │   └── LoggingBehavior.cs              ← Validación integrada vía Vali-Mediator
│   └── ApplicationAssemblyMarker.cs        ← Para DI reflection
│
├── at-prueba-tecnica-backend.Infrastructure/  ← Capa de Infraestructura
│   ├── bin/
│   ├── obj/
│   ├── Properties/
│   ├── at-prueba-tecnica-backend.Infrastructure.csproj
│   ├── Persistence/
│   │   ├── AppDbContext.cs                 ← EF DbContext con OnModelCreating seed
│   │   ├── AppDbContextFactory.cs           ← Design-time factory para EF
│   │   ├── Configurations/
│   │   │   ├── UserConfiguration.cs         ← Fluent API + HasQueryFilter soft delete
│   │   │   ├── OrderConfiguration.cs
│   │   │   ├── OrderItemConfiguration.cs
│   │   │   ├── CustomerConfiguration.cs
│   │   │   └── ProductConfiguration.cs
│   │   ├── Repositories/
│   │   │   ├── UserRepository.cs            ← Hereda DbRepositoryAsync<T>
│   │   │   ├── OrderRepository.cs
│   │   │   ├── OrderItemRepository.cs
│   │   │   ├── CustomerRepository.cs
│   │   │   └── ProductRepository.cs
│   │   └── Migrations/
│   │       └── (auto-generadas por EF)
│   ├── Auth/
│   │   ├── JwtTokenService.cs
│   │   └── JwtSettings.cs
│   └── DependencyInjection.cs               ← Extensión para IServiceCollection
│
├── at-prueba-tecnica-backend.Api/            ← Capa de Presentación (API)
│   ├── bin/
│   ├── obj/
│   ├── Properties/
│   ├── at-prueba-tecnica-backend.Api.csproj
│   ├── appsettings.json                    ← Config (conexión BD, JWT, etc)
│   ├── appsettings.Development.json
│   ├── launchSettings.json
│   ├── Program.cs                          ← Main: DI, middlewares, config
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── OrdersController.cs
│   │   ├── CustomersController.cs
│   │   └── ProductsController.cs
│   ├── Middlewares/
│   │   ├── GlobalExceptionMiddleware.cs
│   │   └── ExceptionHandlingExtensions.cs
│   └── Extensions/
│       ├── MigrationExtensions.cs           ← EnsureDeletedAsync + EnsureCreatedAsync
│       └── AuthenticationExtensions.cs
│
├── docker-compose.yml                       ← SQL Server + otros servicios
├── Dockerfile                               ← Imagen del backend (opcional)
├── .env                                     ← Secrets (NUNCA commitear)
├── .env.example                             ← Template para .env
├── .gitignore                               ← Archivos ignorados
├── init-project.sh                          ← Script para setup inicial
├── check-prerequisites.sh                   ← Verificar prerequisites
│
├── README.md                                ← Documentación general
├── SETUP.md                                 ← Instrucciones paso a paso
├── PROJECT_STRUCTURE.md                     ← Este archivo
│
└── scripts/
    └── migration.sql                        ← Script SQL generado (opcional)
```

---

## 🔗 Relaciones entre proyectos

```
Api (Presentación)
    ↑
    ├── referencia → Infrastructure
    │
Infrastructure (Persistencia)
    ↑
    ├── referencia → Application
    │
Application (Lógica de negocio)
    ↑
    ├── referencia → Domain
    │
Domain (Entidades y contratos)
    (sin referencias a otros proyectos)
```

---

## 📦 Dependencias NuGet por proyecto

### Domain
- *(Sin dependencias — puro .NET)*

### Application
- `Vali-Mediator` v2.0.1
- `Vali-Validation` v2.0.2
- `Vali-Validation.ValiMediator` v1.0.3
- `Vali-Mediator.Resilience` v1.1.0
- `Vali-Flow.Core` v2.0.1
- `FluentValidation` v11.9.2

### Infrastructure
- `Vali-Flow` v1.3.4
- `Vali-Flow.Core` v2.0.2
- `Microsoft.EntityFrameworkCore.SqlServer` v9.0.15
- `Microsoft.EntityFrameworkCore.Tools` v9.0.15
- `BCrypt.Net-Next` v4.1.0
- (+ referencia a Application)

### Api
- `Microsoft.AspNetCore.Authentication.JwtBearer` v9.0.0
- `Vali-Mediator.AspNetCore` v1.1.0
- `Scalar.AspNetCore` v2.13.22
- `Microsoft.AspNetCore.OpenApi` v9.0.0
- (+ referencia a Infrastructure)

---

## 🎯 Flujo de una request HTTP

```
┌─────────────────────────────────────────────────────────────────┐
│  HTTP POST /api/orders                                          │
│  Authorization: Bearer eyJhbGc...                              │
│  { "orderNumber": "ORD-001", "total": 100 }                    │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  API Layer (Controllers)                                        │
│  - OrdersController.Create()                                   │
│  - Deserializa JSON → CreateOrderCommand                       │
│  - Llama mediator.Send(command)                                │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Application Layer (Behaviors Pipeline)                         │
│  1. LoggingBehavior         → Log "START CreateOrderCommand"   │
│  2. ValidationBehavior      → Vali-Validation (integrado)      │
│  3. ResilienceBehavior      → Circuit Breaker, Retry, etc      │
│  4. Siguiente en pipeline...                                   │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Application Layer (Handler)                                   │
│  - CreateOrderCommandHandler.Handle()                          │
│  - Lógica de negocio: validar unicidad OrderNumber            │
│  - Llamar repository.AddAsync()                                │
│  - Retornar Result<OrderDto>                                   │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Infrastructure Layer (Repository)                             │
│  - OrderRepository.AddAsync()                                  │
│  - Hereda de DbRepositoryAsync<T> (Vali-Flow)                 │
│  - Llamar DbContext.SaveChangesAsync()                         │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Persistence (EF Core → SQL Server)                            │
│  - INSERT INTO Orders (OrderNumber, Total, ...) VALUES (...)  │
│  - COMMIT (transacción implícita vía EF)                       │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Return Result<OrderDto> (éxito o error)                       │
│  → Mapeo automático a HTTP Status Code                        │
│  → 201 Created (si todo OK)                                    │
│  → 400 Bad Request (si validación falla)                       │
│  → 409 Conflict (si OrderNumber duplicado)                     │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🧵 Estado de cada capa

| Capa | Finalidad | Dependencias |
|---|---|---|
| **Domain** | Definir reglas de negocio, entidades, interfaces | Ninguna |
| **Application** | Orquestar casos de uso (CQRS), validaciones, comportamientos transversales | Domain |
| **Infrastructure** | Implementar persistencia, autenticación, servicios externos | Application + Domain |
| **Api** | Exponer endpoints HTTP, routing, autorización | Infrastructure + Application |

---

## 🔄 Ciclo de vida de la aplicación

```
Startup (Program.cs)
    ├── 1. Cargar appsettings.json
    ├── 2. Configurar servicios (DI)
    │   ├── DbContext
    │   ├── Vali-Mediator + behaviors
    │   ├── Vali-Mediator.Resilience
    │   ├── Autenticación JWT
    │   └── CORS
    ├── 3. Crear middleware pipeline
    ├── 4. Eliminar y recrear base de datos (EnsureDeletedAsync + EnsureCreatedAsync)
    ├── 5. Seed datos iniciales en AppDbContext.OnModelCreating()
    └── 6. Listen en http://localhost:5001 (Docker) o http://localhost:5000 (local)
            ↓
    Request HTTP
            ↓
    Middleware Pipeline
    ├── ExceptionHandling
    ├── Authentication
    ├── Authorization
    └── Routing → Controllers → Mediator
            ↓
    Response HTTP
```

---

## ✅ Estado actual

**El proyecto está completamente implementado.** Todos los archivos listados en el árbol de directorios existen y funcionan:

- ✅ Todas las entidades (User, Order, OrderItem, Customer, Product)
- ✅ Todas las capas (Domain, Application, Infrastructure, Api)
- ✅ CQRS completo con múltiples commands/queries
- ✅ Validación fluida con Vali-Validation
- ✅ Persistencia con Vali-Flow y EF Core
- ✅ Autenticación JWT
- ✅ Resiliencia (Circuit Breaker, Retry, Timeout)
- ✅ Documentación OpenAPI con Scalar
- ✅ Docker Compose para desarrollo
