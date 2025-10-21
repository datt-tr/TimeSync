using FuenfzehnZeitWrapper.Enums;
using FuenfzehnZeitWrapper.Interfaces;
using FuenfzehnZeitWrapper.Models;
using Microsoft.Extensions.Options;
namespace FuenfzehnZeitWrapper.Services;

internal class FormDataBuilder : IFormDataBuilder
{
  private readonly IUserSessionService _userSessionService;
  private readonly GlobalVariables _globalVariables;

  public FormDataBuilder(IUserSessionService userSessionService, IOptions<GlobalVariables> globalVariables)
  {
    _userSessionService = userSessionService;
    _globalVariables = globalVariables.Value;
  }

  public MultipartFormDataContent Build(RequestType type)
  {
    return type switch
    {
      RequestType.LogIn => GetLoginFormData(),
      RequestType.StartOffice => GetBasicFormData(0, 1, 100, 101, 0, 0),
      RequestType.EndOffice => GetBasicFormData(0, 1, 100, 102, 0, 0),
      RequestType.StartBreak => GetBasicFormData(0, 1, 100, 103, 0, 0),
      RequestType.EndBreak => GetBasicFormData(0, 1, 100, 104, 0, 0),
      RequestType.StartHomeOffice => GetBasicFormData(0, 1, 100, 119, 0, 0),
      RequestType.EndHomeOffice => GetBasicFormData(0, 1, 100, 118, 0, 0),
      RequestType.GetWorkingHours => GetBasicFormData(0, 1, 100, 113, 1, 0),
      RequestType.GetStatus => GetBasicFormData(0, 1, 100, 0, 0, 0),
      _ => throw new NotSupportedException($"{nameof(RequestType)} of {nameof(type)} is not supported")
    };
  }

  private MultipartFormDataContent GetLoginFormData()
  {
    return new MultipartFormDataContent
    {
      { new StringContent(_userSessionService.GetConfirmUid()), "CONFIRMUID"},
      { new StringContent(_globalVariables.Username), "Username" },
      { new StringContent(_globalVariables.Password), "Password" },
      { new StringContent("Anmelden"), "SELECT" }
    };
  }

  private MultipartFormDataContent GetBasicFormData(int pageOnly, int selectedMenu, int selectedSubMenu, int selectedFunction, int selectedValue, int selectedSubValue)
  {
    return new MultipartFormDataContent()
    {
      {new StringContent(_userSessionService.GetCallNumber()), "CALL_NO" },
      {new StringContent(_userSessionService.GetUid()), "UID"},
      {new StringContent("ZZStandard.css"), "CSS_FILE"},
      {new StringContent(pageOnly.ToString()), "PAGEONLY"},
      {new StringContent(selectedMenu.ToString()), "SELECTED_MENU"},
      {new StringContent(selectedSubMenu.ToString()), "SELECTED_SUB_MENU"},
      {new StringContent(_userSessionService.GetCurrentDate()), "SELECTED_DATE"},
      {new StringContent(selectedFunction.ToString()), "SELECTED_FUNCTION"},
      {new StringContent(selectedValue.ToString()), "SELECTED_VALUE"},
      {new StringContent(selectedSubValue.ToString()), "SELECTED_SUB_VALUE"},
    };
  }
}