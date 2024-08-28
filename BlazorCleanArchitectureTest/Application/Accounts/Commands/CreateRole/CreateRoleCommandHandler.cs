using Application.Utilities;
using Domain.Exceptions;
using Domain.Models.Authentication;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Accounts.Commands.CreateRole;

public class CreateRoleCommandHandler (
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ILogger<CreateRoleCommandHandler> logger
) : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingRole = await roleManager.FindByNameAsync(request.Name);
            if (existingRole is null)
            {
                var response = await roleManager.CreateAsync(new IdentityRole(request.Name));
                var errors = AccountUtilities.CheckIdentityResult(response);
                if (!string.IsNullOrEmpty(errors))
                {
                    return Result.Failure(new Error("Error.CreateRole", errors));
                }
                return Result.Success();
            }
            return Result.Failure(new Error("Error.CreateRole", $"Role {request.Name} already exists"));
        }
        catch (Exception e)
        {
           return Result.Failure(new Error("Error.CreateRole", e.Message)); 
        }
    }
}