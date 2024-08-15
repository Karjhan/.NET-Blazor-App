using Infrastructure.Abstractions.CQRS;
using Infrastructure.Primitives;

namespace Application.Accounts.Commands.LoginAccount;

public class LoginAccountCommandHandler : ICommandHandler<LoginAccountCommand>
{
    public Task<Result> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}