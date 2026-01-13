namespace FuenfzehnZeitWrapper.Errors;

public class FuenfzehnZeitHttpRequestError : ResultError
{
  public FuenfzehnZeitHttpRequestError() : base(
    title: "15zeit Server Error",
    statusCode: StatusCodes.Status502BadGateway,
    detail: "Failed to execute FuenfzehnZeit Server Request"
  )
  { }
}