using Infrastructure.Abstractions.CQRS;
using Infrastructure.Primitives;

namespace Application.Accounts.Commands.CreateRole;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
{
    public Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}