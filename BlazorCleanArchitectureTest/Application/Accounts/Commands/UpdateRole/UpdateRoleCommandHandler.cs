using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.UpdateRole;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    public Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}