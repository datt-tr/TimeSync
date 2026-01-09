using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Errors;

public class ApiError : Error
{
  public string Title { get; init; }
  public int StatusCode { get; init; }
  public string Detail { get; init; }

  public ApiError (string title, int statusCode, string detail) : base(detail)
  {
    Title = title;
    StatusCode = statusCode;
    Detail = detail;
  }
}