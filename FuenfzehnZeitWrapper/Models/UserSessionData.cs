namespace FuenfzehnZeitWrapper.Models;

public class UserSessionData
{
  public string ConfirmUid { get; set; } = string.Empty;
  public string Uid { get; set; } = string.Empty;
  public int CallNumber { get; set; } = 1;
  public DateTime CurrentDate { get; set; } = DateTime.Now.Date;
}