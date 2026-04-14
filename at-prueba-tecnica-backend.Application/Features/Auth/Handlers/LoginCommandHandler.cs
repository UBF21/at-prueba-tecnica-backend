using at_prueba_tecnica_backend.Application.Features.Auth.Commands;
using at_prueba_tecnica_backend.Application.Features.Auth.Interfaces;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using BCrypt.Net;
using Vali_Flow.Classes.Specification;
using Vali_Flow.Core;
using Vali_Flow.Core.Builder;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Features.Auth.Handlers;

/// <summary>
/// Handler para el command LoginCommand.
/// Autentica un usuario comparando email y contraseña (con BCrypt).
/// Si es válido, genera y retorna un JWT token.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtTokenService _jwtService;

    public LoginCommandHandler(IUserRepository userRepo, IJwtTokenService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand command, CancellationToken ct)
    {
        try
        {
            // Buscar usuario por email usando BasicSpecification
            var spec = new BasicSpecification<User>()
                .WithFilter(new ValiFlowQuery<User>().EqualTo(u => u.Email, command.Email))
                .WithAsNoTracking(true);

            var user = await _userRepo.EvaluateGetFirstAsync(spec, ct);

            if (user is null)
                return Result<LoginResponseDto>.Fail("Credenciales inválidas", ErrorType.Unauthorized);

            // Validar contraseña con BCrypt
            if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
                return Result<LoginResponseDto>.Fail("Credenciales inválidas", ErrorType.Unauthorized);

            // Generar JWT token
            var token = _jwtService.GenerateToken(user);

            var response = new LoginResponseDto(token, user.Email, user.Name, user.Role.ToString());
            return Result<LoginResponseDto>.Ok(response);
        }
        catch (Exception ex)
        {
            return Result<LoginResponseDto>.Fail($"Error al autenticar: {ex.Message}", ErrorType.Failure);
        }
    }
}
