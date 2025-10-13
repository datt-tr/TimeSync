using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace FunfzehnZeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TimeController : ControllerBase
{
  private readonly ILogger _logger;
  private readonly IWebTerminalService _webTerminalService;

  public TimeController(IWebTerminalService webTerminalService, ILogger<TimeController> logger)
  {
    _logger = logger;
    _webTerminalService = webTerminalService;
  }


  [HttpPost("office/start")]
  public async Task<IActionResult> StartOffice()
  {
    await _webTerminalService.StartOfficeAsync();
    return Ok("Office started");
  }

  [HttpPost("office/end")]
  public async Task<IActionResult> EndOffice()
  {
    await _webTerminalService.EndOfficeAsync();
    return Ok("Office ended");
  }

  [HttpPost("break/start")]
  public async Task<IActionResult> StartBreak()
  {
    await _webTerminalService.StartBreakAsync();
    return Ok("Break started");
  }

  [HttpPost("break/end")]
  public async Task<IActionResult> EndBreak()
  {
    await _webTerminalService.EndBreakAsync();
    return Ok("Break ended");
  }

  [HttpPost("home-office/start")]
  public async Task<IActionResult> StartHomeOffice()
  {
    await _webTerminalService.StartHomeOfficeAsync();
    return Ok("Home Office started");
  }

  [HttpPost("home-office/end")]
  public async Task<IActionResult> EndHomeOffice()
  {
    await _webTerminalService.EndHomeOfficeAsync();
    return Ok("Home Office ended");
  }

  [HttpGet("status")]
  public async Task<IActionResult> GetStatus()
  {
    await _webTerminalService.GetStatusAsync();
    return Ok("Status received");
  }

  [HttpGet("working-hours")]
  public async Task<IActionResult> GetWorkingHours()
  {
    await _webTerminalService.GetWorkingHoursAsync();
    return Ok("Working hours received");
  }

}