using Application.Accounts.Commands.CreateRole;

namespace API.Accounts.DTOs;

public class CreateRoleRequest
{
    public string? Name { get; set; }
    
    public CreateRoleCommand ToCreateRoleCommand()
    {
        CreateRoleCommand createRoleCommand = new CreateRoleCommand(
            Name
        );

        return createRoleCommand;
    }
}