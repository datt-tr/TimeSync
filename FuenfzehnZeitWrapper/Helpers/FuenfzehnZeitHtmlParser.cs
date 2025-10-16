using System.Text.RegularExpressions;
using FuenfzehnZeitWrapper.Interfaces;
using HtmlAgilityPack;

namespace FuenfzehnZeitWrapper.Helpers;

internal class FuenfzehnZeitHtmlParser : IFuenfzehnZeitHtmlParser
{
  private readonly ILogger _logger;

  public FuenfzehnZeitHtmlParser(ILogger<FuenfzehnZeitHtmlParser> logger)
  {
    _logger = logger;
  }

  public bool IsLoggedIn(string html)
  {
    var htmlDoc = GetHtmlDocument(html);

    bool isLoggedIn = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='fs_msg_content']")?.InnerText.Trim() != "Keine Anmeldung gefunden oder Anmeldung abgelaufen";

    return isLoggedIn;
  }

  public string GetConfirmUid(string html)
  {
    var htmlDoc = GetHtmlDocument(html);

    string confirmUid = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='CONFIRMUID']").Attributes["value"].Value;

    return confirmUid;
  }

  public string GetUid(string html)
  {
    var htmlDoc = GetHtmlDocument(html);

    string uid = htmlDoc.DocumentNode.SelectSingleNode("//meta[@http-equiv='refresh']").Attributes["content"].Value.Split("UID=")[1];

    return uid;
  }

  public string GetStatus(string html)
  {
    var htmlDoc = GetHtmlDocument(html);

    string status = htmlDoc.DocumentNode.SelectSingleNode("//td[@class='wtStatusContent']").InnerText.Trim();

    return status;
  }

  public string GetWorkingHours(string html)
  {
    var htmlDoc = GetHtmlDocument(html);

    string currentDayString = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='msg_table']/tr/td").InnerText.Trim();
    string hoursPattern = @"\d{2}:\d{2}";
    var workingHours = Regex.Match(currentDayString, hoursPattern).Value;

    return workingHours;
  }

  private static HtmlDocument GetHtmlDocument(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    return htmlDoc;
  }
}