using Proyecto.Application.DTOs.Auth;
using Proyecto.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Proyecto.Application.Services;
public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IAuthRepository _authRepository;
    private readonly PasswordService _passwordService;

    public AuthService(
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IAuthRepository authRepository,
        PasswordService passwordService)
    {
        _configuration = configuration;
        _logger = logger;
        _authRepository = authRepository;
        _passwordService = passwordService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await _authRepository
            .ObtenerPorUsernameAsync(dto.Username);

        if (usuario is null)
        {
            _logger.LogWarning(
                "Credenciales inválidas para usuario {Usuario}",
                dto.Username);

            return null;
        }

        var passwordValida = _passwordService.Verify(
            dto.Password,
            usuario.PasswordHash);

        if (!passwordValida)
        {
            _logger.LogWarning(
                "Credenciales inválidas para usuario {Usuario}",
                dto.Username);

            return null;
        }

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                usuario.Username),

            new Claim(
                ClaimTypes.Role,
                usuario.Rol)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        _logger.LogInformation(
            "Login exitoso para usuario {Usuario} con rol {Rol}",
            usuario.Username,
            usuario.Rol);

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler()
                .WriteToken(token),

            Expira = expiration,

            Rol = usuario.Rol
        };
    }
}