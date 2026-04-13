using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Pedidos usando Entity Framework Core.
/// Proporciona operaciones CRUD y consultas específicas para la entidad Pedido.
/// </summary>
public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Pedido> _dbSet;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Pedidos;
    }

    public async Task<List<Pedido>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public async Task<Pedido?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Pedido> AddAsync(Pedido pedido, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(pedido, ct);
        await _context.SaveChangesAsync(ct);
        return pedido;
    }

    public async Task UpdateAsync(Pedido pedido, CancellationToken ct = default)
    {
        _dbSet.Update(pedido);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Pedido pedido, CancellationToken ct = default)
    {
        _dbSet.Remove(pedido);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNumeroPedidoAsync(string numeroPedido, CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(p => p.NumeroPedido == numeroPedido, ct);
    }
}
