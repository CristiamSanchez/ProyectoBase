using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;

namespace Proyecto.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<Producto> Productos => Set<Producto>();
	
    public DbSet<Usuario> Usuarios => Set<Usuario>();


}
