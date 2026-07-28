using Proyecto.Domain.Entities;
using Proyecto.Application.DTOs.Clientes;

namespace Proyecto.Application.Interfaces;

public interface IClienteRepository
{
    Task<List<Cliente>> ObtenerClientesAsync();
    Task<Cliente?> ObtenerClientePorIdAsync(int id);
    Task<Cliente> CrearClienteAsync(Cliente cliente);
    
    Task<bool> EliminarClienteAsync(int id);
    
    Task<Cliente?> ActualizarClienteAsync(int id, ClienteUpdateDto dto);

}
