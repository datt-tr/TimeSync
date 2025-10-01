using FunfzehnZeit.Models;
using Microsoft.Extensions.Options;
namespace Funfzehnzeit.Services;

public class Application : IApplication
{
  private readonly GlobalVariables _globalVariables;

  public Application(IOptions<GlobalVariables> globalVariables)
  {
    _globalVariables = globalVariables.Value;
  }
  public void Start()
  {
    Console.WriteLine("Application started");
    Console.WriteLine(_globalVariables.BaseUrl);
    Console.WriteLine("Application finished");
}
}