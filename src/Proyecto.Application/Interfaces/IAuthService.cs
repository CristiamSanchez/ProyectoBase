using Proyecto.Application.DTOs.Auth;

namespace Proyecto.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}