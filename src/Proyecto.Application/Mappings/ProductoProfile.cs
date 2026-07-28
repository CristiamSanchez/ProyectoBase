using AutoMapper;
using Proyecto.Application.DTOs.Productos;
using Proyecto.Domain.Entities;

namespace Proyecto.Application.Mappings;

public class ProductoProfile : Profile
{
    public ProductoProfile()
    {
        CreateMap<Producto, ProductoResponseDto>();

        CreateMap<ProductoCreateDto, Producto>();

        CreateMap<ProductoUpdateDto, Producto>();
    }
}