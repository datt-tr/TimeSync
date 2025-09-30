using Microsoft.AspNetCore.Mvc;

namespace Funfzehnzeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController: ControllerBase {

    private readonly string _baseUrl = "http://15zeit/STIntro.php";

  [HttpGet]
  public void GetLoginPage() {

  }
}