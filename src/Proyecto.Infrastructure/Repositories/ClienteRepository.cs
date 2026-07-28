using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.Entities;
using Proyecto.Infrastructure.Data;
using Proyecto.Application.DTOs.Clientes;

namespace Proyecto.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;


    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<List<Cliente>> ObtenerClientesAsync()
    {
        return await _context.Clientes
            .ToListAsync();
    }


    public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);
    }


    public async Task<Cliente> CrearClienteAsync(Cliente cliente)
    {
        _context.Clientes.Add(cliente);

        await _context.SaveChangesAsync();

        return cliente;
    }

    public async Task<bool> EliminarClienteAsync(int id)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
        {
            return false;
        }

        _context.Clientes.Remove(cliente);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Cliente?> ActualizarClienteAsync(int id, ClienteUpdateDto dto)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return null;

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.Email = dto.Email;
        cliente.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        return cliente;
    }

}