using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de EF Core para la entidad Pedido.
/// Define tabla, índices, restricciones, conversiones y global query filter (soft delete).
/// </summary>
public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");
        builder.HasKey(p => p.Id);

        // Propiedades
        builder.Property(p => p.NumeroPedido)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Total)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Estado)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.ClienteId)
            .IsRequired();

        builder.Property(p => p.FechaCreacion)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.FechaModificacion);

        builder.Property(p => p.Eliminado)
            .IsRequired()
            .HasDefaultValue(false);

        // Índices
        builder.HasIndex(p => p.NumeroPedido)
            .IsUnique()
            .HasDatabaseName("IX_Pedidos_NumeroPedido");

        builder.HasIndex(p => p.ClienteId)
            .HasDatabaseName("IX_Pedidos_ClienteId");

        // Global query filter: excluye pedidos eliminados automáticamente
        builder.HasQueryFilter(p => !p.Eliminado);
    }
}
