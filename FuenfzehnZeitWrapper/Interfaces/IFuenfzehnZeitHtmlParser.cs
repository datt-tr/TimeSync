namespace FuenfzehnZeitWrapper.Interfaces;

public interface IFuenfzehnZeitHtmlParser
{
  bool IsLoggedIn(string html);
  bool IsCorrectCredentials(string html);
  bool IsCorrectConfirmUid(string html);
  string GetConfirmUid(string html);
  string GetUid(string html);
  string GetStatus(string html);
  string GetWorkingHours(string html);
}