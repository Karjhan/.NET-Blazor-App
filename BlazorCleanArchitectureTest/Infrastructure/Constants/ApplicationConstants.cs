namespace Infrastructure.Constants;

public static class ApplicationConstants
{
    public const string ApiAccountBasePath = "api/accounts";
    public const string ApiCreateAccountSubPath = "/identity/create";
    public const string ApiLoginAccountSubPath = "/identity/login";
    public const string ApiCreateRoleSubPath = "/identity/role/create";
    public const string ApiGetRolesSubPath = "/identity/role/list";
    public const string ApiCreateAdminSubPath = "/setting";
    public const string ApiGetAccountsWithRolesSubPath = "/identity/users-with-roles";
    public const string ApiUpdateRoleSubPath = "/identity/change-role";
    public const string ApiRefreshTokenSubPath = "/identity/refresh-token";

    public const string ApiVehicleBasePath = "api/vehicles";
    public const string ApiCreateVehicleSubPath = "/add-vehicle";
    public const string ApiCreateVehicleBrandSubPath = "/add-vehicle-brand";
    public const string ApiCreateVehicleOwnerSubPath = "/add-vehicle-owner";
    public const string ApiGetVehiclesSubPath = "/get-vehicles";
    public const string ApiGetVehicleBrandsSubPath = "/get-vehicle-brands";
    public const string ApiGetVehicleOwnersSubPath = "/get-vehicle-owners";
    public const string ApiUpdateVehicleSubPath = "/update-vehicle";
    public const string ApiUpdateVehicleBrandSubPath = "/update-vehicle-brand";
    public const string ApiUpdateVehicleOwnerSubPath = "/update-vehicle-owner";
    public static string ApiGetSingleVehicleSubPath(string vehicleId) => $"/get-vehicle/{vehicleId}";
    public static string ApiGetSingleVehicleBrandSubPath(string vehicleBrandId) => $"/get-vehicle-brand/{vehicleBrandId}";
    public static string ApiGetSingleVehicleOwnerSubPath(string vehicleOwnerId) => $"/get-vehicle-owner/{vehicleOwnerId}";
    public static string ApiDeleteVehicleSubPath(string vehicleId) => $"/delete-vehicle/{vehicleId}";
    public static string ApiDeleteVehicleBrandSubPath(string vehicleBrandId) => $"/delete-vehicle-brand/{vehicleBrandId}";
    public static string ApiDeleteVehicleOwnerSubPath(string vehicleOwnerId) => $"/delete-vehicle-owner/{vehicleOwnerId}";

    public const string BackendApiClientName = "WebUIClient";
    public const string AuthenticationType = "JwtAuth";
}