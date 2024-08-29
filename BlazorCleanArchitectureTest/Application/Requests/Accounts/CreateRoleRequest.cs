using Application.Accounts.Commands.CreateRole;

namespace Application.Requests.Accounts;

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