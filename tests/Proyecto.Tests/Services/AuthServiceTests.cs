using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Proyecto.Application.DTOs.Auth;
using Proyecto.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;
using Proyecto.Application.Interfaces; 
using Proyecto.Domain.Entities;


namespace Proyecto.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<ILogger<AuthService>> _logger;
    private readonly Mock<IAuthRepository> _authRepository;
    private readonly AuthService _service;
    private readonly PasswordService _passwordService;

    public AuthServiceTests()
    {
        _configuration = new Mock<IConfiguration>();
        _logger = new Mock<ILogger<AuthService>>();
        _authRepository = new Mock<IAuthRepository>();

        _passwordService = new PasswordService();

        _configuration
            .Setup(x => x["Jwt:Key"])
            .Returns("clave-super-secreta-de-prueba-123456789");

        _configuration
            .Setup(x => x["Jwt:Issuer"])
            .Returns("ProyectoApi");

        _configuration
            .Setup(x => x["Jwt:Audience"])
            .Returns("ProyectoClientes");

        _service = new AuthService(
            _configuration.Object,
            _logger.Object,
            _authRepository.Object,
            _passwordService);
    }

    [Fact]
    public async Task LoginAsync_UsuarioAdminValido_DebeGenerarToken()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = "admin",
            Password = "123456"
        };

        var usuario = new Usuario
        {
            Id = 1,
            Username = "admin",
            PasswordHash = _passwordService.Hash("123456"),
            Rol = "Admin",
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _authRepository
            .Setup(x => x.ObtenerPorUsernameAsync("admin"))
            .ReturnsAsync(usuario);

        // Act

        var resultado = await _service.LoginAsync(request);

        // Assert

        resultado.Should().NotBeNull();
        resultado!.Token.Should().NotBeNullOrEmpty();
        resultado.Rol.Should().Be("Admin");
    }


   [Fact]
    public async Task LoginAsync_UsuarioCristiamValido_DebeGenerarToken()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = "cristiam",
            Password = "123456"
        };

        var usuario = new Usuario
        {
            Id = 2,
            Username = "cristiam",
            PasswordHash = _passwordService.Hash("123456"),
            Rol = "User",
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _authRepository
            .Setup(x => x.ObtenerPorUsernameAsync("cristiam"))
            .ReturnsAsync(usuario);

        // Act

        var resultado = await _service.LoginAsync(request);

        // Assert

        resultado.Should().NotBeNull();
        resultado!.Token.Should().NotBeNullOrEmpty();
        resultado.Rol.Should().Be("User");
    }




    [Fact]
    public async Task LoginAsync_CredencialesIncorrectas_DebeRetornarNull()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = "admin",
            Password = "incorrecta"
        };



        // Act

        var resultado =
            await _service.LoginAsync(request);



        // Assert

        resultado.Should()
            .BeNull();
    }




    [Fact]
    public async Task LoginAsync_UsuarioNull_DebeRetornarNull()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = string.Empty,
            Password = "123456"
        };



        // Act

        var resultado =
            await _service.LoginAsync(request);



        // Assert

        resultado.Should()
            .BeNull();
    }




    [Fact]
    public async Task LoginAsync_TokenGenerado_DebeContenerClaims()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = "admin",
            Password = "123456"
        };

        var usuario = new Usuario
        {
            Id = 1,
            Username = "admin",
            PasswordHash = _passwordService.Hash("123456"),
            Rol = "Admin",
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _authRepository
            .Setup(x => x.ObtenerPorUsernameAsync("admin"))
            .ReturnsAsync(usuario);

        // Act

        var resultado = await _service.LoginAsync(request);

        // Assert

        resultado.Should().NotBeNull();

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(resultado!.Token);

        token.Claims.Should()
            .Contain(c =>
                c.Type == ClaimTypes.Name &&
                c.Value == "admin");

        token.Claims.Should()
            .Contain(c =>
                c.Type == ClaimTypes.Role &&
                c.Value == "Admin");
    }

    [Fact]
    public async Task LoginAsync_CredencialesValidas_DebeRetornarToken()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = "admin",
            Password = "123456"
        };

        var usuario = new Usuario
        {
            Id = 1,
            Username = "admin",
            PasswordHash = _passwordService.Hash("123456"),
            Rol = "Admin",
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _authRepository
            .Setup(x => x.ObtenerPorUsernameAsync("admin"))
            .ReturnsAsync(usuario);

        // Act

        var resultado = await _service.LoginAsync(request);

        // Assert

        resultado.Should().NotBeNull();
        resultado!.Token.Should().NotBeNullOrEmpty();
        resultado.Rol.Should().Be("Admin");
    }

    [Fact]
    public async Task LoginAsync_UsuarioNoExiste_DebeRetornarNull()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = "usuario-inexistente",
            Password = "123456"
        };

        _authRepository
            .Setup(x => x.ObtenerPorUsernameAsync("usuario-inexistente"))
            .ReturnsAsync((Usuario?)null);

        // Act

        var resultado = await _service.LoginAsync(request);

        // Assert

        resultado.Should().BeNull();
    }


    [Fact]
    public async Task LoginAsync_ContrasenaIncorrecta_DebeRetornarNull()
    {
        // Arrange

        var request = new LoginRequestDto
        {
            Username = "admin",
            Password = "password-incorrecto"
        };

        var usuario = new Usuario
        {
            Id = 1,
            Username = "admin",
            PasswordHash = _passwordService.Hash("123456"),
            Rol = "Admin",
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _authRepository
            .Setup(x => x.ObtenerPorUsernameAsync("admin"))
            .ReturnsAsync(usuario);

        // Act

        var resultado = await _service.LoginAsync(request);

        // Assert

        resultado.Should().BeNull();
    }

    [Fact]
    public void GenerarHashPassword()
    {
        var hash = new PasswordService().Hash("123456");

        Console.WriteLine($"HASH_GENERADO: {hash}");

        hash.Should().NotBeNullOrEmpty();
    }

}