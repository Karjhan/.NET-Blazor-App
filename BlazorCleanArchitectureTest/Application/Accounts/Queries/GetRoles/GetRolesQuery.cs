using Application.Responses.Accounts;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Queries.GetRoles;

public sealed record GetRolesQuery
(
    
) : IQuery<IEnumerable<GetRoleResponse>>;