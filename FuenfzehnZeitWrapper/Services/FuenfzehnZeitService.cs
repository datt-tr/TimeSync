using Microsoft.Extensions.Options;
using FuenfzehnZeitWrapper.Interfaces;
using FuenfzehnZeitWrapper.Models;

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
      { new StringContent("wlejf92jdjf2j3929ef"), "CONFIRMUID"},
      { new StringContent(_globalVariables.Username), "Username" },
      { new StringContent(_globalVariables.Password), "Password" },
      { new StringContent("Anmelden"), "SELECT" }
    };

    using var response = await _httpClient.PostAsync(string.Empty, formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();

    if (!_htmlParser.IsCorrectConfirmUid(responseString))
    {
      _logger.LogError("Wrong ConfirmUid");
      return;
    }
    _logger.LogDebug("Correct ConfirmUid");

    if (!_htmlParser.IsCorrectCredentials(responseString))
    {
      _logger.LogError("Wrong Credentials");
      return;
    }
    _logger.LogDebug("Correct Credentials");

    var uid = _htmlParser.GetUid(responseString);
    _userSessionService.UpdateUid(uid);
    _userSessionService.UpdateCurrentDate();

    _logger.LogDebug("Uid: {uid}", uid);

    using var followUp = await _httpClient.GetAsync($"?UID={uid}");
    followUp.EnsureSuccessStatusCode();
  }

  public async Task LogOutAsync()
  {
    var uid = _userSessionService.GetUid();
    using var response = await _httpClient.GetAsync($"?LOGOFF_x=1&UID={uid}");
    response.EnsureSuccessStatusCode();
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
    _logger.LogDebug("Working hours: {workingHours}", _htmlParser.GetWorkingHours(responseString));
  }

  public async Task GetStatusAsync()
  {
    _userSessionService.UpdateCallNumber();

    using var formData = GetBasicFormData(0, 1, 100, 0, 0, 0);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);

    var responseString = await response.Content.ReadAsStringAsync();
    if (_htmlParser.IsLoggedIn(responseString))
    {
      _logger.LogDebug("Status: {status}", _htmlParser.GetStatus(responseString));
    }
    else
    {
      _logger.LogDebug("Not Logged In");
    }
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