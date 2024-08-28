using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    string EmailAddress,
    string Password,
    string Name,
    string Role
) : ICommand;