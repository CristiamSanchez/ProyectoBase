using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.Entities;
using Proyecto.Infrastructure.Data;

namespace Proyecto.Infrastructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Productos
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id);
    }


    public async Task<Producto> AddAsync(Producto producto)
    {
        await _context.Productos.AddAsync(producto);

        await _context.SaveChangesAsync();

        return producto;
    }


    public async Task UpdateAsync(Producto producto)
    {
        _context.Productos.Update(producto);

        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var producto = await GetByIdAsync(id);

        if (producto == null)
            return;

        _context.Productos.Remove(producto);

        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Producto> Items, int Total)> GetPagedAsync( int page, int pageSize, string? search)
    {
        var query = _context.Productos
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Nombre.Contains(search));
        }

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

}