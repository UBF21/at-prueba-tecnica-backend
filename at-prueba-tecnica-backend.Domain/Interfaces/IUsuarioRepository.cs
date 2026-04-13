using at_prueba_tecnica_backend.Domain.Entities;

namespace at_prueba_tecnica_backend.Domain.Interfaces;

/// <summary>
/// Contrato del repositorio de Usuarios.
///
/// Interfaz genérica de repositorio para Usuarios. La implementación específica
/// (usando Vali-Flow) estará en Infrastructure.
///
/// Esta interfaz es agnóstica a la tecnología de persistencia (ORM, BD, etc)
/// de modo que Domain no depende de frameworks externos.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>Obtiene todos los usuarios.</summary>
    Task<List<Usuario>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Obtiene un usuario por ID.</summary>
    Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Agrega un nuevo usuario.</summary>
    Task<Usuario> AddAsync(Usuario usuario, CancellationToken ct = default);

    /// <summary>Actualiza un usuario existente.</summary>
    Task UpdateAsync(Usuario usuario, CancellationToken ct = default);

    /// <summary>Elimina un usuario (lógicamente).</summary>
    Task DeleteAsync(Usuario usuario, CancellationToken ct = default);

    /// <summary>Verifica si existe un usuario con el email especificado.</summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
}
