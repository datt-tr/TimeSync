using FuenfzehnZeitWrapper.Interfaces;
using FuenfzehnZeitWrapper.Models;

namespace FuenfzehnZeitWrapper.Services;

internal class UserSessionService : IUserSessionService
{
  private readonly ILogger _logger;
  private UserSessionData _userSession = new();

  public UserSessionService(ILogger<UserSessionService> logger)
  {
    _logger = logger;
  }

  public string GetUid()
  {
    var uid = _userSession.Uid;
    
    _logger.LogDebug("{GetUid} retrieves {uid}", nameof(GetUid), uid);
    
    return uid;
  }

  public void UpdateUid(string uid)
  {
    _userSession.Uid = uid;
  }

  public string GetCallNumber()
  {
    var callNumber = _userSession.CallNumber.ToString();
    
    _logger.LogDebug("{GetCallNumber} retrieves {callNumber}", nameof(GetCallNumber), callNumber);
    
    return callNumber;
  }

  public void UpdateCallNumber()
  {
    _userSession.CallNumber += 1;
  }

  public string GetConfirmUid()
  {
    var confirmUid = _userSession.ConfirmUid; 
    
    _logger.LogDebug("{GetConfirmUid} retrieves {confirmUid}", nameof(GetConfirmUid), confirmUid);

    return confirmUid;
  }

  public void UpdateConfirmUid(string confirmUid)
  {
    _userSession.ConfirmUid = confirmUid;
  }

  public string GetCurrentDate()
  {
    var currentDate = _userSession.CurrentDate.Date.ToString("dd.MM.yyyy");

    _logger.LogDebug("{GetCurrentDate} retrieves {currentDate}", nameof(GetCurrentDate), currentDate);

    return currentDate;
  }

  public void UpdateCurrentDate()
  {
    _userSession.CurrentDate = DateTime.Now.Date;
  }
}