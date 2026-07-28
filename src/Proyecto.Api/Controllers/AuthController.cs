using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs.Auth;
using Proyecto.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; 


namespace Proyecto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;


    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if(result == null)
        {
            return Unauthorized();
        }


        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        var rol = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrWhiteSpace(userId) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(rol))
        {
            return Unauthorized();
        }

        var response = new CurrentUserResponseDto
        {
            Id = int.Parse(userId),
            Username = username,
            Rol = rol
        };

        return Ok(response);
    }

}