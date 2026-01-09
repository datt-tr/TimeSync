using System.Runtime.CompilerServices;
using FuenfzehnZeitWrapper.Errors;
using FuenfzehnZeitWrapper.Extensions;
using FuenfzehnZeitWrapper.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TimeController : ControllerBase
{
  private readonly IFuenfzehnZeitService _fuenfzehnZeitService;

  public TimeController(IFuenfzehnZeitService fuenfzehnZeitService)
  {
    _fuenfzehnZeitService = fuenfzehnZeitService;
  }

  [HttpPost("office/start")]
  public async Task<IResult> StartOffice()
  {
    try
    {
      await _fuenfzehnZeitService.StartOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }

  [HttpPost("office/end")]
  public async Task<IResult> EndOffice()
  {
    try
    {
      await _fuenfzehnZeitService.EndOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }

  [HttpPost("break/start")]
  public async Task<IResult> StartBreak()
  {
    try
    {
      await _fuenfzehnZeitService.StartBreakAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }

  [HttpPost("break/end")]
  public async Task<IResult> EndBreak()
  {
    try
    {
      await _fuenfzehnZeitService.EndBreakAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }

  [HttpPost("home-office/start")]
  public async Task<IResult> StartHomeOffice()
  {
    try
    {
      await _fuenfzehnZeitService.StartHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }

  [HttpPost("home-office/end")]
  public async Task<IResult> EndHomeOffice()
  {
    try
    {
      await _fuenfzehnZeitService.EndHomeOfficeAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }

  [HttpGet("status")]
  public async Task<IResult> GetStatus()
  {
    string status;

    try
    {
      status = await _fuenfzehnZeitService.GetStatusAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }
    catch (InvalidOperationException)
    {
      return Results.BadRequest($"{nameof(GetStatus)} is not possible");
      //TODO: remove invalidoption exceptions
    }

    return Results.Ok(status);
  }

  [HttpGet("working-hours")]
  public async Task<IResult> GetWorkingHours()
  {
    string workingHours;

    try
    {
      workingHours = await _fuenfzehnZeitService.GetWorkingHoursAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }
    catch (InvalidOperationException)
    {
      return Results.BadRequest($"{nameof(GetWorkingHours)} is not possible");
    }

    return Results.Ok(workingHours);
  }
}