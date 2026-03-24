using Microsoft.AspNetCore.Mvc;

namespace Oranum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            service = "Oranum API",
            utcNow = DateTime.UtcNow
        });
    }
}
