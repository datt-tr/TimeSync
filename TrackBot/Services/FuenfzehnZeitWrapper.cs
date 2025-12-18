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
    var response = await _httpClient.GetAsync("/Time/status");
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    
    return responseString;
  }
}