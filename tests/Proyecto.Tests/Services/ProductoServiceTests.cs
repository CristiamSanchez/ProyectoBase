using FluentAssertions;
using Moq;
using Proyecto.Application.DTOs.Productos;
using Proyecto.Application.Interfaces;
using Proyecto.Application.Services;
using Proyecto.Domain.Entities;
using Xunit;

namespace Proyecto.Tests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _repositoryMock;
    private readonly Mock<AutoMapper.IMapper> _mapperMock;

    private readonly ProductoService _service;


    public ProductoServiceTests()
    {
        _repositoryMock = new Mock<IProductoRepository>();

        _mapperMock = new Mock<AutoMapper.IMapper>();

        _service = new ProductoService(
            _repositoryMock.Object,
            _mapperMock.Object);
    }



    [Fact]
    public async Task GetAllAsync_DebeRetornarProductos()
    {
        // Arrange

        var productos = new List<Producto>
        {
            new Producto
            {
                Id = 1,
                Nombre = "Laptop",
                Precio = 1500
            }
        };


        var productosDto = new List<ProductoResponseDto>
        {
            new ProductoResponseDto
            {
                Id = 1,
                Nombre = "Laptop",
                Precio = 1500
            }
        };


        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(productos);


        _mapperMock
            .Setup(x => x.Map<IEnumerable<ProductoResponseDto>>(productos))
            .Returns(productosDto);



        // Act

        var resultado =
            await _service.GetAllAsync();



        // Assert

        resultado.Should()
            .NotBeNull();


        resultado.Should()
            .HaveCount(1);


        resultado.First().Nombre
            .Should()
            .Be("Laptop");
    }




    [Fact]
    public async Task GetByIdAsync_ProductoExiste_DebeRetornarProducto()
    {
        // Arrange

        var producto = new Producto
        {
            Id = 1,
            Nombre = "Mouse",
            Precio = 25
        };


        var productoDto = new ProductoResponseDto
        {
            Id = 1,
            Nombre = "Mouse",
            Precio = 25
        };


        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(producto);


        _mapperMock
            .Setup(x => x.Map<ProductoResponseDto>(producto))
            .Returns(productoDto);



        // Act

        var resultado =
            await _service.GetByIdAsync(1);



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

        _repositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Producto?)null);



        // Act

        var resultado =
            await _service.GetByIdAsync(999);



        // Assert

        resultado.Should()
            .BeNull();
    }




    [Fact]
    public async Task CreateAsync_DatosValidos_DebeCrearProducto()
    {
        // Arrange

        var dto = new ProductoCreateDto
        {
            Nombre = "Teclado",
            Precio = 40
        };


        var producto = new Producto
        {
            Nombre = "Teclado",
            Precio = 40
        };


        var productoCreado = new Producto
        {
            Id = 1,
            Nombre = "Teclado",
            Precio = 40
        };


        _mapperMock
            .Setup(x => x.Map<Producto>(dto))
            .Returns(producto);


        _repositoryMock
            .Setup(x => x.AddAsync(producto))
            .ReturnsAsync(productoCreado);


        _mapperMock
            .Setup(x => x.Map<ProductoResponseDto>(productoCreado))
            .Returns(new ProductoResponseDto
            {
                Id = 1,
                Nombre = "Teclado",
                Precio = 40
            });



        // Act

        var resultado =
            await _service.CreateAsync(dto);



        // Assert

        resultado.Should()
            .NotBeNull();


        resultado.Nombre
            .Should()
            .Be("Teclado");


        _repositoryMock.Verify(
            x => x.AddAsync(producto),
            Times.Once);
    }




    [Fact]
    public async Task UpdateAsync_ProductoExiste_DebeActualizarProducto()
    {
        // Arrange

        var producto = new Producto
        {
            Id = 1,
            Nombre = "Teclado",
            Precio = 40
        };


        var dto = new ProductoCreateDto
        {
            Nombre = "Teclado Gamer",
            Precio = 80
        };


        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(producto);



        // Act

        await _service.UpdateAsync(1, dto);



        // Assert


        _repositoryMock.Verify(
            x => x.UpdateAsync(producto),
            Times.Once);
    }




    [Fact]
    public async Task DeleteAsync_DebeEliminarProducto()
    {
        // Arrange


        _repositoryMock
            .Setup(x => x.DeleteAsync(1))
            .Returns(Task.CompletedTask);



        // Act

        await _service.DeleteAsync(1);



        // Assert


        _repositoryMock.Verify(
            x => x.DeleteAsync(1),
            Times.Once);
    }
}