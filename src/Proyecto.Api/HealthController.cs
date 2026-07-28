using Microsoft.AspNetCore.Mvc;
using Proyecto.Infrastructure.Data;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;

    public HealthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("db")]
    public IActionResult CheckDatabase()
    {
        var canConnect = _context.Database.CanConnect();
        return Ok(new { database = "db_Ini", connected = canConnect });
    }
}