using FuenfzehnZeitWrapper.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Extensions;

public static class ApiErrorExtensions
{
  extension(ResultError error)
  {
    public IResult ToProblem()
    {
      ArgumentNullException.ThrowIfNull(error, nameof(error));

      return Results.Problem(
        title: error.Title,
        detail: error.Detail,
        statusCode: error.StatusCode);
    }
  }
}