using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using FuenfzehnZeitWrapper.Interfaces;

namespace FuenfzehnzeitWrapper.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IFuenfzehnZeitService _fuenfzehnZeitService;
  private readonly ILogger _logger;
  private readonly string _fuenfzehnZeitError = "Failed FuenfzehnZeit Server Request";

  public AuthController(IFuenfzehnZeitService fuenfzehnZeitService, ILogger<AuthController> logger)
  {
    _logger = logger;
    _fuenfzehnZeitService = fuenfzehnZeitService;
  }

  [HttpPost("log-in")]
  public async Task<Results<Ok, BadRequest<string>>> LogIn()
  {
    try
    {
      await _fuenfzehnZeitService.GetLogInPageAsync();
      await _fuenfzehnZeitService.LogInAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }

    return TypedResults.Ok();
  }

  [HttpPost("log-out")]
  public async Task<Results<Ok, BadRequest<string>>> LogOut()
  {
    try
    {
      await _fuenfzehnZeitService.LogOutAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest(_fuenfzehnZeitError);
    }
    return TypedResults.Ok();
  }
}