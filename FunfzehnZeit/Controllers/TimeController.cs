using Microsoft.AspNetCore.Mvc;

namespace FunfzehnZeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TimeController : ControllerBase
{
  private readonly IWebTerminalService _webTerminalService;

  public TimeController(IWebTerminalService webTerminalService)
  {
    _webTerminalService = webTerminalService;
  }

  [HttpGet]
  [Route("status")]
  public async Task<IActionResult> GetStatus()
  {
    await _webTerminalService.GetStatusAsync();
    return Ok("Status received");
  }
}