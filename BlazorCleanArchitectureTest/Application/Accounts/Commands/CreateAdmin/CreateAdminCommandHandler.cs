using Application.Accounts.Commands.CreateAccount;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Accounts.Commands.CreateAdmin;

public class CreateAdminCommandHandler (
    RoleManager<IdentityRole> roleManager,
    ILogger<CreateAdminCommandHandler> logger,
    ISender sender
): ICommandHandler<CreateAdminCommand>
{
    public async Task<Result> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await roleManager.FindByNameAsync(RoleConstants.Admin) != null)
            {
                logger.LogInformation("Admin role '{RoleName}' already exists. Skipping admin creation.", RoleConstants.Admin);
                return Result.Success();
            }

            var adminCommand = new CreateAccountCommand(
                Name: AdminConstants.Name,
                EmailAddress: AdminConstants.Email,
                Password: AdminConstants.Password,
                Role: RoleConstants.Admin
            );
            
            logger.LogInformation("Creating admin account with {AdminName} and {AdminEmail}", AdminConstants.Name, AdminConstants.Email);

            var result = await sender.Send(adminCommand);
            if (result.IsSuccess)
            {
                logger.LogInformation("Admin account created successfully for {AdminName} with role {RoleName}", AdminConstants.Name, RoleConstants.Admin);
            }
            else
            {
                logger.LogWarning("Failed to create admin account. Error: {Error}", result.Error?.Description);
            }
            return result;
            
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating admin account: {Error}", e.Message);
            return Result.Failure(new Error("Error.CreateAdmin", e.Message));
        }
    }
}