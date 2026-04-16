# Database Initialization (NOT EF Core Migrations)

## ⚠️ Important: Project Uses EnsureCreated, NOT Migrations

**This project does NOT use `dotnet ef migrations` for database updates.** 

Instead:
- The database schema is defined entirely in **EF Core Fluent API** configurations (`*Configuration.cs` files)
- Each time the application starts, it:
  1. **Deletes** the existing database (`EnsureDeletedAsync()`)
  2. **Recreates** the schema from scratch (`EnsureCreatedAsync()`)
  3. **Seeds** initial data in `AppDbContext.OnModelCreating()`

This means:
- ✅ **All schema changes are automatic** — just modify the `*Configuration.cs` files
- ✅ **No manual migration scripts needed**
- ✅ **Development database always matches EF Core definitions**
- ❌ **Data does NOT persist** between application restarts (by design)

## What's Currently in the Schema

### Entities (All use Guid as PK + int Code)
- **User**: Auth users with JWT support
- **Customer**: Customer information
- **Product**: Product catalog
- **Order**: Orders (linked to Customers)
- **OrderItem**: Line items in orders

### Schema Features
- **Soft delete**: All tables have `DeletedAt` nullable timestamp
- **Global filters**: `HasQueryFilter(e => !e.DeletedAt.HasValue)` excludes soft-deleted records
- **Audit columns**: `CreatedAt`, `UpdatedAt`, `DeletedAt` on all entities
- **Sequential codes**: `Code: int` auto-incremented for user-friendly references

## For Development

No action needed. Just start the backend:

```bash
# Docker (includes BD recreation)
docker-compose up -d --build

# Or local
docker-compose up -d sqlserver
dotnet run --project at-prueba-tecnica-backend.Api
```

The database will be automatically created and seeded.

## To Modify the Schema

1. **Edit** the entity configuration in `Infrastructure/Persistence/Configurations/*Configuration.cs`
2. **Restart** the backend
3. The database will be recreated with your changes

Example: To add a new column to `Customer`:

```csharp
// In CustomerConfiguration.cs
builder.Property(c => c.NewColumn)
    .HasMaxLength(100);
```

Then restart the application and the schema updates automatically.

## Verification After Startup

Once the backend starts, the database is ready:

```sql
-- Check tables exist
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo';

-- Check seed admin user
SELECT Id, Code, Email FROM Users WHERE Email = 'admin@retopedidos.com';

-- Check soft delete filter works
SELECT COUNT(*) FROM Customers;  -- Counts only non-deleted
SELECT COUNT(*) FROM Customers WITH (NOLOCK) WHERE DeletedAt IS NOT NULL;  -- Manually check deleted
```

## Testing the API

After the backend starts:

1. **Login** with admin credentials:
   ```bash
   curl -X POST http://localhost:5001/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"email":"admin@retopedidos.com","password":"Admin123!"}'
   ```

2. **View API Docs**: http://localhost:5001/scalar

3. **Test CRUD endpoints**:
   - GET `/api/customers` - List customers
   - POST `/api/customers` - Create customer
   - GET `/api/orders` - List orders
   - POST `/api/products` - Create product

## Troubleshooting

### "Connection string error" or "Cannot connect to database"
```bash
# Verify SQL Server is running
docker ps | grep reto_sqlserver

# Check logs
docker logs reto_sqlserver

# Manual test
docker exec -it reto_sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "SqlServer123!" -Q "SELECT 1"
```

### "Tables don't exist"
- Database should auto-create on startup
- Check logs: `docker logs <backend-container>`
- Ensure `AppDbContext.OnModelCreating()` is executing

### "Seed data missing"
- Admin user seeds automatically in `AppDbContext.OnModelCreating()`
- If missing, check the code in `AppDbContext.cs` lines 28-38
