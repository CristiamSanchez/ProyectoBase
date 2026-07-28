using Proyecto.Application.DTOs.Clientes;

namespace Proyecto.Application.Interfaces;

public interface IClienteService
{
    Task<List<ClienteResponseDto>> ObtenerClientesAsync();

    Task<ClienteResponseDto?> ObtenerClientePorIdAsync(int id);

    Task<ClienteResponseDto> CrearClienteAsync(
        ClienteCreateDto dto);

    Task<ClienteResponseDto?> ActualizarClienteAsync(
        int id,
        ClienteUpdateDto dto);

    Task<bool> EliminarClienteAsync(int id);
}