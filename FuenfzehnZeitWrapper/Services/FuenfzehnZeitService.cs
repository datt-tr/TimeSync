using System.Text.Json;
using Azure;
using FuenfzehnZeit.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using FuenfzehnZeit.Interfaces;
using System.Text.RegularExpressions;

namespace FuenfzehnZeit.Services;

internal class FuenfzehnZeitService : IFuenfzehnZeitService
{
  private readonly HttpClient _httpClient;
  private readonly GlobalVariables _globalVariables;
  private readonly IUserSessionService _userSessionService;
  private readonly ILogger _logger;

  public FuenfzehnZeitService(HttpClient httpClient, IOptions<GlobalVariables> globalVariables, IUserSessionService userSessionService, ILogger<FuenfzehnZeitService> logger)
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
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var confirmUid = GetConfirmUidFromHtml(responseString);
    _userSessionService.UpdateConfirmUid(confirmUid);

    _logger.LogDebug("ConfirmUid: {confirmUid}", confirmUid);
  }

  public async Task LoginAsync()
  {
    using var formData = new MultipartFormDataContent
    {
      { new StringContent(_userSessionService.GetConfirmUid()), "CONFIRMUID" },
      { new StringContent(_globalVariables.Username), "Username" },
      { new StringContent(_globalVariables.Password), "Password" },
      { new StringContent("Anmelden"), "SELECT" }
    };

    using var response = await _httpClient.PostAsync(string.Empty, formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var uid = GetUidFromHtml(responseString);
    _userSessionService.UpdateUid(uid);
    _userSessionService.UpdateCurrentDate();

    _logger.LogDebug("Uid: {uid}", uid);

    using var followUp = await _httpClient.GetAsync($"?UID={uid}");
    followUp.EnsureSuccessStatusCode();
  }

  public async Task StartOfficeAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 101, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();
  }

  public async Task EndOfficeAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 102, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();
  }

  public async Task StartBreakAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 103, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();
  }

  public async Task EndBreakAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 104, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();
  }

  public async Task StartHomeOfficeAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 119, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();
  }

  public async Task EndHomeOfficeAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 118, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();
  }

  public async Task GetWorkingHoursAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 113, 1, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    _logger.LogDebug("Working hours: {workingHours}", GetWorkingHoursFromHtml(responseString));
  }

  public async Task GetStatusAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 0, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    _logger.LogDebug("Status: {status}", GetStatusFromHtml(responseString));
  }

  private MultipartFormDataContent GetBasicFormData(int pageOnly, int selectedMenu, int selectedSubMenu, int selectedFunction, int selectedValue, int selectedSubValue)
  {
    var formData = new MultipartFormDataContent()
    {
      {new StringContent(_userSessionService.GetCallNumber()), "CALL_NO" },
      {new StringContent(_userSessionService.GetUid()), "UID"},
      {new StringContent("ZZStandard.css"), "CSS_FILE"},
      {new StringContent(pageOnly.ToString()), "PAGEONLY"},
      {new StringContent(selectedMenu.ToString()), "SELECTED_MENU"},
      {new StringContent(selectedSubMenu.ToString()), "SELECTED_SUB_MENU"},
      {new StringContent(_userSessionService.GetCurrentDate()), "SELECTED_DATE"},
      {new StringContent(selectedFunction.ToString()), "SELECTED_FUNCTION"},
      {new StringContent(selectedValue.ToString()), "SELECTED_VALUE"},
      {new StringContent(selectedSubValue.ToString()), "SELECTED_SUB_VALUE"},
    };

    return formData;
  }

  private static string GetWorkingHoursFromHtml(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string currentDayString = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='msg_table']/tr/td").InnerText.Trim();
    string hoursPattern = @"\d{2}:\d{2}";
    var workingHours = Regex.Match(currentDayString, hoursPattern).Value;

    return workingHours;
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