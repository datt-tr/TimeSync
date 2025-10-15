namespace FuenfzehnZeitWrapper.Interfaces;

public interface IFuenfzehnZeitHtmlParser
{
  string GetConfirmUid(string html);
  string GetUid(string html);
  string GetStatus(string html);
  string GetWorkingHours(string html);
}