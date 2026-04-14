using System.Linq;
using Vali_Flow.Core;
using Vali_Flow.Classes.Specification;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Application.Features.Mappings;
using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using at_prueba_tecnica_backend.Application.Features.Orders.Filters;
using at_prueba_tecnica_backend.Application.Features.Products.Filters;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Enums;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Abstractions;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Handlers;

/// <summary>
/// Handler for CreateOrderItemCommand.
/// Validates order state, product stock, creates the item, and recalculates the order total.
/// </summary>
public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;
    private readonly IOrderItemRepository _itemRepo;

    public CreateOrderItemCommandHandler(
        IOrderRepository orderRepo,
        IProductRepository productRepo,
        IOrderItemRepository itemRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _itemRepo = itemRepo;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderItemCommand command, CancellationToken ct)
    {
        try
        {
            // Load order
            var orderSpec = new BasicSpecification<Order>()
                .WithFilter(OrderFilters.ById(command.OrderId));

            var order = await _orderRepo.EvaluateGetFirstAsync(orderSpec, ct);

            if (order is null)
                return Result<OrderDto>.Fail("Order not found", ErrorType.NotFound);

            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Delivered)
                return Result<OrderDto>.Fail("Cannot add items to a cancelled or delivered order", ErrorType.Conflict);

            // Load product
            var productSpec = new BasicSpecification<Product>()
                .WithFilter(ProductFilters.ById(command.ProductId));

            var product = await _productRepo.EvaluateGetFirstAsync(productSpec, ct);

            if (product is null)
                return Result<OrderDto>.Fail("Product not found", ErrorType.NotFound);

            if (product.Stock < command.Quantity)
                return Result<OrderDto>.Fail(
                    $"Insufficient stock. Available: {product.Stock}, Requested: {command.Quantity}",
                    ErrorType.Conflict);

            // Create item with price snapshot
            var item = new OrderItem
            {
                Code = Guid.NewGuid().ToString("N").Substring(0, 36),
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = command.Quantity,
                UnitPrice = product.UnitPrice,
                CreatedAt = DateTime.UtcNow
            };

            // Add item to order collection (in memory)
            order.Items.Add(item);

            // Decrement product stock
            product.Stock -= command.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            // Recalculate order total
            order.RecalculateTotal();
            order.UpdatedAt = DateTime.UtcNow;

            // Save all changes at once (product, order, and item via order navigation)
            await _productRepo.UpdateAsync(product, saveChanges: false, ct);
            await _orderRepo.UpdateAsync(order, saveChanges: true, ct);

            // Return updated order with items
            return Result<OrderDto>.Ok(order.ToDto(includeItems: true));
        }
        catch (Exception ex)
        {
            return Result<OrderDto>.Fail($"Error adding item to order: {ex.Message}", ErrorType.Failure);
        }
    }
}
