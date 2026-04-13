namespace at_prueba_tecnica_backend.Domain.Enums;

/// <summary>
/// Estados posibles de un pedido en el sistema.
/// </summary>
public enum EstadoPedido
{
    /// <summary>Pedido recibido, pendiente de procesamiento.</summary>
    Pendiente = 1,

    /// <summary>Pedido en proceso de preparación.</summary>
    Procesando = 2,

    /// <summary>Pedido completado y listo.</summary>
    Completado = 3,

    /// <summary>Pedido cancelado.</summary>
    Cancelado = 4
}
