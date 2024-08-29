namespace Application.Responses.Accounts;

public sealed record GetAccountWithRoleResponse
(
    string? Name,
    string? Email,
    string? RoleName,
    string? RoleId
);