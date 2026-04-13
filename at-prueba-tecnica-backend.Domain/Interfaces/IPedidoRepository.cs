using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Domain.Interfaces;

/// <summary>
/// Contrato del repositorio de Pedidos.
///
/// Interfaz genérica de repositorio para Pedidos. La implementación específica
/// (usando Vali-Flow) estará en Infrastructure.
///
/// Esta interfaz es agnóstica a la tecnología de persistencia (ORM, BD, etc)
/// de modo que Domain no depende de frameworks externos.
/// </summary>
public interface IPedidoRepository
{
    /// <summary>Obtiene todos los pedidos.</summary>
    Task<List<Pedido>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Obtiene un pedido por ID.</summary>
    Task<Pedido?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Agrega un nuevo pedido.</summary>
    Task<Pedido> AddAsync(Pedido pedido, CancellationToken ct = default);

    /// <summary>Actualiza un pedido existente.</summary>
    Task UpdateAsync(Pedido pedido, CancellationToken ct = default);

    /// <summary>Elimina un pedido (físicamente).</summary>
    Task DeleteAsync(Pedido pedido, CancellationToken ct = default);

    /// <summary>Verifica si existe un pedido con el número especificado.</summary>
    Task<bool> ExistsByNumeroPedidoAsync(string numeroPedido, CancellationToken ct = default);
}
