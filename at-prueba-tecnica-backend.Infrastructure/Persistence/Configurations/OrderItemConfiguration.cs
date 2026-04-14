using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the OrderItem entity.
/// Defines table, indexes, constraints, and global query filter (soft delete).
/// </summary>
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(oi => oi.Id);

        // Audit properties
        builder.Property(oi => oi.Code)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(oi => oi.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(oi => oi.UpdatedAt);

        builder.Property(oi => oi.DeletedAt);

        // Business properties
        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.ProductId)
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // Subtotal is computed — no database column, only in memory
        builder.Ignore(oi => oi.Subtotal);

        // Foreign keys
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(oi => oi.Code)
            .IsUnique()
            .HasDatabaseName("IX_OrderItems_Code");

        builder.HasIndex(oi => oi.OrderId)
            .HasDatabaseName("IX_OrderItems_OrderId");

        builder.HasIndex(oi => oi.ProductId)
            .HasDatabaseName("IX_OrderItems_ProductId");

        // Global query filter: exclude soft-deleted records
        builder.HasQueryFilter(oi => oi.DeletedAt == null);
    }
}
