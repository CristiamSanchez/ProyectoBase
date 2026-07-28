using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Proyecto.Application.DTOs.Clientes;
using Proyecto.IntegrationTests.Infrastructure;

namespace Proyecto.IntegrationTests.Api;

public class ClientesControllerTests
    : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public ClientesControllerTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ObtenerClientes_SinToken_DebeRetornarUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                "/api/v1/Clientes");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ObtenerClientes_ConUsuarioAutenticado_DebeRetornarOk()
    {
        // Arrange
        var client =
            await AuthTestHelper
                .CreateAuthenticatedClientAsync(
                    _factory,
                    "cristiam",
                    "123456");

        // Act
        var response =
            await client.GetAsync(
                "/api/v1/Clientes");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ObtenerClientePorId_ClienteInexistente_DebeRetornarNotFound()
    {
        // Arrange
        var client =
            await AuthTestHelper
                .CreateAuthenticatedClientAsync(
                    _factory,
                    "cristiam",
                    "123456");

        // Act
        var response =
            await client.GetAsync(
                "/api/v1/Clientes/999999");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CrearCliente_ConUsuarioAutenticado_DebeRetornarCreated()
    {
        // Arrange
        var client =
            await AuthTestHelper
                .CreateAuthenticatedClientAsync(
                    _factory,
                    "cristiam",
                    "123456");

        var request = new ClienteCreateDto
        {
            Nombre = "Cliente Integration",
            Apellido = "Test",
            Email = $"integration_{Guid.NewGuid()}@test.com"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/v1/Clientes",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ActualizarCliente_ConUsuarioNormal_DebeRetornarForbidden()
    {
        // Arrange
        var client =
            await AuthTestHelper
                .CreateAuthenticatedClientAsync(
                    _factory,
                    "cristiam",
                    "123456");

        var request = new ClienteUpdateDto
        {
            Nombre = "Nombre Actualizado",
            Apellido = "Apellido Actualizado",
            Email = $"update_{Guid.NewGuid()}@test.com"
        };

        // Act
        var response =
            await client.PutAsJsonAsync(
                "/api/v1/Clientes/1",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ActualizarCliente_ConAdmin_ClienteInexistente_DebeRetornarNotFound()
    {
        // Arrange
        var client =
            await AuthTestHelper
                .CreateAuthenticatedClientAsync(
                    _factory,
                    "admin",
                    "123456");

        var request = new ClienteUpdateDto
        {
            Nombre = "Nombre Actualizado",
            Apellido = "Apellido Actualizado",
            Email = $"update_{Guid.NewGuid()}@test.com"
        };

        // Act
        var response =
            await client.PutAsJsonAsync(
                "/api/v1/Clientes/999999",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task EliminarCliente_ConUsuarioNormal_DebeRetornarForbidden()
    {
        // Arrange
        var client =
            await AuthTestHelper
                .CreateAuthenticatedClientAsync(
                    _factory,
                    "cristiam",
                    "123456");

        // Act
        var response =
            await client.DeleteAsync(
                "/api/v1/Clientes/1");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task EliminarCliente_ConAdmin_ClienteInexistente_DebeRetornarNotFound()
    {
        // Arrange
        var client =
            await AuthTestHelper
                .CreateAuthenticatedClientAsync(
                    _factory,
                    "admin",
                    "123456");

        // Act
        var response =
            await client.DeleteAsync(
                "/api/v1/Clientes/999999");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}