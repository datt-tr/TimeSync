using Microsoft.AspNetCore.Mvc;

namespace Funfzehnzeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{

  [HttpGet]
  public void GetLoginPage()
  {

  }

  [HttpPost]
  public ActionResult Login()
  {
    
    return Ok();
  }
}