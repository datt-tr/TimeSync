using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Presentation.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class AuthController : ControllerBase
{
    [HttpGet("log-in")]
    public async Task<IActionResult> LogIn()
    {
        return Ok();
    }
}
