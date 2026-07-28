using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.Entities;
using Proyecto.Application.DTOs.Productos;
using Proyecto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace Proyecto.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableRateLimiting("authenticated")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var productos = await _productoService.GetAllAsync();

        return Ok(ApiResponse<IEnumerable<ProductoResponseDto>>
        .Ok(productos));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _productoService.GetByIdAsync(id);

        if (producto == null)
            return NotFound();

        return Ok(   ApiResponse<ProductoResponseDto>.Ok(producto));

    }


    [HttpPost]
    public async Task<IActionResult> Create(  ProductoCreateDto dto)
    {
        var creado = await _productoService.CreateAsync(dto);

        return CreatedAtAction(
        nameof(GetById),
        new { id = creado.Id },
        ApiResponse<ProductoResponseDto>.Ok(creado, "Producto creado correctamente"));
        
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        ProductoCreateDto dto)
    {
        await _productoService.UpdateAsync(id, dto);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productoService.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(   int page = 1,   int pageSize = 10,    string? search = null)
    {
        var productos =
            await _productoService.GetPagedAsync( page,       pageSize,    search);

        return Ok(productos);
    }

}