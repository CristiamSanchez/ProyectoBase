using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Proyecto.Application.DTOs.Auth;
using Proyecto.Application.Services;
using Proyecto.IntegrationTests.Infrastructure;

namespace Proyecto.IntegrationTests.Api;

public class AuthControllerTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;


    public AuthControllerTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_CredencialesInvalidas_DebeRetornarUnauthorized()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Username = "usuario@incorrecto.com",
            Password = "password-invalido"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_CredencialesValidas_DebeRetornarOkConToken()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Username = "admin",
            Password = "123456"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result = await response.Content
            .ReadFromJsonAsync<LoginResponseDto>();

        result.Should().NotBeNull();

        result!.Token
            .Should()
            .NotBeNullOrWhiteSpace();

        result.Rol
            .Should()
            .Be("Admin");

        result.Expira
            .Should()
            .BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_CredencialesValidas_DebeRetornarTokenConFormatoJwt()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Username = "admin",
            Password = "123456"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result = await response.Content
            .ReadFromJsonAsync<LoginResponseDto>();

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrWhiteSpace();

        // Un JWT tiene tres segmentos separados por puntos.
        var partesToken = result.Token.Split('.');

        partesToken.Should().HaveCount(3);
    }

    [Fact]
    public void GenerarHashPassword()
    {
        var hash = new PasswordService().Hash("123456");

        Console.WriteLine($"HASH_GENERADO: {hash}");

        hash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void VerificarPassword_Correcta_DebeRetornarTrue()
    {
        // Arrange
        var passwordService = new PasswordService();

        var hash = passwordService.Hash("123456");

        // Act
        var resultado = passwordService.Verify("123456", hash);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void VerificarPassword_Incorrecta_DebeRetornarFalse()
    {
        // Arrange
        var passwordService = new PasswordService();

        var hash = passwordService.Hash("123456");

        // Act
        var resultado = passwordService.Verify("password-incorrecto", hash);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task Me_SinToken_DebeRetornarUnauthorized()
    {
        var response = await _client.GetAsync(
            "/api/Auth/me");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

}
