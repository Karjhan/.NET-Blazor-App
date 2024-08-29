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

    public const string BackendApiClientName = "WebUIClient";
    public const string AuthenticationType = "JwtAuth";
}