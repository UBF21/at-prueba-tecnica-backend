using at_prueba_tecnica_backend.Api.Extensions;
using at_prueba_tecnica_backend.Api.Responses;
using at_prueba_tecnica_backend.Application.Features.Auth.Commands;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using Microsoft.AspNetCore.Mvc;
using Vali_Mediator.Core.General.Mediator;

namespace at_prueba_tecnica_backend.Api.Controllers;

/// <summary>
/// Endpoints de autenticación.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IValiMediator _mediator;

    public AuthController(IValiMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Inicia sesión con email y contraseña.
    /// Retorna un JWT token válido por 60 minutos.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        var response = result.ToApiResponse();
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
