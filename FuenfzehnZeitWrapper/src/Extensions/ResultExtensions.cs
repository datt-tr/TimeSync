using FluentResults;
using FuenfzehnZeitWrapper.Extensions;

namespace FuenfzehnZeitWrapper.Errors;

public static class ResultExtensions
{
  extension(Result result)
  {
    public IResult ToProblemDetails()
    {
      ArgumentNullException.ThrowIfNull(result, nameof(result));

      if (result.IsSuccess)
        throw new InvalidOperationException($"{nameof(ToProblemDetails)} must not be called for Success Results");

      var error = result.Errors.OfType<ApiError>().FirstOrDefault()
        ?? throw new NullReferenceException($"{nameof(result)} does not contain the needed error of {nameof(ApiError)}");

      return error.ToProblemDetails();
    }
  }
}