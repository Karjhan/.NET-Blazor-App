using Application.Utilities;
using Domain.Exceptions;
using Domain.Models.Authentication;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Accounts.Commands.UpdateRole;

public class UpdateRoleCommandHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ILogger<UpdateRoleCommandHandler> logger
) : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await roleManager.FindByNameAsync(request.RoleName) is null)
        {
            logger.LogWarning("Role not found: {RoleName}", request.RoleName);
            return Result.Failure(Error.RoleNotFound);
        }
        var user = await userManager.FindByEmailAsync(request.UserEmail); 
        if (user is null)
        {
            logger.LogWarning("User not found for email: {UserEmail}", request.UserEmail);
            return Result.Failure(Error.UserNotFound);
        }

        logger.LogInformation("User {UserId} found. Current role processing...", user.Id);
        var previousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        var removeOldRole = await userManager.RemoveFromRoleAsync(user, previousRole);
        var errors = AccountUtilities.CheckIdentityResult(removeOldRole);
        if (!string.IsNullOrEmpty(errors))
        {
            logger.LogError("Failed to remove user {UserId} from role {PreviousRole}. Errors: {Errors}", user.Id, previousRole, errors);
            return Result.Failure(new Error("Error.RemoveRole", errors));
        }

        logger.LogInformation("Adding user {UserId} to new role {NewRole}", user.Id, request.RoleName);
        var result = await userManager.AddToRoleAsync(user, request.RoleName);
        var addRoleErrors = AccountUtilities.CheckIdentityResult(result);
        if (!string.IsNullOrEmpty(addRoleErrors))
        {
            logger.LogError("Failed to add user {UserId} to role {NewRole}. Errors: {Errors}", user.Id, request.RoleName, addRoleErrors);
            return Result.Failure(new Error("Error.AddToRole", addRoleErrors));
        }

        logger.LogInformation("User {UserId} successfully updated to role {NewRole}", user.Id, request.RoleName);
        return Result.Success();
    }
}