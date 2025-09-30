namespace Funfzehnzeit.Services;
public class Application : IApplication
{
    private readonly IConfiguration _config;

    public Application(IConfiguration config)
    {
        _config = config;
    }
  public void Start()
  {
        Console.WriteLine("Application started");
        Console.WriteLine(_config.GetValue<string>("username"));
  }
}