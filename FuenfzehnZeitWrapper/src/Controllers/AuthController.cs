using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using FuenfzehnZeitWrapper.Interfaces;
using FluentResults;
using FuenfzehnZeitWrapper.Errors;
using FuenfzehnZeitWrapper.Extensions;
using NLog.LayoutRenderers;

namespace FuenfzehnZeitWrapper.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IFuenfzehnZeitService _fuenfzehnZeitService;

  public AuthController(IFuenfzehnZeitService fuenfzehnZeitService)
  {
    _fuenfzehnZeitService = fuenfzehnZeitService;
  }

  [HttpPost("log-in")]
  public async Task<IResult> LogIn()
  {
    try
    {
      await _fuenfzehnZeitService.GetLogInPageAsync();
      await _fuenfzehnZeitService.LogInAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }

  [HttpPost("log-out")]
  public async Task<IResult> LogOut()
  {
    try
    {
      await _fuenfzehnZeitService.LogOutAsync();
    }
    catch (HttpRequestException)
    {
      return new FuenfzehnZeitHttpRequestError().ToProblem();
    }

    return Results.Ok();
  }
}