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
  private readonly ILogger _logger;

  public WebTerminalService(HttpClient httpClient, IOptions<GlobalVariables> globalVariables, IUserSessionService userSessionService, ILogger<WebTerminalService> logger)
  {
    _logger = logger;
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
      var confirmUid = GetConfirmUidFromHtml(responseString);
      _userSessionService.UpdateConfirmUid(confirmUid);
      _logger.LogDebug($"ConfirmUid: {confirmUid}");
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
      var uid = GetUidFromHtml(responseString);
      _userSessionService.UpdateUid(uid);
      _userSessionService.UpdateCurrentDate();
      _logger.LogDebug($"Uid: {uid}");

      using var followUp = await _httpClient.GetAsync($"?UID={uid}");
    }
  }

  public async Task GetStatusAsync()
  {
    _userSessionService.UpdateCallNumber();

    var formData = new MultipartFormDataContent()
    {
      {new StringContent(_userSessionService.GetCallNumber()), "CALL_NO" },
      {new StringContent(_userSessionService.GetUid()), "UID"},
      {new StringContent("ZZStandard.css"), "CSS_FILE"},
      {new StringContent("0"), "PAGEONLY"},
      {new StringContent("1"), "SELECTED_MENU"},
      {new StringContent("100"), "SELECTED_SUB_MENU"},
      {new StringContent(_userSessionService.GetCurrentDate()), "SELECTED_DATE"},
      {new StringContent("0"), "SELECTED_FUNCTION"},
      {new StringContent("0"), "SELECTED_VALUE"},
      {new StringContent("0"), "SELECTED_SUB_VALUE"},
    };
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    if (response.IsSuccessStatusCode) {
      var responseString = await response.Content.ReadAsStringAsync();
      _logger.LogDebug(GetStatusFromHtml(responseString));
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

  private static string GetStatusFromHtml(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string status = htmlDoc.DocumentNode.SelectSingleNode("//td[@class='wtStatusContent']").InnerText.Trim();
    return status;
  }
}