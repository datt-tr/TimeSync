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


  [HttpGet]
  [Route("office/start")]
  public async Task<IActionResult> StartOffice()
  {
    await _webTerminalService.StartOfficeAsync();
    return Ok("Office started");
  }

  [HttpGet]
  [Route("office/end")]
  public async Task<IActionResult> EndOffice()
  {
    await _webTerminalService.EndOfficeAsync();
    return Ok("Office ended");
  }

  [HttpGet]
  [Route("break/start")]
  public async Task<IActionResult> StartBreak()
  {
    await _webTerminalService.StartBreakAsync();
    return Ok("Break started");
  }

  [HttpGet]
  [Route("break/end")]
  public async Task<IActionResult> EndBreak()
  {
    await _webTerminalService.EndBreakAsync();
    return Ok("Break ended");
  }

  [HttpGet]
  [Route("home-office/start")]
  public async Task<IActionResult> StartHomeOffice()
  {
    await _webTerminalService.StartHomeOfficeAsync();
    return Ok("Home Office started");
  }

  [HttpGet]
  [Route("home-office/end")]
  public async Task<IActionResult> EndHomeOffice()
  {
    await _webTerminalService.EndHomeOfficeAsync();
    return Ok("Home Office ended");
  }

  [HttpGet]
  [Route("status")]
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