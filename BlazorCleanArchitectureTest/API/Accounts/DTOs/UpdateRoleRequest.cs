namespace API.Accounts.DTOs;

public class UpdateRoleRequest
{
    public string UserEmail { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;
}