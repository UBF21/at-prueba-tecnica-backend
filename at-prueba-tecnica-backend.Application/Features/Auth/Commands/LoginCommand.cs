using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Auth.Commands;

/// <summary>
/// Command para autenticar un usuario y obtener un JWT token.
/// </summary>
public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponseDto>>;
