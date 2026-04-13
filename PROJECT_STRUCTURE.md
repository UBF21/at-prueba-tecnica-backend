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
│   │   ├── Usuario.cs
│   │   └── Pedido.cs
│   ├── Enums/
│   │   └── EstadoPedido.cs
│   └── Interfaces/
│       ├── IPedidoRepository.cs
│       └── IUsuarioRepository.cs
│
├── at-prueba-tecnica-backend.Application/  ← Capa de Aplicación (CQRS)
│   ├── bin/
│   ├── obj/
│   ├── Properties/
│   ├── at-prueba-tecnica-backend.Application.csproj
│   ├── Auth/
│   │   ├── Commands/
│   │   │   └── LoginCommand.cs
│   │   ├── Handlers/
│   │   │   └── LoginCommandHandler.cs
│   │   └── Validators/
│   │       └── LoginCommandValidator.cs
│   ├── Pedidos/
│   │   ├── Commands/
│   │   │   ├── CreatePedidoCommand.cs
│   │   │   ├── UpdatePedidoCommand.cs
│   │   │   └── DeletePedidoCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetPedidosQuery.cs
│   │   │   └── GetPedidoByIdQuery.cs
│   │   ├── Handlers/
│   │   │   ├── CreatePedidoCommandHandler.cs
│   │   │   ├── UpdatePedidoCommandHandler.cs
│   │   │   ├── DeletePedidoCommandHandler.cs
│   │   │   ├── GetPedidosQueryHandler.cs
│   │   │   └── GetPedidoByIdQueryHandler.cs
│   │   ├── Validators/
│   │   │   ├── CreatePedidoCommandValidator.cs
│   │   │   └── UpdatePedidoCommandValidator.cs
│   │   ├── Filters/
│   │   │   └── PedidoFilters.cs              ← Vali-Flow builders
│   │   ├── DTOs/
│   │   │   ├── PedidoDto.cs
│   │   │   ├── CreatePedidoRequest.cs
│   │   │   └── UpdatePedidoRequest.cs
│   ├── Behaviors/
│   │   ├── LoggingBehavior.cs
│   │   └── ValidationBehavior.cs
│   ├── Mappings/
│   │   └── PedidoMappingExtensions.cs
│   └── Marker/
│       └── ApplicationAssemblyMarker.cs
│
├── at-prueba-tecnica-backend.Infrastructure/  ← Capa de Infraestructura
│   ├── bin/
│   ├── obj/
│   ├── Properties/
│   ├── at-prueba-tecnica-backend.Infrastructure.csproj
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── AppDbContextFactory.cs           ← Design-time factory para EF
│   │   ├── Configurations/
│   │   │   ├── PedidoConfiguration.cs
│   │   │   └── UsuarioConfiguration.cs
│   │   ├── Repositories/
│   │   │   ├── PedidoRepository.cs          ← Hereda DbRepositoryAsync<T>
│   │   │   └── UsuarioRepository.cs
│   │   ├── Seeds/
│   │   │   └── DataSeeding.cs               ← Seed de datos iniciales
│   │   └── Migrations/
│   │       └── (auto-generadas)
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
│   │   └── PedidosController.cs
│   ├── Middlewares/
│   │   ├── GlobalExceptionMiddleware.cs
│   │   └── ExceptionHandlingExtensions.cs
│   └── Extensions/
│       ├── MigrationExtensions.cs           ← Auto-apply migrations
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
- `Vali-Validation` v2.0.1
- `Vali-Validation.ValiMediator` v1.0.1
- `Vali-Mediator.Resilience` v1.0.1
- `Vali-Flow.Core` v2.0.1

### Infrastructure
- `Vali-Flow` v1.1.0
- `Microsoft.EntityFrameworkCore.SqlServer` v9.0.0
- `Microsoft.EntityFrameworkCore.Tools` v9.0.0
- `BCrypt.Net-Next` v4.0.3
- (+ referencia a Application)

### Api
- `Microsoft.AspNetCore.Authentication.JwtBearer` v9.0.0
- `Vali-Mediator.AspNetCore` v1.0.1
- (+ referencia a Infrastructure)

---

## 🎯 Flujo de una request HTTP

```
┌─────────────────────────────────────────────────────────────────┐
│  HTTP POST /api/pedidos                                         │
│  Authorization: Bearer eyJhbGc...                              │
│  { "numeroPedido": "PED-001", "total": 100 }                   │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  API Layer (Controllers)                                        │
│  - PedidosController.Create()                                  │
│  - Deserializa JSON → CreatePedidoCommand                      │
│  - Llama mediator.Send(command)                                │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Application Layer (Behaviors Pipeline)                         │
│  1. LoggingBehavior         → Log "START CreatePedidoCommand"  │
│  2. ValidationBehavior      → Vali-Validation                  │
│  3. ResilienceBehavior      → Circuit Breaker, Retry, etc      │
│  4. Siguiente en pipeline...                                   │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Application Layer (Handler)                                   │
│  - CreatePedidoCommandHandler.Handle()                         │
│  - Lógica de negocio: validar unicidad NumeroPedido           │
│  - Llamar repository.AddAsync()                                │
│  - Retornar Result<PedidoDto>                                  │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Infrastructure Layer (Repository)                             │
│  - PedidoRepository.AddAsync()                                 │
│  - Hereda de DbRepositoryAsync<T> (Vali-Flow)                 │
│  - Llamar DbContext.SaveChangesAsync()                         │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Persistence (EF Core → SQL Server)                            │
│  - INSERT INTO Pedidos (NumeroPedido, Total, ...) VALUES (...)│
│  - COMMIT                                                      │
└─────────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│  Return Result<PedidoDto> (éxito o error)                     │
│  → Mapeo automático a HTTP Status Code                        │
│  → 201 Created (si todo OK)                                    │
│  → 400 Bad Request (si validación falla)                       │
│  → 409 Conflict (si NumeroPedido duplicado)                    │
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
    ├── 4. Auto-apply EF Core migrations (MigrationExtensions)
    ├── 5. Seed datos iniciales (si no existen)
    └── 6. Listen en https://localhost:5000
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

## 📚 Próximos archivos a crear

Una vez ejecutes `init-project.sh`, necesitarás crear:

**Domain/**
- `Entities/Pedido.cs`
- `Entities/Usuario.cs`
- `Enums/EstadoPedido.cs`
- `Interfaces/IPedidoRepository.cs`
- `Interfaces/IUsuarioRepository.cs`

**Application/**
- `Auth/Commands/LoginCommand.cs`
- `Auth/Handlers/LoginCommandHandler.cs`
- `Auth/Validators/LoginCommandValidator.cs`
- `Pedidos/Commands/*`
- `Pedidos/Queries/*`
- `Pedidos/Handlers/*`
- `Pedidos/Validators/*`
- `Pedidos/Filters/PedidoFilters.cs`
- `Pedidos/DTOs/*`
- `Behaviors/LoggingBehavior.cs`
- `Mappings/PedidoMappingExtensions.cs`
- `Marker/ApplicationAssemblyMarker.cs`

**Infrastructure/**
- `Persistence/AppDbContext.cs`
- `Persistence/AppDbContextFactory.cs`
- `Persistence/Configurations/*`
- `Persistence/Repositories/PedidoRepository.cs`
- `Persistence/Repositories/UsuarioRepository.cs`
- `Auth/JwtTokenService.cs`
- `Auth/JwtSettings.cs`
- `DependencyInjection.cs`

**Api/**
- `appsettings.json`
- `Controllers/AuthController.cs`
- `Controllers/PedidosController.cs`
- `Middlewares/GlobalExceptionMiddleware.cs`
- `Extensions/MigrationExtensions.cs`
- `Program.cs` (configuración completa)

**Archivos de configuración:**
- `appsettings.json` (conexión BD, JWT)
- `appsettings.Development.json`

---

**¡Estructura lista para iniciar implementación!** 🚀
