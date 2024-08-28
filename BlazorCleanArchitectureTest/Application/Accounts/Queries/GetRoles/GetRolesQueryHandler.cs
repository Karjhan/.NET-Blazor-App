using Application.Accounts.DTOs;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Accounts.Queries.GetRoles;

public class GetRolesQueryHandler(
    RoleManager<IdentityRole> roleManager,
    ILogger<GetRolesQueryHandler> logger
) : IQueryHandler<GetRolesQuery, IEnumerable<GetRoleResponse>>
{
    public async Task<Result<IEnumerable<GetRoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving roles from RoleManager");
        var result = (await roleManager.Roles.ToListAsync())
            .Select(identityRole => new GetRoleResponse(identityRole.Id, identityRole.Name));
        
        logger.LogInformation("Successfully retrieved roles and mapped to response");
        return result.ToList();
    }
}