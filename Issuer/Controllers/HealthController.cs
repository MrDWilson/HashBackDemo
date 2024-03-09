using Microsoft.AspNetCore.Mvc;

namespace Issuer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    public HealthController() { }

    [HttpGet]
    public IActionResult Health()
    {
        return Ok("Healthy");
    }
}
