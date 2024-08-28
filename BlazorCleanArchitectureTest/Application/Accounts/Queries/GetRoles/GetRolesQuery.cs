using Application.Accounts.DTOs;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Queries.GetRoles;

public sealed record GetRolesQuery
(
    
) : IQuery<IEnumerable<GetRoleResponse>>;