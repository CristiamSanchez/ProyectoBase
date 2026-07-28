using AutoMapper;
using Proyecto.Application.DTOs.Clientes;
using Proyecto.Application.Interfaces;
using Proyecto.Domain.Entities;

namespace Proyecto.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;
    private readonly IMapper _mapper;

    public ClienteService(
        IClienteRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ClienteResponseDto>> ObtenerClientesAsync()
    {
        var clientes =
            await _repository.ObtenerClientesAsync();

        return _mapper.Map<List<ClienteResponseDto>>(clientes);
    }

    public async Task<ClienteResponseDto?> ObtenerClientePorIdAsync(int id)
    {
        var cliente =
            await _repository.ObtenerClientePorIdAsync(id);

        if (cliente == null)
        {
            return null;
        }

        return _mapper.Map<ClienteResponseDto>(cliente);
    }

    public async Task<ClienteResponseDto> CrearClienteAsync(
        ClienteCreateDto dto)
    {
        var cliente =
            _mapper.Map<Cliente>(dto);

        var clienteCreado =
            await _repository.CrearClienteAsync(cliente);

        return _mapper.Map<ClienteResponseDto>(
            clienteCreado);
    }

    public async Task<ClienteResponseDto?> ActualizarClienteAsync(
        int id,
        ClienteUpdateDto dto)
    {
        var clienteActualizado =
            await _repository.ActualizarClienteAsync(
                id,
                dto);

        if (clienteActualizado == null)
        {
            return null;
        }

        return _mapper.Map<ClienteResponseDto>(
            clienteActualizado);
    }

    public async Task<bool> EliminarClienteAsync(int id)
    {
        return await _repository
            .EliminarClienteAsync(id);
    }
}