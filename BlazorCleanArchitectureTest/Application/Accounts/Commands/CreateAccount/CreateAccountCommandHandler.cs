using Infrastructure.Abstractions.CQRS;
using Infrastructure.Primitives;

namespace Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
{
    public Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}