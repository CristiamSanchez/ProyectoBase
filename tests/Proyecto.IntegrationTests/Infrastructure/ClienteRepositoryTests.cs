using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;
using Proyecto.Infrastructure.Repositories;
using Proyecto.IntegrationTests.Infrastructure;

namespace Proyecto.IntegrationTests;

public class ClienteRepositoryTests : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _fixture;

    public ClienteRepositoryTests(
        PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async Task ObtenerClientesAsync_DebeRetornarClientes()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        context.Clientes.RemoveRange(
            await context.Clientes.ToListAsync());

        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            Nombre = "Cristiam",
            Apellido = "Sanchez",
            Email = "cristiam@test.com",
            Activo = true
        };

        context.Clientes.Add(cliente);

        await context.SaveChangesAsync();

        var repository =
            new ClienteRepository(context);


        // Act

        var resultado =
            await repository.ObtenerClientesAsync();


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado.Should()
            .ContainSingle();

        resultado.First()
            .Nombre
            .Should()
            .Be("Cristiam");
    }


    [Fact]
    public async Task ObtenerClientePorIdAsync_DebeRetornarCliente()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var cliente = new Cliente
        {
            Nombre = "Juan",
            Apellido = "Perez",
            Email = "juan@test.com",
            Activo = true
        };

        context.Clientes.Add(cliente);

        await context.SaveChangesAsync();

        var repository =
            new ClienteRepository(context);


        // Act

        var resultado =
            await repository.ObtenerClientePorIdAsync(
                cliente.Id);


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado!.Nombre
            .Should()
            .Be("Juan");
    }


    [Fact]
    public async Task ObtenerClientePorIdAsync_ClienteNoExiste_DebeRetornarNull()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository =
            new ClienteRepository(context);


        // Act

        var resultado =
            await repository.ObtenerClientePorIdAsync(999999);


        // Assert

        resultado.Should()
            .BeNull();
    }


    [Fact]
    public async Task CrearClienteAsync_DebeGuardarCliente()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository =
            new ClienteRepository(context);

        var cliente = new Cliente
        {
            Nombre = "Pedro",
            Apellido = "Lopez",
            Email = "pedro@test.com",
            Activo = true
        };


        // Act

        var resultado =
            await repository.CrearClienteAsync(cliente);


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado.Id
            .Should()
            .BeGreaterThan(0);

        resultado.Nombre
            .Should()
            .Be("Pedro");
    }


    [Fact]
    public async Task ActualizarClienteAsync_DebeActualizarCliente()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var cliente = new Cliente
        {
            Nombre = "Carlos",
            Apellido = "Gomez",
            Email = "carlos@test.com",
            Activo = true
        };

        context.Clientes.Add(cliente);

        await context.SaveChangesAsync();

        var repository =
            new ClienteRepository(context);

        var dto = new Proyecto.Application.DTOs.Clientes.ClienteUpdateDto
        {
            Nombre = "Carlos Actualizado",
            Apellido = "Gomez",
            Email = "carlos.updated@test.com",
            Activo = false
        };


        // Act

        var resultado =
            await repository.ActualizarClienteAsync(
                cliente.Id,
                dto);


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado!.Nombre
            .Should()
            .Be("Carlos Actualizado");

        resultado.Email
            .Should()
            .Be("carlos.updated@test.com");

        resultado.Activo
            .Should()
            .BeFalse();
    }


    [Fact]
    public async Task ActualizarClienteAsync_ClienteNoExiste_DebeRetornarNull()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository =
            new ClienteRepository(context);

        var dto = new Proyecto.Application.DTOs.Clientes.ClienteUpdateDto
        {
            Nombre = "No Existe",
            Apellido = "Cliente",
            Email = "noexiste@test.com",
            Activo = true
        };


        // Act

        var resultado =
            await repository.ActualizarClienteAsync(
                999999,
                dto);


        // Assert

        resultado.Should()
            .BeNull();
    }


    [Fact]
    public async Task EliminarClienteAsync_ClienteExiste_DebeRetornarTrue()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var cliente = new Cliente
        {
            Nombre = "Eliminar",
            Apellido = "Cliente",
            Email = "eliminar@test.com",
            Activo = true
        };

        context.Clientes.Add(cliente);

        await context.SaveChangesAsync();

        var repository =
            new ClienteRepository(context);


        // Act

        var resultado =
            await repository.EliminarClienteAsync(
                cliente.Id);


        // Assert

        resultado.Should()
            .BeTrue();

        var clienteEliminado =
            await context.Clientes
                .FirstOrDefaultAsync(
                    x => x.Id == cliente.Id);

        clienteEliminado.Should()
            .BeNull();
    }


    [Fact]
    public async Task EliminarClienteAsync_ClienteNoExiste_DebeRetornarFalse()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository =
            new ClienteRepository(context);


        // Act

        var resultado =
            await repository.EliminarClienteAsync(
                999999);


        // Assert

        resultado.Should()
            .BeFalse();
    }
}