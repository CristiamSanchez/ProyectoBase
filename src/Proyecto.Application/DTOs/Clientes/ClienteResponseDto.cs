namespace Proyecto.Application.DTOs.Clientes;

public class ClienteResponseDto
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; }
}
