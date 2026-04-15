using at_prueba_tecnica_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the User entity.
/// Defines table, indexes, constraints, conversions, and global query filter (soft delete).
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        // Id (UUID)
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        // Code (int, autoincrement)
        builder.Property(u => u.Code)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Audit properties

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UpdatedAt);

        builder.Property(u => u.DeletedAt);

        // Business properties
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        builder.HasIndex(u => u.Code)
            .IsUnique()
            .HasDatabaseName("IX_Users_Code");

        // Global query filter: exclude soft-deleted records
        // Fixed with Vali-Flow 1.3.4 and Vali-Flow.Core 2.0.2 which properly handle null field checks
        builder.HasQueryFilter(u => !u.DeletedAt.HasValue);
    }
}
