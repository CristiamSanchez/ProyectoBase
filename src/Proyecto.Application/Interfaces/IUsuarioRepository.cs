using Proyecto.Domain.Entities;

namespace Proyecto.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorUsernameAsync(string username);

    Task<Usuario?> ObtenerPorIdAsync(int id);

    Task<IEnumerable<Usuario>> ObtenerTodosAsync();

    Task<Usuario> CrearAsync(Usuario usuario);
}
