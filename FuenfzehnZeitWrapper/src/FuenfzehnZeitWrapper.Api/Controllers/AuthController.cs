using FuenfzehnZeitWrapper.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IFuenfzehnZeitService _fuenfzehnZeitService;

    public AuthController(IFuenfzehnZeitService fuenfzehnZeitService)
    {
        _fuenfzehnZeitService = fuenfzehnZeitService;
    }

    [HttpGet("log-in")]
    public async Task<IActionResult> LogIn()
    {
        await _fuenfzehnZeitService.LogInAsync();
        return Ok();
    }
}
