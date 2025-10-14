using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FunfzehnZeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TimeController : ControllerBase
{
  private readonly ILogger _logger;
  private readonly IWebTerminalService _webTerminalService;
  private readonly string _funfzehnZeitError = "Failed FunfzehnZeit Server Request";

  public TimeController(IWebTerminalService webTerminalService, ILogger<TimeController> logger)
  {
    _logger = logger;
    _webTerminalService = webTerminalService;
  }


  [HttpPost("office/start")]
  public async Task<Results<Ok<string>, BadRequest<string>>> StartOffice()
  {
    try
    {
      await _webTerminalService.StartOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Office started");
  }

  [HttpPost("office/end")]
  public async Task<Results<Ok<string>, BadRequest<string>>> EndOffice()
  {
    try
    {
      await _webTerminalService.EndOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Office ended");
  }

  [HttpPost("break/start")]
  public async Task<Results<Ok<string>, BadRequest<string>>> StartBreak()
  {
    try
    {
      await _webTerminalService.StartBreakAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Break started");
  }

  [HttpPost("break/end")]
  public async Task<Results<Ok<string>, BadRequest<string>>> EndBreak()
  {
    try
    {
      await _webTerminalService.EndBreakAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Break ended");
  }

  [HttpPost("home-office/start")]
  public async Task<Results<Ok<string>, BadRequest<string>>> StartHomeOffice()
  {
    try
    {
      await _webTerminalService.StartHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Home Office started");
  }

  [HttpPost("home-office/end")]
  public async Task<Results<Ok<string>, BadRequest<string>>> EndHomeOffice()
  {
    try
    {
      await _webTerminalService.EndHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Home Office ended");
  }

  [HttpGet("status")]
  public async Task<Results<Ok<string>, BadRequest<string>>> GetStatus()
  {
    try
    {
      await _webTerminalService.GetStatusAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Status received");
  }

  [HttpGet("working-hours")]
  public async Task<Results<Ok<string>, BadRequest<string>>> GetWorkingHours()
  {
    try
    {
      await _webTerminalService.GetWorkingHoursAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_funfzehnZeitError);
    }

    return TypedResults.Ok("Working hours received");
  }
}