public interface IWebTerminalService
{
  Task GetLoginPageAsync();
  Task LoginAsync();
  Task GetStatusAsync();
}