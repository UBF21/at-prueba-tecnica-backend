using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Application.Features.Auth.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
