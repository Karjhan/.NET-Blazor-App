using Application.Accounts.Commands.UpdateRole;

namespace Application.Requests.Accounts;

public class UpdateRoleRequest
{
    public string UserEmail { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;
    
    public UpdateRoleCommand ToUpdateRoleCommand()
    {
        UpdateRoleCommand updateRoleCommand = new UpdateRoleCommand(
            UserEmail,
            RoleName
        );

        return updateRoleCommand;
    }
}