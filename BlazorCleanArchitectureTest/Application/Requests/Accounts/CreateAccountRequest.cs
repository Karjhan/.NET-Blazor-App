using System.ComponentModel.DataAnnotations;
using Application.Accounts.Commands.CreateAccount;

namespace Application.Requests.Accounts;

public class CreateAccountRequest : LoginAccountRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
    
    public CreateAccountCommand ToCreateAccountCommand()
    {
        CreateAccountCommand createAccountCommand = new CreateAccountCommand(
            EmailAddress, 
            Password,
            Name,
            Role
        );

        return createAccountCommand;
    }
}