namespace FuenfzehnZeitWrapper.Errors;

public class FuenfzehnZeitHttpRequestError : ApiError
{
  public FuenfzehnZeitHttpRequestError() : base(
    title: "15zeit Server Error",
    statusCode: StatusCodes.Status502BadGateway,
    detail: "Failed FuenfzehnZeit Server Request"
  )
  { }
}