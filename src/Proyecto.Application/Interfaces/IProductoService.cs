using Proyecto.Domain.Entities;
using Proyecto.Application.DTOs.Productos;
using Proyecto.Application.DTOs.Common; 


namespace Proyecto.Application.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoResponseDto>> GetAllAsync();

    Task<ProductoResponseDto?> GetByIdAsync(int id);

    Task<ProductoResponseDto> CreateAsync( ProductoCreateDto dto);

    Task<PagedResultDto<ProductoResponseDto>>GetPagedAsync(    int page,    int pageSize,    string? search);

    Task UpdateAsync(   int id,  ProductoCreateDto dto);

    Task DeleteAsync(int id);
}