namespace Funfzehnzeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController: ControllerBase {

  private string _baseUrl = "http://15zeit/STIntro.php"

  [HttpGet]
  public ActionResult GetLoginPage() {

  }
}