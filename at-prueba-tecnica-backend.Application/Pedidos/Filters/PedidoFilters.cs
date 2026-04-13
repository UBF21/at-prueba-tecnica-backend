using at_prueba_tecnica_backend.Domain.Entities;
using Vali_Flow.Core.Builder;

namespace at_prueba_tecnica_backend.Application.Pedidos.Filters;

/// <summary>
/// Filtros reutilizables para consultas de Pedidos usando Vali-Flow.
/// Construye dinámicamente expresiones LINQ sin dependencias externas.
/// </summary>
public static class PedidoFilters
{
    /// <summary>
    /// Filtro base: excluye pedidos eliminados (soft delete).
    /// </summary>
    public static ValiFlow<Pedido> Activos() =>
        new ValiFlow<Pedido>().IsFalse(p => p.Eliminado);

    /// <summary>
    /// Filtro por estado (condicional en tiempo de construcción).
    /// Si estado es null o vacío, retorna solo el filtro base (Activos).
    /// </summary>
    public static ValiFlow<Pedido> PorEstado(string? estado)
    {
        var filter = Activos();
        if (!string.IsNullOrEmpty(estado))
            filter = filter.And().EqualTo(p => p.Estado.ToString(), estado);
        return filter;
    }

    /// <summary>
    /// Filtro por ID de pedido.
    /// </summary>
    public static ValiFlow<Pedido> PorId(int id) =>
        Activos().And().EqualTo(p => p.Id, id);

    /// <summary>
    /// Filtro por número de pedido (para validar unicidad).
    /// </summary>
    public static ValiFlow<Pedido> ConNumero(string numero) =>
        new ValiFlow<Pedido>().EqualTo(p => p.NumeroPedido, numero);

    /// <summary>
    /// Filtro por ClienteId.
    /// </summary>
    public static ValiFlow<Pedido> PorClienteId(int clienteId) =>
        Activos().And().EqualTo(p => p.ClienteId, clienteId);
}
