using Application.Responses.Accounts;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Queries.GetAccountsWithRoles;

public sealed record GetAccountsWithRolesQuery
(
    
) : IQuery<IEnumerable<GetAccountWithRoleResponse>>;