using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Customer entity.
/// Defines table, indexes, constraints, conversions, and global query filter (soft delete).
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);

        // Id (UUID)
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        // Code (int, autoincrement) - readonly after save
        builder.Property(c => c.Code)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        // Audit properties

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.DeletedAt);

        // Business properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        // Navigation
        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Email");

        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Code");

        builder.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Customers_Name");

        // Global query filter for soft delete
        // Fixed with Vali-Flow 1.3.4 and Vali-Flow.Core 2.0.2 which properly handle null field checks
        builder.HasQueryFilter(c => !c.DeletedAt.HasValue);
    }
}
