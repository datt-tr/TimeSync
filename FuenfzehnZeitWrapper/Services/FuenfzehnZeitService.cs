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
  private readonly IFormDataBuilder _formDataBuilder;

  public FuenfzehnZeitService(HttpClient httpClient, IOptions<GlobalVariables> globalVariables, IUserSessionService userSessionService, ILogger<FuenfzehnZeitService> logger, IFuenfzehnZeitHtmlParser htmlParser, IFormDataBuilder formDataBuilder)
  {
    _formDataBuilder = formDataBuilder;
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
    using var formData = _formDataBuilder.Build(RequestType.LogIn);

    using var response = await _httpClient.PostAsync(string.Empty, formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();

    if (_htmlParser.ContainsError(responseString, ErrorType.InvalidConfirmUid))
    {
      _logger.LogError("Invalid ConfirmUid");
      return;
    }

    if (_htmlParser.ContainsError(responseString, ErrorType.InvalidCredentials))
    {
      _logger.LogError("Invalid Credentials");
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
    using var formData = _formDataBuilder.Build(RequestType.StartOffice);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task EndOfficeAsync()
  {
    using var formData = _formDataBuilder.Build(RequestType.EndOffice);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task StartBreakAsync()
  {
    using var formData = _formDataBuilder.Build(RequestType.StartBreak);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task EndBreakAsync()
  {
    using var formData = _formDataBuilder.Build(RequestType.EndBreak);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task StartHomeOfficeAsync()
  {
    using var formData = _formDataBuilder.Build(RequestType.StartHomeOffice);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task EndHomeOfficeAsync()
  {
    using var formData = _formDataBuilder.Build(RequestType.EndHomeOffice);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _userSessionService.UpdateCallNumber();
  }

  public async Task GetWorkingHoursAsync()
  {
    using var formData = _formDataBuilder.Build(RequestType.GetWorkingHours);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _logger.LogDebug("Working hours: {workingHours}", _htmlParser.GetWorkingHours(responseString));

    _userSessionService.UpdateCallNumber();
  }

  public async Task GetStatusAsync()
  {
    using var formData = _formDataBuilder.Build(RequestType.GetStatus);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return;

    _logger.LogDebug("Status: {status}", _htmlParser.GetStatus(responseString));

    _userSessionService.UpdateCallNumber();
  }

  private async Task<string> SendWebTerminalRequestAsync(RequestType type)
  {
    using var formData = _formDataBuilder.Build(type);
    using var response = await _httpClient.PostAsync($"?UID={_userSessionService.GetUid()}", formData);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    if (!IsLoggedIn(responseString) || !IsValidOrder(responseString)) return string.Empty;

    _userSessionService.UpdateCallNumber();

    return responseString;
  }

  private bool IsLoggedIn(string html)
  {
    if (_htmlParser.ContainsError(html, ErrorType.InvalidUid))
    {
      _logger.LogError("Not Logged In ({error})", nameof(ErrorType.InvalidUid));

      return false;
    }

    return true;
  }

  private bool IsValidOrder(string html)
  {
    if (_htmlParser.ContainsError(html, ErrorType.InvalidCallNumber))
    {
      _logger.LogError("Wrong Order ({error})", nameof(ErrorType.InvalidCallNumber));

      return false;
    }

    return true;
  }
}