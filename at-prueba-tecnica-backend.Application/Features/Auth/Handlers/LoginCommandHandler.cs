using at_prueba_tecnica_backend.Application.Features.Auth.Commands;
using at_prueba_tecnica_backend.Application.Features.Auth.Filters;
using at_prueba_tecnica_backend.Application.Features.Auth.Interfaces;
using at_prueba_tecnica_backend.Application.Features.DTOs;
using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using BCrypt.Net;
using Vali_Flow.Classes.Specification;
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
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IUserRepository userRepo, IJwtTokenService jwtService, ILogger<LoginCommandHandler> logger)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand command, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", command.Email);

            // Buscar usuario por email usando BasicSpecification
            var spec = new BasicSpecification<User>()
                .WithFilter(UserFilters.ByEmail(command.Email))
                .WithAsNoTracking(true);

            var user = await _userRepo.EvaluateGetFirstAsync(spec, ct);

            _logger.LogInformation("User found: {Found}", user != null);

            if (user is null)
                return Result<LoginResponseDto>.Fail("Credenciales inválidas", ErrorType.Unauthorized);

            // Validar contraseña con BCrypt
            _logger.LogInformation("Verifying password for user: {Email}", user.Email);
            var passwordValid = BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash);
            _logger.LogInformation("Password valid: {Valid}", passwordValid);

            if (!passwordValid)
                return Result<LoginResponseDto>.Fail("Credenciales inválidas", ErrorType.Unauthorized);

            // Generar JWT token
            _logger.LogInformation("Generating token for user: {Email}", user.Email);
            var token = _jwtService.GenerateToken(user);
            _logger.LogInformation("Token generated successfully");

            var response = new LoginResponseDto(token, user.Email, user.Name, user.Role.ToString());
            _logger.LogInformation("Login successful for: {Email}", user.Email);
            return Result<LoginResponseDto>.Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error for email: {Email}", command.Email);
            return Result<LoginResponseDto>.Fail($"Error al autenticar: {ex.Message}", ErrorType.Failure);
        }
    }
}
