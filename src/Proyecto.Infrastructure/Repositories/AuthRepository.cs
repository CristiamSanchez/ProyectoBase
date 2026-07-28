using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.Entities;
using Proyecto.Infrastructure.Data;

namespace Proyecto.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u =>
                u.Username == username &&
                u.Activo);
    }
}