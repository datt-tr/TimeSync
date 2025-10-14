using System.Threading.Tasks;
using FunfzehnZeit.Services;
using Microsoft.AspNetCore.Mvc;
using FunfzehnZeit.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

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

  [HttpPost("login")]
  public async Task<Results<Ok<string>, BadRequest<string>>> Login()
  {
    try
    {
      await _webTerminalService.GetLoginPageAsync();
      await _webTerminalService.LoginAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest("Failed FunfzehnZeit Server Request");
    }

    return TypedResults.Ok("Login executed");
  }
}