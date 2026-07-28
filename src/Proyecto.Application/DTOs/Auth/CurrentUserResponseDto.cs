namespace Proyecto.Application.DTOs.Auth;

public class CurrentUserResponseDto
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}