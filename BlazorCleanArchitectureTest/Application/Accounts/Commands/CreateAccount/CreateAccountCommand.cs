using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(

) : ICommand;