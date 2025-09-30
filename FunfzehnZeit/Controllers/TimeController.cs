using Microsoft.AspNetCore.Mvc;

namespace FunfzehnZeit.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TimeController : ControllerBase
{
  [HttpPost]
  public ActionResult<string> GetStatus()
  {
    return "";
  }
}