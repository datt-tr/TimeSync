using Microsoft.Extensions.Options;
using FuenfzehnZeitWrapper.Interfaces;
using FuenfzehnZeitWrapper.Models;
using FuenfzehnZeitWrapper.Enums;

namespace FuenfzehnZeitWrapper.Services;

internal class FuenfzehnZeitService : IFuenfzehnZeitService
{
  private readonly HttpClient _httpClient;
  private readonly GlobalVariables _globalVariables;
  private readonly IUserSessionService _userSessionService;
  private readonly ILogger _logger;
  private readonly IFuenfzehnZeitHtmlParser _htmlParser;

  public FuenfzehnZeitService(HttpClient httpClient, IOptions<GlobalVariables> globalVariables, IUserSessionService userSessionService, ILogger<FuenfzehnZeitService> logger, IFuenfzehnZeitHtmlParser htmlParser)
  {
    _htmlParser = htmlParser;
    _logger = logger;
    _userSessionService = userSessionService;
    _httpClient = httpClient;
    _globalVariables = globalVariables.Value;

    _httpClient.BaseAddress = new Uri(_globalVariables.BaseUrl);
  }

  public async Task GetLogInPageAsync()
  {
    using var response = await _httpClient.GetAsync(string.Empty);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var confirmUid = _htmlParser.GetConfirmUid(responseString);
    _userSessionService.UpdateConfirmUid(confirmUid);

    _logger.LogDebug("ConfirmUid: {confirmUid}", confirmUid);
  }

  public async Task LogInAsync()
  {
    using var formData = new MultipartFormDataContent
    {
      { new StringContent(_userSessionService.GetConfirmUid()), "CONFIRMUID"},
      { new StringContent(_globalVariables.Username), "Username" },
      { new StringContent(_globalVariables.Password), "Password" },
      { new StringContent("Anmelden"), "SELECT" }
    };

    using var response = await _httpClient.PostAsync(string.Empty, formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();

    if (_htmlParser.ContainsError(responseString, ErrorType.WrongConfirmUid))
    {
      _logger.LogError("Wrong ConfirmUid");
      return;
    }

    if (_htmlParser.ContainsError(responseString, ErrorType.WrongCredentials))
    {
      _logger.LogError("Wrong Credentials");
      return;
    }

    var uid = _htmlParser.GetUid(responseString);
    _userSessionService.UpdateUid(uid);
    _userSessionService.UpdateCurrentDate();

    _logger.LogDebug("Uid: {uid}", uid);

    // FuenfzehnZeit requires to be redirected to this page to continue
    using var followUp = await _httpClient.GetAsync($"?UID={uid}");
    followUp.EnsureSuccessStatusCode();
  }

  public async Task LogOutAsync()
  {
    var uid = _userSessionService.GetUid();
    using var response = await _httpClient.GetAsync($"?LOGOFF_x=1&UID={uid}");
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;
  }

  public async Task StartOfficeAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 101, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task EndOfficeAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 102, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task StartBreakAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 103, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task EndBreakAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 104, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task StartHomeOfficeAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 119, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task EndHomeOfficeAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 118, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task GetWorkingHoursAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 113, 1, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _logger.LogDebug("Working hours: {workingHours}", _htmlParser.GetWorkingHours(responseString));

    _userSessionService.UpdateCallNumber();
  }

  public async Task GetStatusAsync()
  {
    using var formData = GetBasicFormData(0, 1, 100, 0, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString)) return;

    _logger.LogDebug("Status: {status}", _htmlParser.GetStatus(responseString));

    _userSessionService.UpdateCallNumber();
  }

  private bool IsLoggedIn(string html)
  {
    if (_htmlParser.ContainsError(html, ErrorType.WrongUid))
    {
      _logger.LogError("Not Logged In");

      return false;
    }

    return true;
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
}