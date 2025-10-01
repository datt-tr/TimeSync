namespace FunfzehnZeit.Services;

public class WebTerminalService
{
  private readonly HttpClient _httpClient;
  private readonly IConfiguration _config;

  public WebTerminalService(HttpClient httpClient, IConfiguration config)
  {
    _httpClient = httpClient;
    _config = config;

    _httpClient.BaseAddress = new Uri(_config.GetValue<string>("baseUrl"));
  }
}