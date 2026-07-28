using AutoMapper;
using Proyecto.Domain.Entities;
using Proyecto.Application.DTOs.Clientes;

namespace Proyecto.Application.Mappings;

public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        // Entity → Response DTO
        CreateMap<Cliente, ClienteResponseDto>()
            .ForMember(
                dest => dest.NombreCompleto,
                opt => opt.MapFrom(
                    src => src.Nombre + " " + src.Apellido
                )
            );

        // Create DTO → Entity
        CreateMap<ClienteCreateDto, Cliente>();

        // Update DTO → Entity
        CreateMap<ClienteUpdateDto, Cliente>();
    }
}