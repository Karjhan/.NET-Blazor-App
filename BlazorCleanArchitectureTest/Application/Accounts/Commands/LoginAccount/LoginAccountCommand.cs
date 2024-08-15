using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.LoginAccount;

public sealed record LoginAccountCommand
(
    
) : ICommand;