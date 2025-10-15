using System.Text.RegularExpressions;
using FuenfzehnZeitWrapper.Interfaces;
using HtmlAgilityPack;

namespace FuenfzehnZeitWrapper.Helpers;

internal class FuenfzehnZeitHtmlParser : IFuenfzehnZeitHtmlParser
{
  public string GetConfirmUid(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string confirmUid = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='CONFIRMUID']").Attributes["value"].Value;

    return confirmUid;
  }

  public string GetUid(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string uid = htmlDoc.DocumentNode.SelectSingleNode("//meta[@http-equiv='refresh']").Attributes["content"].Value.Split("UID=")[1];

    return uid;
  }

  public string GetStatus(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string status = htmlDoc.DocumentNode.SelectSingleNode("//td[@class='wtStatusContent']").InnerText.Trim();

    return status;
  }

  public string GetWorkingHours(string html)
  {
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    string currentDayString = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='msg_table']/tr/td").InnerText.Trim();
    string hoursPattern = @"\d{2}:\d{2}";
    var workingHours = Regex.Match(currentDayString, hoursPattern).Value;

    return workingHours;
  }
}