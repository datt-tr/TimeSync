using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Errors;

public class ResultError : Error
{
  public string Title { get; init; }
  public int StatusCode { get; init; }
  public string Detail { get; init; }

  public ResultError(string title, int statusCode, string detail) : base(detail)
  {
    Title = title;
    StatusCode = statusCode;
    Detail = detail;
  }
}