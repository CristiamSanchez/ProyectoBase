using FluentAssertions;
using Moq;
using Proyecto.Application.DTOs.Clientes;
using Proyecto.Application.Interfaces;
using Proyecto.Application.Services;
using Proyecto.Domain.Entities;
using Xunit;

namespace Proyecto.Tests.Services;

public class ClienteServiceTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<AutoMapper.IMapper> _mapperMock;

    private readonly ClienteService _service;


    public ClienteServiceTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();

        _mapperMock = new Mock<AutoMapper.IMapper>();

        _service = new ClienteService(
            _repositoryMock.Object,
            _mapperMock.Object);
    }


    // AQUÍ DEBAJO DEL CONSTRUCTOR VA EL PRIMER TEST


    [Fact]
    public async Task ObtenerClientesAsync_DebeRetornarClientes()
    {
        // Arrange

        var clientes = new List<Cliente>
        {
            new Cliente
            {
                Id = 1,
                Nombre = "Cristiam",
                Apellido = "Sanchez",
                Email = "test@test.com"
            }
        };


        var clientesDto = new List<ClienteResponseDto>
        {
            new ClienteResponseDto
            {
                Id = 1,
                NombreCompleto = "Cristiam Sanchez",
                Email = "test@test.com",
                Activo = true
            }
        };


        _repositoryMock
            .Setup(x => x.ObtenerClientesAsync())
            .ReturnsAsync(clientes);


        _mapperMock
            .Setup(x => x.Map<List<ClienteResponseDto>>(clientes))
            .Returns(clientesDto);



        // Act

        var resultado =
            await _service.ObtenerClientesAsync();



        // Assert

        resultado.Should()
            .NotBeNull();


        resultado.Should()
            .HaveCount(1);


       resultado[0].NombreCompleto
            .Should()
            .Be("Cristiam Sanchez");
    }

    [Fact]
    public async Task ObtenerClientePorIdAsync_ClienteExiste_DebeRetornarCliente()
    {
        // Arrange

        var cliente = new Cliente
        {
            Id = 1,
            Nombre = "Cristiam",
            Apellido = "Sanchez",
            Email = "test@test.com"
        };


        var clienteDto = new ClienteResponseDto
        {
            Id = 1,
            NombreCompleto = "Cristiam Sanchez",
            Email = "test@test.com",
            Activo = true
        };


        _repositoryMock
            .Setup(x => x.ObtenerClientePorIdAsync(1))
            .ReturnsAsync(cliente);


        _mapperMock
            .Setup(x => x.Map<ClienteResponseDto>(cliente))
            .Returns(clienteDto);



        // Act

        var resultado =
            await _service.ObtenerClientePorIdAsync(1);



        // Assert

        resultado.Should()
            .NotBeNull();


        resultado!.Id
            .Should()
            .Be(1);


        resultado.NombreCompleto
            .Should()
            .Be("Cristiam Sanchez");
    }

    [Fact]
    public async Task ObtenerClientePorIdAsync_ClienteNoExiste_DebeRetornarNull()
    {
        // Arrange

        _repositoryMock
            .Setup(x => x.ObtenerClientePorIdAsync(999))
            .ReturnsAsync((Cliente?)null);



        // Act

        var resultado =
            await _service.ObtenerClientePorIdAsync(999);



        // Assert

        resultado.Should()
            .BeNull();
    }

    [Fact]
    public async Task CrearClienteAsync_DatosValidos_DebeCrearCliente()
    {
        // Arrange

        var dto = new ClienteCreateDto
        {
            Nombre = "Cristiam",
            Apellido = "Sanchez",
            Email = "cristiam@test.com"
        };

        var clienteCreado = new Cliente
        {
            Id = 1,
            Nombre = "Cristiam",
            Apellido = "Sanchez",
            Email = "cristiam@test.com"
        };

        var clienteResponse = new ClienteResponseDto
        {
            Id = 1,
            NombreCompleto = "Cristiam Sanchez",
            Email = "cristiam@test.com",
            Activo = true
        };

        _mapperMock
            .Setup(x => x.Map<Cliente>(dto))
            .Returns(new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email
            });

        _repositoryMock
            .Setup(x =>
                x.CrearClienteAsync(
                    It.IsAny<Cliente>()))
            .ReturnsAsync(clienteCreado);

        _mapperMock
            .Setup(x =>
                x.Map<ClienteResponseDto>(
                    clienteCreado))
            .Returns(clienteResponse);


        // Act

        var resultado =
            await _service.CrearClienteAsync(dto);


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado.Id
            .Should()
            .Be(1);

        resultado.NombreCompleto
            .Should()
            .Be("Cristiam Sanchez");

        resultado.Email
            .Should()
            .Be("cristiam@test.com");


        _repositoryMock.Verify(
            x => x.CrearClienteAsync(
                It.Is<Cliente>(c =>
                    c.Nombre == "Cristiam" &&
                    c.Apellido == "Sanchez" &&
                    c.Email == "cristiam@test.com")),
            Times.Once);
    }

    [Fact]
    public async Task ActualizarClienteAsync_DatosValidos_DebeActualizarCliente()
    {
        // Arrange

        var dto = new ClienteUpdateDto
        {
            Nombre = "Cristiam Actualizado",
            Apellido = "Sanchez",
            Email = "nuevo@test.com"
        };

        var clienteActualizado = new Cliente
        {
            Id = 1,
            Nombre = "Cristiam Actualizado",
            Apellido = "Sanchez",
            Email = "nuevo@test.com"
        };

        var clienteResponse = new ClienteResponseDto
        {
            Id = 1,
            NombreCompleto = "Cristiam Actualizado Sanchez",
            Email = "nuevo@test.com",
            Activo = true
        };

        _repositoryMock
            .Setup(x =>
                x.ActualizarClienteAsync(1, dto))
            .ReturnsAsync(clienteActualizado);

        _mapperMock
            .Setup(x =>
                x.Map<ClienteResponseDto>(
                    clienteActualizado))
            .Returns(clienteResponse);


        // Act

        var resultado =
            await _service.ActualizarClienteAsync(
                1,
                dto);


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado!.Id
            .Should()
            .Be(1);

        resultado.NombreCompleto
            .Should()
            .Be("Cristiam Actualizado Sanchez");

        resultado.Email
            .Should()
            .Be("nuevo@test.com");


        _repositoryMock.Verify(
            x =>
                x.ActualizarClienteAsync(
                    1,
                    dto),
            Times.Once);
    }

    [Fact]
    public async Task ActualizarClienteAsync_ClienteNoExiste_DebeRetornarNull()
    {
        // Arrange

        var dto = new ClienteUpdateDto
        {
            Nombre = "Cristiam",
            Apellido = "Sanchez",
            Email = "nuevo@test.com"
        };

        _repositoryMock
            .Setup(x =>
                x.ActualizarClienteAsync(999, dto))
            .ReturnsAsync((Cliente?)null);


        // Act

        var resultado =
            await _service.ActualizarClienteAsync(
                999,
                dto);


        // Assert

        resultado.Should()
            .BeNull();


        _repositoryMock.Verify(
            x =>
                x.ActualizarClienteAsync(
                    999,
                    dto),
            Times.Once);
    }



    [Fact]
    public async Task EliminarClienteAsync_ClienteExiste_DebeEliminarCliente()
    {
        // Arrange

        _repositoryMock
            .Setup(x => x.EliminarClienteAsync(1))
            .ReturnsAsync(true);



        // Act

        var resultado =
            await _service.EliminarClienteAsync(1);



        // Assert

        resultado.Should()
            .BeTrue();


        _repositoryMock.Verify(
            x => x.EliminarClienteAsync(1),
            Times.Once);
    }

}
