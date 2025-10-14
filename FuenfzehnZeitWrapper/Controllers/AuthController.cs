using System.Threading.Tasks;
using FuenfzehnZeit.Services;
using Microsoft.AspNetCore.Mvc;
using FuenfzehnZeit.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Funfzehnzeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IFuenfzehnZeitService _fuenfzehnZeitService;
  private readonly ILogger _logger;

  public AuthController(IFuenfzehnZeitService fuenfzehnZeitService, ILogger<AuthController> logger)
  {
    _logger = logger;
    _fuenfzehnZeitService = fuenfzehnZeitService;
  }

  [HttpPost("login")]
  public async Task<Results<Ok<string>, BadRequest<string>>> Login()
  {
    try
    {
      await _fuenfzehnZeitService.GetLoginPageAsync();
      await _fuenfzehnZeitService.LoginAsync();
    }
    catch (HttpRequestException)
    {
      return TypedResults.BadRequest("Failed FuenfzehnZeit Server Request");
    }

    return TypedResults.Ok("Login executed");
  }
}