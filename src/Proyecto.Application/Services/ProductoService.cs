using Proyecto.Application.Interfaces;
using Proyecto.Domain.Entities;
using AutoMapper;
using Proyecto.Application.DTOs.Productos;
using Proyecto.Application.DTOs.Common;

namespace Proyecto.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;
    private readonly IMapper _mapper;
    public ProductoService(IProductoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


   public async Task<IEnumerable<ProductoResponseDto>> GetAllAsync()
    {
        var productos = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProductoResponseDto>>(productos);
    }


    public async Task<ProductoResponseDto?> GetByIdAsync(int id)
    {
        var producto = await _repository.GetByIdAsync(id);

        if (producto == null)
            return null;

        return _mapper.Map<ProductoResponseDto>(producto);
    }
  
    public async Task<ProductoResponseDto> CreateAsync(ProductoCreateDto dto)
    {
       var producto = _mapper.Map<Producto>(dto);
       producto = await _repository.AddAsync(producto);
       return _mapper.Map<ProductoResponseDto>(producto);
    }


    public async Task UpdateAsync(    int id,    ProductoCreateDto dto)
    {
        var producto = await _repository.GetByIdAsync(id);

        if (producto == null)
            return;


        _mapper.Map(dto, producto);

        await _repository.UpdateAsync(producto);
    }


    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<ProductoResponseDto>> GetPagedAsync(     int page,   int pageSize,     string? search)
    {
        var result =
            await _repository.GetPagedAsync(
                page,
                pageSize,
                search);


        return new PagedResultDto<ProductoResponseDto>
        {
            Items = _mapper.Map<IEnumerable<ProductoResponseDto>>
            (
                result.Items
            ),

            TotalItems = result.Total,

            Page = page,

            PageSize = pageSize
        };
    }

}