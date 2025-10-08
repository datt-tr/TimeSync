using System.Text.Json;
using Azure;
using FunfzehnZeit.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using FunfzehnZeit.Interfaces;

namespace FunfzehnZeit.Services;

internal class WebTerminalService : IWebTerminalService
{
  private readonly HttpClient _httpClient;
  private readonly GlobalVariables _globalVariables;
  private readonly IUserSessionService _userSessionService;

  public WebTerminalService(HttpClient httpClient, IOptions<GlobalVariables> globalVariables, IUserSessionService userSessionService)
  {
    _userSessionService = userSessionService;
    _httpClient = httpClient;
    _globalVariables = globalVariables.Value;

    _httpClient.BaseAddress = new Uri(_globalVariables.BaseUrl);
  }

  public async Task GetLoginPageAsync()
  {
    using var response = await _httpClient.GetAsync(string.Empty);
    if (response.IsSuccessStatusCode)
    {
      var responseString = await response.Content.ReadAsStringAsync();
      _userSessionService.UpdateConfirmUid(GetConfirmUidFromHtml(responseString));
      // Console.WriteLine($"CONFIRMUID: {GetConfirmUidFromHtml(responseString)}");
    }
  }

  public async Task LoginAsync()
  {
    var formData = new MultipartFormDataContent
    {
      { new StringContent(_userSessionService.GetConfirmUid()), "CONFIRMUID" },
      { new StringContent(_globalVariables.Username), "Username" },
      { new StringContent(_globalVariables.Password), "Password" },
      { new StringContent("Anmelden"), "SELECT" }
    };

    using var response = await _httpClient.PostAsync(string.Empty, formData);
    if (response.IsSuccessStatusCode)
    {
      var responseString = await response.Content.ReadAsStringAsync();
      _userSessionService.UpdateUid(GetUidFromHtml(responseString));
      // Console.WriteLine(GetUidFromHtml(responseString));

      using var followUp = await _httpClient.GetAsync($"?UID={_userSessionService.GetUid}");
      _userSessionService.UpdateCurrentDate();
    }
  }

  

  private static string GetUidFromHtml(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string uid = htmlDoc.DocumentNode.SelectSingleNode("//meta[@http-equiv='refresh']").Attributes["content"].Value.Split("UID=")[1];

    return uid;
  }
  private static string GetConfirmUidFromHtml(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string confirmUid = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='CONFIRMUID']").Attributes["value"].Value;

    return confirmUid;
  }
}