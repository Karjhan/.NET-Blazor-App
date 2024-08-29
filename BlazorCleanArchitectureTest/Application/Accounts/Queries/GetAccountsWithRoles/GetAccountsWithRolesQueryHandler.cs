using Application.Responses.Accounts;
using Domain.Models.Authentication;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Accounts.Queries.GetAccountsWithRoles;

public class GetAccountsWithRolesQueryHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ILogger<GetAccountsWithRolesQueryHandler> logger
) : IQueryHandler<GetAccountsWithRolesQuery, IEnumerable<GetAccountWithRoleResponse>>
{
    public async Task<Result<IEnumerable<GetAccountWithRoleResponse>>> Handle(GetAccountsWithRolesQuery request, CancellationToken cancellationToken)
    {
        var allUsers = await userManager.Users.ToListAsync();
        var result = new List<GetAccountWithRoleResponse>();
        
        if (allUsers is null)
        {
            return result;
        }

        foreach (var user in allUsers)
        {
            var getUserRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var getRoleInfo = await roleManager.Roles.FirstOrDefaultAsync(role => role.Name.ToLower() == getUserRole.ToLower());
            result.Add(new GetAccountWithRoleResponse(
                user.Name,
                user.Email,
                getRoleInfo.Name,
                getRoleInfo.Id
            ));
        }

        return result;
    }
}