using System.Text.RegularExpressions;
using FuenfzehnZeitWrapper.Enums;
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

  public bool ContainsError(string html, ErrorType errorType)
  {
    string errorMessage = errorType switch
    {
      ErrorType.WrongConfirmUid => "Die Anmeldung konnte nicht verifiziert werden, da die Anmeldeseite abgelaufen ist.",
      ErrorType.WrongUid => "Keine Anmeldung gefunden oder Anmeldung abgelaufen",
      ErrorType.WrongCredentials => "Benutzer oder Passwort unbekannt!",
      _ => throw new ArgumentOutOfRangeException($"{nameof(errorType)} doesn't exist")
    };

    bool containsErrorMessage = html.Contains(errorMessage);

    return containsErrorMessage;
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