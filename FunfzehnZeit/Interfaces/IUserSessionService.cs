namespace FunfzehnZeit.Interfaces;

public interface IUserSessionService
{
  void CreateSession();
  void GetSession();
  void DeleteSession();

  int GetCallNumber();
  void UpdateCallNumber();
  string GetUid();
  void UpdateUid(string uid);
  string GetConfirmUid();
  void UpdateConfirmUid(string confirmUid);
  string GetCurrentDate();
  void UpdateCurrentDate();
}