using Proyecto.Domain.Entities;

namespace Proyecto.Application.Interfaces;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAllAsync();

    Task<Producto?> GetByIdAsync(int id);

    Task<Producto> AddAsync(Producto producto);

    Task UpdateAsync(Producto producto);

    Task DeleteAsync(int id);


    Task<(IEnumerable<Producto> Items, int Total)>
        GetPagedAsync(
            int page,
            int pageSize,
            string? search);
}