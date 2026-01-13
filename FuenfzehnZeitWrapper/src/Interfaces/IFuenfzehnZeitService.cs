using FluentResults;

namespace FuenfzehnZeitWrapper.Interfaces;

public interface IFuenfzehnZeitService
{
  Task GetLogInPageAsync();
  Task<Result> LogInAsync();
  Task LogOutAsync();
  Task StartOfficeAsync();
  Task EndOfficeAsync();
  Task StartBreakAsync();
  Task EndBreakAsync();
  Task StartHomeOfficeAsync();
  Task EndHomeOfficeAsync();
  Task<string> GetStatusAsync();
  Task<string> GetWorkingHoursAsync();
}