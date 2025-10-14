namespace FuenfzehnZeit.Interfaces;

public interface IUserSessionService
{
  void CreateSession();
  void GetSession();
  void DeleteSession();

  string GetCallNumber();
  void UpdateCallNumber();
  string GetUid();
  void UpdateUid(string uid);
  string GetConfirmUid();
  void UpdateConfirmUid(string confirmUid);
  string GetCurrentDate();
  void UpdateCurrentDate();
}