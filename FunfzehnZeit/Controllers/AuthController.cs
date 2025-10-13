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
  private readonly IUserSessionService _userSessionService;
  private readonly ILogger _logger;

  public AuthController(IWebTerminalService webTerminalService, IUserSessionService userSessionService, ILogger<AuthController> logger)
  {
    _logger = logger;
    _webTerminalService = webTerminalService;
    _userSessionService = userSessionService;
  }

  [HttpGet]
  [Route("login-page")]
  public async Task<IActionResult> GetLoginPage()
  {
    await _webTerminalService.GetLoginPageAsync();
    _logger.LogInformation($"{nameof(GetLoginPage)} Request executed");
    return Ok("Login page fetched");
  }

  [HttpPost]
  [Route("login")]
  public async Task<IActionResult> Login()
  {
    await _webTerminalService.LoginAsync();
    _logger.LogInformation($"{nameof(Login)} Request executed");
    return Ok("Logged in");
  }
}