using System.Net.Http;
using System.Threading.Tasks;

internal class FuenfzehnZeitWrapper : IFuenfzehnZeitWrapper
{
  private readonly HttpClient _httpClient;
  private readonly GlobalVariables _globalVariables;

  public FuenfzehnZeitWrapper(HttpClient httpClient, GlobalVariables globalVariables)
  {
    _httpClient = httpClient;
    _globalVariables = globalVariables;

    _httpClient.BaseAddress = new System.Uri(_globalVariables.BaseUrl + "/api/v1");
  }

  public async Task<string> GetStatusAsync()
  {
    var response = await _httpClient.GetAsync("/time/status");

    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadAsStringAsync();
    }

    return "Can't get status";
  }
}