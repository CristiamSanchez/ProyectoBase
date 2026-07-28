using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proyecto.Infrastructure.Data;
using Proyecto.Application.Interfaces;
using Proyecto.Infrastructure.Repositories;

namespace Proyecto.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();        
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        

        return services;
    }
}