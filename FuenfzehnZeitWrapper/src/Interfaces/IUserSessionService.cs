namespace FuenfzehnZeitWrapper.Interfaces;

public interface IUserSessionService
{
  string GetCallNumber();
  void UpdateCallNumber();
  string GetUid();
  void UpdateUid(string uid);
  string GetConfirmUid();
  void UpdateConfirmUid(string confirmUid);
  string GetCurrentDate();
  void UpdateCurrentDate();
}