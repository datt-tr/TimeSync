using System.Runtime.CompilerServices;
using FuenfzehnZeitWrapper.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TimeController : ControllerBase
{
  private readonly ILogger _logger;
  private readonly IFuenfzehnZeitService _fuenfzehnZeitService;
  private readonly string _fuenfzehnZeitError = "Failed FuenfzehnZeit Server Request";

  public TimeController(IFuenfzehnZeitService fuenfzehnZeitService, ILogger<TimeController> logger)
  {
    _logger = logger;
    _fuenfzehnZeitService = fuenfzehnZeitService;
  }


  [HttpPost("office/start")]
  public async Task<Results<Ok<string>, BadRequest<string>>> StartOffice()
  {
    try
    {
      await _fuenfzehnZeitService.StartOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Office started");
  }

  [HttpPost("office/end")]
  public async Task<Results<Ok<string>, BadRequest<string>>> EndOffice()
  {
    try
    {
      await _fuenfzehnZeitService.EndOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Office ended");
  }

  [HttpPost("break/start")]
  public async Task<Results<Ok<string>, BadRequest<string>>> StartBreak()
  {
    try
    {
      await _fuenfzehnZeitService.StartBreakAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Break started");
  }

  [HttpPost("break/end")]
  public async Task<Results<Ok<string>, BadRequest<string>>> EndBreak()
  {
    try
    {
      await _fuenfzehnZeitService.EndBreakAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Break ended");
  }

  [HttpPost("home-office/start")]
  public async Task<Results<Ok<string>, BadRequest<string>>> StartHomeOffice()
  {
    try
    {
      await _fuenfzehnZeitService.StartHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Home Office started");
  }

  [HttpPost("home-office/end")]
  public async Task<Results<Ok<string>, BadRequest<string>>> EndHomeOffice()
  {
    try
    {
      await _fuenfzehnZeitService.EndHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Home Office ended");
  }

  [HttpGet("status")]
  public async Task<Results<Ok<string>, BadRequest<string>>> GetStatus()
  {
    try
    {
      await _fuenfzehnZeitService.GetStatusAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Status received");
  }

  [HttpGet("working-hours")]
  public async Task<Results<Ok<string>, BadRequest<string>>> GetWorkingHours()
  {
    try
    {
      await _fuenfzehnZeitService.GetWorkingHoursAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok("Working hours received");
  }
}