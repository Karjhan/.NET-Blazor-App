using Domain.Primitives;

namespace Domain.Exceptions;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    public static readonly Error AlreadyExistingUser = new("Error.AlreadyExistingUser", "Sorry, the user is already created!");
    public static readonly Error UserNotFound = new("Error.UserNotFound", "User not found!");
    public static readonly Error InvalidCredentials = new("Error.InvalidCredentials", "Invalid Credentials");
    public static readonly Error RoleNotFound = new("Error.RoleNotFound", "Role not found!");
    public static readonly Error SaveLocalStorageError = new("Error.SaveLocalStorageError", "Failure to save in local storage");
    public static readonly Error DeleteFromLocalStorageError = new("Error.DeleteFromLocalStorageError", "Failure to delete from local storage");
    public static readonly Error AccountLoggingError = new("Error.AccountLoggingError",
        "Error occured while logging in account, please contact administrator!");
    public static readonly Error AccountCreationError = new("Error.AccountCreationError",
        "Error occured while creating the account, please contact administrator!");
    public static readonly Error InternalServerError = new("Error.Internal", "Internal Server Error");
    public static readonly Error VehicleAlreadyExists = new("Error.Vehicle", "Vehicle already exists!");
    public static readonly Error VehicleBrandAlreadyExists = new("Error.VehicleBrand", "Vehicle brand already exists!");
    public static readonly Error VehicleOwnerAlreadyExists = new("Error.VehicleOwner", "Vehicle owner already exists!");
    public static readonly Error VehicleNotFound = new("Error.Vehicle", "Vehicle not found!");
    public static readonly Error VehicleBrandNotFound = new("Error.VehicleBrand", "Vehicle brand not found!");
    public static readonly Error VehicleOwnerNotFound = new("Error.VehicleOwner", "Vehicle owner not found!");

    public static implicit operator Result(Error error)
    {
        return Result.Failure(error);
    }

    public Result ToResult()
    {
        return Result.Failure(this);
    }
}