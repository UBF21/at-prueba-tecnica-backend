using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de EF Core para la entidad Usuario.
/// Define tabla, índices, restricciones y global query filter (soft delete).
/// </summary>
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);

        // Propiedades de auditoría
        builder.Property(u => u.Code)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UpdatedAt);

        builder.Property(u => u.DeletedAt);

        // Propiedades de negocio
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Rol)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.FechaCreacion)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.FechaModificacion);

        builder.Property(u => u.Eliminado)
            .IsRequired()
            .HasDefaultValue(false);

        // Índices
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_Email");

        builder.HasIndex(u => u.Code)
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_Code");

        // Global query filter: excluye usuarios eliminados (DeletedAt != null)
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
