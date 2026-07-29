using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Services;
using Proyecto.Domain.Entities;
using Proyecto.Infrastructure.Data;

namespace Proyecto.IntegrationTests.Infrastructure;

public static class TestDataSeeder
{
    public static async Task SeedAsync(
        AppDbContext context)
    {
        var passwordService = new PasswordService();

        if (!await context.Usuarios.AnyAsync(
            u => u.Username == "admin"))
        {
            var admin = new Usuario
            {
                Id = 1,
                Username = "admin",
                PasswordHash =
                    passwordService.Hash("123456"),
                Rol = "Admin",
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            context.Usuarios.Add(admin);
        }

        if (!await context.Usuarios.AnyAsync(
            u => u.Username == "cristiam"))
        {
            var usuario = new Usuario
            {
                Id = 2,
                Username = "cristiam",
                PasswordHash =
                    passwordService.Hash("123456"),
                Rol = "User",
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            context.Usuarios.Add(usuario);
        }

        await context.SaveChangesAsync();
    }
}