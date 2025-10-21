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
  public async Task<Results<Ok, BadRequest<string>>> StartOffice()
  {
    try
    {
      await _fuenfzehnZeitService.StartOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok();
  }

  [HttpPost("office/end")]
  public async Task<Results<Ok, BadRequest<string>>> EndOffice()
  {
    try
    {
      await _fuenfzehnZeitService.EndOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok();
  }

  [HttpPost("break/start")]
  public async Task<Results<Ok, BadRequest<string>>> StartBreak()
  {
    try
    {
      await _fuenfzehnZeitService.StartBreakAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok();
  }

  [HttpPost("break/end")]
  public async Task<Results<Ok, BadRequest<string>>> EndBreak()
  {
    try
    {
      await _fuenfzehnZeitService.EndBreakAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok();
  }

  [HttpPost("home-office/start")]
  public async Task<Results<Ok, BadRequest<string>>> StartHomeOffice()
  {
    try
    {
      await _fuenfzehnZeitService.StartHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok();
  }

  [HttpPost("home-office/end")]
  public async Task<Results<Ok, BadRequest<string>>> EndHomeOffice()
  {
    try
    {
      await _fuenfzehnZeitService.EndHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok();
  }

  [HttpGet("status")]
  public async Task<Results<Ok<string>, BadRequest<string>>> GetStatus()
  {
    try
    {
      var status = await _fuenfzehnZeitService.GetStatusAsync();
      return TypedResults.Ok(status);
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }
    catch (InvalidOperationException)
    {
      return TypedResults.BadRequest($"{nameof(GetStatus)} is not possible");
    }
  }

  [HttpGet("working-hours")]
  public async Task<Results<Ok<string>, BadRequest<string>>> GetWorkingHours()
  {
    try
    {
      var workingHours = await _fuenfzehnZeitService.GetWorkingHoursAsync();
      return TypedResults.Ok(workingHours);
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }
    catch (InvalidOperationException)
    {
      return TypedResults.BadRequest($"{nameof(GetWorkingHours)} is not possible");
    }
  }
}