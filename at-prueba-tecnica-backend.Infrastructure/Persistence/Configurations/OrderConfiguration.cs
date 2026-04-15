using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Order entity.
/// Defines table, indexes, constraints, conversions, and global query filter (soft delete).
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);

        // Id (UUID)
        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        // Code (int, autoincrement) - readonly after save
        builder.Property(o => o.Code)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        // Audit properties

        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(o => o.UpdatedAt);

        builder.Property(o => o.DeletedAt);

        // Business properties
        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Total)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.CustomerId)
            .IsRequired();

        // Navigation
        builder.HasMany(o => o.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(o => o.OrderNumber)
            .IsUnique()
            .HasDatabaseName("IX_Orders_OrderNumber");

        builder.HasIndex(o => o.Code)
            .IsUnique()
            .HasDatabaseName("IX_Orders_Code");

        builder.HasIndex(o => o.CustomerId)
            .HasDatabaseName("IX_Orders_CustomerId");

        // Global query filter: exclude soft-deleted records
        // Fixed with Vali-Flow 1.3.4 and Vali-Flow.Core 2.0.2 which properly handle null field checks
        builder.HasQueryFilter(o => !o.DeletedAt.HasValue);
    }
}
