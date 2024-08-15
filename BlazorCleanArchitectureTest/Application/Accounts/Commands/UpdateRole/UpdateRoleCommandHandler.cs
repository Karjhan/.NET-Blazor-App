using Infrastructure.Abstractions.CQRS;
using Infrastructure.Primitives;

namespace Application.Accounts.Commands.UpdateRole;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    public Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}