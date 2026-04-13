using at_prueba_tecnica_backend.Application.Auth.Commands;
using at_prueba_tecnica_backend.Application.Auth.Interfaces;
using at_prueba_tecnica_backend.Application.DTOs;
using at_prueba_tecnica_backend.Domain.Interfaces;
using BCrypt.Net;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;

namespace at_prueba_tecnica_backend.Application.Auth.Handlers;

/// <summary>
/// Handler para el command LoginCommand.
/// Autentica un usuario comparando email y contraseña (con BCrypt).
/// Si es válido, genera y retorna un JWT token.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IJwtTokenService _jwtService;

    public LoginCommandHandler(IUsuarioRepository usuarioRepo, IJwtTokenService jwtService)
    {
        _usuarioRepo = usuarioRepo;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand command, CancellationToken ct)
    {
        try
        {
            // Buscar usuario por email (obtenemos todos y filtramos en memoria por ahora)
            var usuarios = await _usuarioRepo.GetAllAsync(ct);
            var usuario = usuarios.FirstOrDefault(u => u.Email == command.Email && !u.Eliminado);

            if (usuario is null)
                return Result<LoginResponseDto>.Fail("Credenciales inválidas", ErrorType.Unauthorized);

            // Validar contraseña con BCrypt
            if (!BCrypt.Net.BCrypt.Verify(command.Password, usuario.PasswordHash))
                return Result<LoginResponseDto>.Fail("Credenciales inválidas", ErrorType.Unauthorized);

            // Generar JWT token
            var token = _jwtService.GenerateToken(usuario);

            var response = new LoginResponseDto(token, usuario.Email, usuario.Nombre);
            return Result<LoginResponseDto>.Ok(response);
        }
        catch (Exception ex)
        {
            return Result<LoginResponseDto>.Fail($"Error al autenticar: {ex.Message}", ErrorType.Failure);
        }
    }
}
