using Microsoft.Extensions.DependencyInjection;
using Proyecto.Application.Interfaces;
using Proyecto.Application.Services;

namespace Proyecto.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IClienteService, ClienteService>();

        services.AddScoped<IProductoService, ProductoService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<PasswordService>();
        
        return services;
    }
}