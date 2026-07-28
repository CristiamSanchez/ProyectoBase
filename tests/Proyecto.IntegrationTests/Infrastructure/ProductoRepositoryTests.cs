using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Proyecto.Domain.Entities;
using Proyecto.Infrastructure.Repositories;
using Proyecto.IntegrationTests.Infrastructure;

namespace Proyecto.IntegrationTests;

public class ProductoRepositoryTests : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _fixture;

    public ProductoRepositoryTests(
        PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async Task GetAllAsync_DebeRetornarProductos()
    {
        // Arrange

        using var context = _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository = new ProductoRepository(context);

        var producto = new Producto
        {
            Nombre = "Teclado",
            Precio = 40
        };

        await repository.AddAsync(producto);

        // Act

        var resultado =
            await repository.GetAllAsync();

        // Assert

        resultado.Should()
            .Contain(x =>
                x.Nombre == "Teclado" &&
                x.Precio == 40);
    }


    [Fact]
    public async Task GetByIdAsync_DebeRetornarProducto()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var producto = new Producto
        {
            Nombre = "Mouse",
            Precio = 25
        };

        context.Productos.Add(producto);

        await context.SaveChangesAsync();

        var repository =
            new ProductoRepository(context);


        // Act

        var resultado =
            await repository.GetByIdAsync(
                producto.Id);


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado!.Nombre
            .Should()
            .Be("Mouse");
    }


    [Fact]
    public async Task GetByIdAsync_ProductoNoExiste_DebeRetornarNull()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository =
            new ProductoRepository(context);


        // Act

        var resultado =
            await repository.GetByIdAsync(
                999999);


        // Assert

        resultado.Should()
            .BeNull();
    }


    [Fact]
    public async Task AddAsync_DebeGuardarProducto()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository =
            new ProductoRepository(context);

        var producto = new Producto
        {
            Nombre = "Teclado",
            Precio = 40
        };


        // Act

        var resultado =
            await repository.AddAsync(
                producto);


        // Assert

        resultado.Should()
            .NotBeNull();

        resultado.Id
            .Should()
            .BeGreaterThan(0);

        resultado.Nombre
            .Should()
            .Be("Teclado");
    }


    [Fact]
    public async Task UpdateAsync_DebeActualizarProducto()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var producto = new Producto
        {
            Nombre = "Monitor",
            Precio = 300
        };

        context.Productos.Add(producto);

        await context.SaveChangesAsync();

        var repository =
            new ProductoRepository(context);

        producto.Nombre = "Monitor 4K";
        producto.Precio = 500;


        // Act

        await repository.UpdateAsync(
            producto);


        // Assert

        var productoActualizado =
            await context.Productos
                .FirstOrDefaultAsync(
                    x => x.Id == producto.Id);

        productoActualizado.Should()
            .NotBeNull();

        productoActualizado!.Nombre
            .Should()
            .Be("Monitor 4K");

        productoActualizado.Precio
            .Should()
            .Be(500);
    }


    [Fact]
    public async Task DeleteAsync_ProductoExiste_DebeEliminarProducto()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var producto = new Producto
        {
            Nombre = "Impresora",
            Precio = 200
        };

        context.Productos.Add(producto);

        await context.SaveChangesAsync();

        var repository =
            new ProductoRepository(context);


        // Act

        await repository.DeleteAsync(
            producto.Id);


        // Assert

        var productoEliminado =
            await context.Productos
                .FirstOrDefaultAsync(
                    x => x.Id == producto.Id);

        productoEliminado.Should()
            .BeNull();
    }


    [Fact]
    public async Task DeleteAsync_ProductoNoExiste_NoDebeGenerarError()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository =
            new ProductoRepository(context);


        // Act

        var action = async () =>
            await repository.DeleteAsync(
                999999);


        // Assert

        await action.Should()
            .NotThrowAsync();
    }


    [Fact]
    public async Task GetPagedAsync_DebeRetornarProductosPaginados()
    {
        // Arrange
        using var context = _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var repository = new ProductoRepository(context);

        await repository.AddAsync(
            new Producto
            {
                Nombre = "TestPaged-1",
                Precio = 10
            });

        await repository.AddAsync(
            new Producto
            {
                Nombre = "TestPaged-2",
                Precio = 20
            });

        await repository.AddAsync(
            new Producto
            {
                Nombre = "TestPaged-3",
                Precio = 30
            });

        // Act

        var resultado =
            await repository.GetPagedAsync(
                page: 1,
                pageSize: 2,
                search: "TestPaged");

        // Assert

        resultado.Total
            .Should()
            .Be(3);

        resultado.Items
            .Should()
            .HaveCount(2);
    }


    [Fact]
    public async Task GetPagedAsync_ConBusqueda_DebeFiltrarProductos()
    {
        // Arrange

        await using var context =
            _fixture.CreateDbContext();

        await context.Database.EnsureCreatedAsync();

        var productos = new List<Producto>
        {
            new Producto
            {
                Nombre = "Laptop Lenovo",
                Precio = 1000
            },
            new Producto
            {
                Nombre = "Laptop Dell",
                Precio = 1200
            },
            new Producto
            {
                Nombre = "Mouse Logitech",
                Precio = 50
            }
        };

        context.Productos.AddRange(productos);

        await context.SaveChangesAsync();

        var repository =
            new ProductoRepository(context);


        // Act

        var resultado =
            await repository.GetPagedAsync(
                page: 1,
                pageSize: 10,
                search: "Laptop");


        // Assert

        resultado.Total
            .Should()
            .Be(2);

        resultado.Items
            .Should()
            .HaveCount(2);

        resultado.Items
            .Should()
            .OnlyContain(
                x => x.Nombre.Contains("Laptop"));
    }
}