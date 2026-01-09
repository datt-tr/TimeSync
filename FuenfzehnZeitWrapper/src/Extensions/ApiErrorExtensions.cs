using FuenfzehnZeitWrapper.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FuenfzehnZeitWrapper.Extensions;

public static class ApiErrorExtensions
{
  extension(ApiError error)
  {
    public IResult ToProblemDetails()
    {
      ArgumentNullException.ThrowIfNull(error, nameof(error));

      return Results.Problem(
        title: error.Title,
        detail: error.Detail,
        statusCode: error.StatusCode);
    }
  }
}