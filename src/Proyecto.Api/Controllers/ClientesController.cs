using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Proyecto.Api.Models;
using Proyecto.Application.DTOs.Clientes;
using Proyecto.Application.Interfaces;

namespace Proyecto.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableRateLimiting("authenticated")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(
        IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerClientes()
    {
        var clientes =
            await _clienteService.ObtenerClientesAsync();

        return Ok(
            ApiResponse<List<ClienteResponseDto>>
                .Ok(clientes)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerClientePorId(
        int id)
    {
        var cliente =
            await _clienteService
                .ObtenerClientePorIdAsync(id);

        if (cliente == null)
        {
            return NotFound();
        }

        return Ok(
            ApiResponse<ClienteResponseDto>
                .Ok(cliente)
        );
    }

    [HttpPost]
    public async Task<IActionResult> CrearCliente(
        ClienteCreateDto dto)
    {
        var cliente =
            await _clienteService.CrearClienteAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerClientePorId),
            new { id = cliente.Id },
            ApiResponse<ClienteResponseDto>
                .Ok(
                    cliente,
                    "Cliente creado correctamente"
                )
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarCliente(
        int id,
        ClienteUpdateDto dto)
    {
        var cliente =
            await _clienteService
                .ActualizarClienteAsync(id, dto);

        if (cliente == null)
        {
            return NotFound();
        }

        return Ok(
            ApiResponse<ClienteResponseDto>
                .Ok(
                    cliente,
                    "Cliente actualizado correctamente"
                )
        );
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarCliente(
        int id)
    {
        var eliminado =
            await _clienteService
                .EliminarClienteAsync(id);

        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }
}