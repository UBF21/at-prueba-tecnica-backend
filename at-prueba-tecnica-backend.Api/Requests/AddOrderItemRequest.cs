namespace at_prueba_tecnica_backend.Api.Requests;

/// <summary>
/// Request body for adding an item to an order.
/// </summary>
public record AddOrderItemRequest(int ProductId, int Quantity);
