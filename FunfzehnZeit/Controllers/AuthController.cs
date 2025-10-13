using System.Threading.Tasks;
using FunfzehnZeit.Services;
using Microsoft.AspNetCore.Mvc;
using FunfzehnZeit.Interfaces;

namespace Funfzehnzeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IWebTerminalService _webTerminalService;
  private readonly ILogger _logger;

  public AuthController(IWebTerminalService webTerminalService, ILogger<AuthController> logger)
  {
    _logger = logger;
    _webTerminalService = webTerminalService;
  }

  [HttpPost]
  [Route("login")]
  public async Task<IActionResult> Login()
  {
    await _webTerminalService.GetLoginPageAsync();

    await _webTerminalService.LoginAsync();
    _logger.LogInformation($"{nameof(Login)} Request executed");
    return Ok("Logged in");
  }
}