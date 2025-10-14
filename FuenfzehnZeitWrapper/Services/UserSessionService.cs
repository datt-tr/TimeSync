using FuenfzehnZeit.Interfaces;
using FuenfzehnZeit.Models;

namespace FuenfzehnZeit.Services;

internal class UserSessionService : IUserSessionService
{
  private readonly ILogger _logger;
  private UserSessionData _userSession = new();

  public UserSessionService(ILogger<UserSessionService> logger)
  {
    _logger = logger;
  }
  public void CreateSession()
  {
    throw new NotImplementedException();
  }

  public void GetSession()
  {
    throw new NotImplementedException();
  }

  public void DeleteSession()
  {
    throw new NotImplementedException();
  }
  public string GetUid()
  {
    _logger.LogDebug($"{nameof(GetUid)} retrieves {_userSession.Uid}");
    return _userSession.Uid;
  }

  public void UpdateUid(string uid)
  {
    _userSession.Uid = uid;
  }

  public string GetCallNumber()
  {
    _logger.LogDebug($"{nameof(GetCallNumber)} retrieves {_userSession.CallNumber}");
    return _userSession.CallNumber.ToString();
  }

  public void UpdateCallNumber()
  {
    _userSession.CallNumber += 1;
  }

  public string GetConfirmUid()
  {
    _logger.LogDebug($"{nameof(GetConfirmUid)} retrieves {_userSession.ConfirmUid}");
    return _userSession.ConfirmUid;
  }

  public void UpdateConfirmUid(string confirmUid)
  {
    _userSession.ConfirmUid = confirmUid;
  }

  public string GetCurrentDate()
  {
    _logger.LogDebug($"{nameof(GetCurrentDate)} retrieves {_userSession.CurrentDate}");
    return _userSession.CurrentDate.Date.ToString("dd.MM.yyyy");
  }

  public void UpdateCurrentDate()
  {
    _userSession.CurrentDate = DateTime.Now.Date;
  }
}