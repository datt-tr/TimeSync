using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

internal class FuenfzehnZeitWrapper : IFuenfzehnZeitWrapper
{
  private readonly HttpClient _httpClient;
  private readonly GlobalVariables _globalVariables;

  public FuenfzehnZeitWrapper(HttpClient httpClient, IOptions<GlobalVariables> globalVariables)
  {
    _httpClient = httpClient;
    _globalVariables = globalVariables.Value;

    _httpClient.BaseAddress = new Uri(_globalVariables.BaseUrl);
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