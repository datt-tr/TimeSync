namespace FuenfzehnZeitWrapper.Errors;

public class InvalidCredentialsError : ResultError
{
  public InvalidCredentialsError() : base(
    title: "The credentials are incorrect",
    statusCode: StatusCodes.Status401Unauthorized,
    detail: "The provided email or password is incorrect"
  )
  { }
}