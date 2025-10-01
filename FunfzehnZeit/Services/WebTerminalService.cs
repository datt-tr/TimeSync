using System.Text.Json;
using Azure;
using FunfzehnZeit.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace FunfzehnZeit.Services;

public class WebTerminalService
{
  private readonly HttpClient _httpClient;
  private readonly GlobalVariables _globalVariables;

  public WebTerminalService(HttpClient httpClient, IOptions<GlobalVariables> globalVariables)
  {
    _httpClient = httpClient;
    _globalVariables = globalVariables.Value;

    _httpClient.BaseAddress = new Uri(_globalVariables.BaseUrl);
  }

  public async Task GetLoginPageAsync()
  {
    using var response = await _httpClient.GetAsync("");
    response.EnsureSuccessStatusCode();
    if (response.IsSuccessStatusCode)
    {
      var responseString = await response.Content.ReadAsStringAsync();
      Console.WriteLine(responseString);
      ParseHtml(responseString);
    }
  }

  public void ParseHtml(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string confirmUid = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='CONFIRMUID']").Attributes["value"].Value;
    Console.WriteLine($"CONFIRMUID: {confirmUid}");
  }
}