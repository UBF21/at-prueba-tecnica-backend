# Database Migration Instructions

## Overview

The backend has been refactored to use **Guid (UUID) as primary keys** with **int Code as sequential identifiers** for public references. A new **Customers** table has been created.

## What Changed

### Type Changes
- **User**: `Id: int → Guid`, `Code: string → int`
- **Product**: `Id: int → Guid`, `Code: string → int`
- **Order**: `Id: int → Guid`, `Code: string → int`, `CustomerId: int → Guid`
- **OrderItem**: `Id: int → Guid`, `Code: string → int`, `OrderId: int → Guid`, `ProductId: int → Guid`

### New Table
- **Customer**: `Id: Guid (PK)`, `Code: int (sequential)`, Name, Email, Phone, Address, audit timestamps

## How to Apply the Migration

### Option 1: Using dotnet ef (Recommended)

```bash
cd ~/RiderProjects/at-prueba-tecnica-backend

# Install EF Core CLI if needed
dotnet tool install --global dotnet-ef

# Generate the migration script
dotnet ef migrations script --from 0 --output migration.sql

# Apply the migration
dotnet ef database update
```

### Option 2: Manual SQL Execution

The migration file is located at:
```
at-prueba-tecnica-backend.Infrastructure/Persistence/Migrations/20260414_RefactorToGuidIdsWithIntCode.cs
```

If you need to execute the migration manually, extract the SQL from the Migration file and run it in SQL Server Management Studio.

## Important Notes

- ⚠️ This migration **cannot be safely reverted** (Down method throws NotSupportedException)
- All existing data will be migrated automatically
- New Guid IDs will be generated using `NEWID()` SQL function
- Sequential Code values will be auto-generated
- Admin user seed data has been updated with Guid Id and int Code

## Verification

After running the migration, verify:

1. All tables have been updated with new column types
2. Customers table exists with correct schema
3. Indexes are properly recreated
4. Foreign key constraints are in place

```sql
-- Check Customers table
SELECT * FROM Customers;

-- Check Users have Guid IDs
SELECT TOP 1 Id, Code, Email FROM Users;

-- Check Orders reference Customers correctly
SELECT TOP 1 o.Id, o.Code, o.CustomerId, c.Code as CustomerCode FROM Orders o 
LEFT JOIN Customers c ON o.CustomerId = c.Id;
```

## Next Steps

After migration:
1. Run the backend: `dotnet run` in the API project
2. Test endpoints with Postman or the frontend
3. Login at `http://localhost:5176` (frontend is running on port 5176)
4. Navigate to /customers to see the new Customer CRUD interface

## Troubleshooting

If the migration fails:
1. Check SQL Server is running: `sqlserver --version`
2. Verify connection string in `appsettings.json`
3. Check that the database exists and is accessible
4. Review the error message in the Output window

For questions or issues, refer to the backend logs or SQL Server error logs.
