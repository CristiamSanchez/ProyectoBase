using System.Net;
using FluentAssertions;
using Proyecto.IntegrationTests.Infrastructure;
using System.Net.Http.Json;

namespace Proyecto.IntegrationTests.Api;

public class ProductosControllerTests
    : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _client;

    public ProductosControllerTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task ObtenerProductos_SinToken_DebeRetornarUnauthorized()
    {
        // Act
        var response =
            await _client.GetAsync(
                "/api/v1/Productos");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task ObtenerProductos_ConUsuarioAutenticado_DebeRetornarOk()
    {
        // Arrange
        var client =
            await AuthTestHelper.CreateAuthenticatedClientAsync(
                _factory,
                "cristiam",
                "123456");

        // Act
        var response =
            await client.GetAsync(
                "/api/v1/Productos");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task ObtenerProductos_ConAdmin_DebeRetornarOk()
    {
        // Arrange
        var client =
            await AuthTestHelper.CreateAuthenticatedClientAsync(
                _factory,
                "admin",
                "123456");

        // Act
        var response =
            await client.GetAsync(
                "/api/v1/Productos");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task ActualizarProducto_ConUsuarioNormal_DebeRetornarForbidden()
    {
        // Arrange
        var client =
            await AuthTestHelper.CreateAuthenticatedClientAsync(
                _factory,
                "cristiam",
                "123456");

        // Act
        var response =
            await client.PutAsJsonAsync(
                "/api/v1/Productos/999999",
                new
                {
                    Nombre = "Producto actualizado",
                    Precio = 100
                });

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }


    [Fact]
    public async Task EliminarProducto_ConUsuarioNormal_DebeRetornarForbidden()
    {
        // Arrange
        var client =
            await AuthTestHelper.CreateAuthenticatedClientAsync(
                _factory,
                "cristiam",
                "123456");

        // Act
        var response =
            await client.DeleteAsync(
                "/api/v1/Productos/999999");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }
}