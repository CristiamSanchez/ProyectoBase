using Proyecto.Domain.Entities;

namespace Proyecto.Application.Interfaces;

public interface IAuthRepository
{
    Task<Usuario?> ObtenerPorUsernameAsync(string username);
}