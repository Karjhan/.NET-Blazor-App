namespace Application.Accounts.DTOs;

public sealed record GetAccountWithRoleResponse
(
    string? Name,
    string? Email,
    string? RoleName,
    string? RoleId
);