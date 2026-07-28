namespace Proyecto.Application.DTOs.Clientes;

public class ClienteUpdateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; }
}