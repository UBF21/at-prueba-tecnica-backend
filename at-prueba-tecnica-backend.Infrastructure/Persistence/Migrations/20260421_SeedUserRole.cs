using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Users
                SET CreatedAt = '2025-01-01T00:00:00.000',
                    PasswordHash = '$2a$11$xiggvcJ1Tfe.BU9otSp11uN6fYwMLRKTjFczQ6YwXrefuDZYPuUVe'
                WHERE Id = '550e8400-e29b-41d4-a716-446655440000';
                """);

            migrationBuilder.Sql("""
                SET IDENTITY_INSERT Users ON;
                IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = '550e8400-e29b-41d4-a716-446655440001')
                BEGIN
                    INSERT INTO Users (Id, Code, CreatedAt, DeletedAt, Email, Name, PasswordHash, Role, UpdatedAt)
                    VALUES (
                        '550e8400-e29b-41d4-a716-446655440001',
                        2,
                        '2025-01-01T00:00:00.000',
                        NULL,
                        'user@retopedidos.com',
                        'User',
                        '$2a$11$YOeN8Xrq4dODGu.vEJ6qheBASX91rNqmlxmG7/D2hM3hZYkqFVDr2',
                        'User',
                        NULL
                    );
                END
                SET IDENTITY_INSERT Users OFF;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Users WHERE Id = '550e8400-e29b-41d4-a716-446655440001';");
        }
    }
}
