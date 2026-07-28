namespace Proyecto.Application.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expira { get; set; }

    public string Rol { get; set; } = string.Empty;
}