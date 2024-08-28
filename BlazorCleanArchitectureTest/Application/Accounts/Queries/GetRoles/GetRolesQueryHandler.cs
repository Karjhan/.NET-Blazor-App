using Application.Accounts.DTOs;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Queries.GetRoles;

public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<GetRoleResponse>>
{
    public Task<Result<IEnumerable<GetRoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}