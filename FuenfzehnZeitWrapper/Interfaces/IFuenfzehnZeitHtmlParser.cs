using FuenfzehnZeitWrapper.Enums;

namespace FuenfzehnZeitWrapper.Interfaces;

public interface IFuenfzehnZeitHtmlParser
{
  bool ContainsError(string html, ErrorType erroType);
  string GetConfirmUid(string html);
  string GetUid(string html);
  string GetStatus(string html);
  string GetWorkingHours(string html);
}