namespace FuenfzehnZeitWrapper.Interfaces;

public interface IFuenfzehnZeitService
{
  Task GetLoginPageAsync();
  Task LoginAsync();
  Task StartOfficeAsync();
  Task EndOfficeAsync();
  Task StartBreakAsync();
  Task EndBreakAsync();
  Task StartHomeOfficeAsync();
  Task EndHomeOfficeAsync();
  Task GetStatusAsync();
  Task GetWorkingHoursAsync();
}