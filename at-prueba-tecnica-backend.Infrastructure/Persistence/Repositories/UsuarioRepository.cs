using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Usuarios usando Entity Framework Core.
/// Proporciona operaciones CRUD y consultas específicas para la entidad Usuario.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Usuario> _dbSet;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Usuarios;
    }

    public async Task<List<Usuario>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<Usuario> AddAsync(Usuario usuario, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(usuario, ct);
        await _context.SaveChangesAsync(ct);
        return usuario;
    }

    public async Task UpdateAsync(Usuario usuario, CancellationToken ct = default)
    {
        _dbSet.Update(usuario);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Usuario usuario, CancellationToken ct = default)
    {
        _dbSet.Remove(usuario);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, ct);
    }
}
