using System.Threading.Tasks;
using FunfzehnZeit.Services;
using Microsoft.AspNetCore.Mvc;

namespace Funfzehnzeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
  private readonly WebTerminalService _webTerminalService;

  public AuthController(WebTerminalService webTerminalService)
  {
    _webTerminalService = webTerminalService;
  }

  [HttpGet]
  public async Task GetLoginPage()
  {
    await _webTerminalService.GetLoginPageAsync();
  }

  [HttpPost]
  public async Task Login()
  {
  }
}