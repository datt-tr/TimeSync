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

  public AuthController(IWebTerminalService webTerminalService, IUserSessionService userSessionService)
  {
    _webTerminalService = webTerminalService;
    _userSessionService = userSessionService;
  }

  [HttpGet]
  [Route("login-page")]
  public async Task<IActionResult> GetLoginPage()
  {
    await _webTerminalService.GetLoginPageAsync();
    return Ok("Login page fetched");
  }

  [HttpPost]
  [Route("login")]
  public async Task<IActionResult> Login()
  {
    await _webTerminalService.LoginAsync();
    return Ok("Logged in");
  }
}