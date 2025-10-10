using FunfzehnZeit.Interfaces;
using FunfzehnZeit.Models;

namespace FunfzehnZeit.Services;

internal class UserSessionService : IUserSessionService
{
  private UserSessionData _userSession = new();

  public void CreateSession()
  {
    _userSession = new();
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
    return _userSession.Uid;
  }

  public void UpdateUid(string uid)
  {
    _userSession.Uid = uid;
  }

  public string GetCallNumber()
  {
    return _userSession.CallNumber.ToString();
  }

  public void UpdateCallNumber()
  {
    _userSession.CallNumber += 1;
  }

  public string GetConfirmUid()
  {
    return _userSession.ConfirmUid;
  }

  public void UpdateConfirmUid(string confirmUid)
  {
    _userSession.ConfirmUid = confirmUid;
  }

  public string GetCurrentDate()
  {
    return _userSession.CurrentDate.Date.ToString("dd.MM.yyyy");
  }

  public void UpdateCurrentDate()
  {
    _userSession.CurrentDate = DateTime.Now.Date;
  }
}