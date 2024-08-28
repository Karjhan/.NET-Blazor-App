using Application.Accounts.DTOs;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Queries.GetAccountsWithRoles;

public sealed record GetAccountsWithRolesQuery
(
    
) : IQuery<IEnumerable<GetAccountWithRoleResponse>>;