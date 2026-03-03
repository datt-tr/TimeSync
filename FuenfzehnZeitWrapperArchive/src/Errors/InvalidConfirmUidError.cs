namespace FuenfzehnZeitWrapper.Errors;

public class InvalidConfirmUidError : ResultError
{
  public InvalidConfirmUidError() : base(
    title: "The confirmation link is invalid or has expired",
    statusCode: StatusCodes.Status404NotFound,
    detail: "The provided by user Confirm Uid is invalid"
  )
  { }
}