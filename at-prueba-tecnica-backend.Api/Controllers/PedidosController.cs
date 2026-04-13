using at_prueba_tecnica_backend.Application.Pedidos.Commands;
using at_prueba_tecnica_backend.Application.Pedidos.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vali_Mediator.AspNetCore;
using Vali_Mediator.Core.General;
using Vali_Mediator.Core.General.Mediator;

namespace at_prueba_tecnica_backend.Api.Controllers;

/// <summary>
/// Endpoints de gestión de pedidos.
/// Requiere autenticación JWT (Bearer token).
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IValiMediator _mediator;

    public PedidosController(IValiMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los pedidos con paginación y filtro de estado opcional.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPedidos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? estado = null,
        CancellationToken ct = default)
    {
        var query = new GetPedidosQuery(page, pageSize, estado);
        var result = await _mediator.Send(query, ct);
        return result.ToActionResult
    }

    /// <summary>
    /// Obtiene un pedido específico por ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPedidoById(int id, CancellationToken ct)
    {
        var query = new GetPedidoByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result.ToActionResult();
    }

    /// <summary>
    /// Crea un nuevo pedido.
    /// NumeroPedido debe ser único.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreatePedido(
        [FromBody] CreatePedidoCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.ToActionResult(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Actualiza un pedido existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePedido(
        int id,
        [FromBody] UpdatePedidoCommand command,
        CancellationToken ct)
    {
        var cmd = command with { Id = id };
        var result = await _mediator.Send(cmd, ct);
        return result.ToActionResult();
    }

    /// <summary>
    /// Elimina un pedido (soft delete).
    /// El pedido se marca como eliminado pero no se borra de la BD.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedido(int id, CancellationToken ct)
    {
        var command = new DeletePedidoCommand(id);
        var result = await _mediator.Send(command, ct);
        return result.ToActionResult();
    }
}
