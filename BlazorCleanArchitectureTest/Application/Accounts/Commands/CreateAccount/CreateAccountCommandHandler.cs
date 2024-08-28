using Application.Accounts.Commands.CreateRole;
using Application.Utilities;
using Domain.Exceptions;
using Domain.Models.Authentication;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler(
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ILogger<CreateAccountCommandHandler> logger,
    ISender sender
) : ICommandHandler<CreateAccountCommand>
{
    public async Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await userManager.FindByEmailAsync(request.EmailAddress) is not null)
            {
                logger.LogWarning("Attempt to create an account with an already existing Email: {EmailAddress}", request.EmailAddress);
                return Result.Failure(Error.AlreadyExistingUser);
            }

            var user = new ApplicationUser()
            {
                Name = request.Name,
                UserName = request.EmailAddress,
                Email = request.EmailAddress,
                PasswordHash = request.Password
            };

            logger.LogInformation("Creating user account for {EmailAddress}", request.EmailAddress);
            var result = await userManager.CreateAsync(user, request.Password);
            string error = AccountUtilities.CheckIdentityResult(result);

            if (!string.IsNullOrEmpty(error))
            {
                logger.LogError("Failed to create user account for {EmailAddress}. Error: {Error}", request.EmailAddress, error);
                return Result.Failure(new Error("Error.Identity", error));
            }

            logger.LogInformation("Assigning user {UserId} to role {Role}", user.Id, request.Role);
            return await AccountUtilities.AssignUserToRole(
                user,
                new IdentityRole() { Name = request.Role },
                sender,
                roleManager,
                userManager,
                logger
            );
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while handling CreateAccountCommand for Email: {EmailAddress}", request.EmailAddress);
            return Result.Failure(new Error("Error.CreateAccount", e.Message));
        }
    }
}