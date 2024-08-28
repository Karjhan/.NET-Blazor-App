using Domain.Primitives;

namespace Domain.Exceptions;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    public static readonly Error AlreadyExistingUser = new("Error.AlreadyExistingUser", "Sorry, the user is already created!");
    public static readonly Error UserNotFound = new("Error.UserNotFound", "User not found!");
    public static readonly Error InvalidCredentials = new("Error.InvalidCredentials", "Invalid Credentials");

    public static readonly Error AccountLoggingError = new("Error.AccountLoggingError",
        "Error occured while logging in account, please contact administrator!");

    public static implicit operator Result(Error error)
    {
        return Result.Failure(error);
    }

    public Result ToResult()
    {
        return Result.Failure(this);
    }
}