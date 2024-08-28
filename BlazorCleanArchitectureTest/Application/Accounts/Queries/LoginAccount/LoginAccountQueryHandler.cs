using Application.Accounts.DTOs;
using Application.Utilities;
using Domain.Exceptions;
using Domain.Models.Authentication;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.Configurations;
using Infrastructure.DataContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Accounts.Queries.LoginAccount;

public class LoginAccountQueryHandler(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    ILogger<LoginAccountQueryHandler> logger,
    IOptions<JWTConfiguration> jwtConfiguration,
    ApplicationContext applicationContext
) : IQueryHandler<LoginAccountQuery, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginAccountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.EmailAddress);
            if (user is null)
            {
                logger.LogWarning("User not found for email: {EmailAddress}", request.EmailAddress);
                return Result.Failure<LoginResponse>(Error.UserNotFound);
            }

            SignInResult result;
            try
            {
                logger.LogInformation("Attempting to sign in user {UserId}", user.Id);
                result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while checking the password for user {UserId}", user.Id);
                return Result.Failure<LoginResponse>(Error.InvalidCredentials);
            }

            if (!result.Succeeded)
            {
                logger.LogWarning("Invalid credentials provided for user {UserId}", user.Id);
                return Result.Failure<LoginResponse>(Error.InvalidCredentials);
            }

            logger.LogInformation("Generating tokens for user {UserId}", user.Id);
            string jwtToken = await AccountUtilities.GenerateToken(user, userManager, jwtConfiguration.Value);
            string refreshToken = AccountUtilities.GenerateRefreshToken();
            
            var isTokenOrRefreshTokenNull = string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken);
            if (isTokenOrRefreshTokenNull)
            {
                logger.LogError("Failed to generate JWT or refresh token for user {UserId}", user.Id);
                return Result.Failure<LoginResponse>(Error.AccountLoggingError);
            }

            logger.LogInformation("Successfully generated tokens for user {UserId}", user.Id);
            var saveRefreshTokenResult = await AccountUtilities.SaveRefreshToken(user.Id, refreshToken, applicationContext, logger);
            if (saveRefreshTokenResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(new Error("Error.SaveRefreshToken", "Couldn't save refresh token!"));
            }
            
            return new LoginResponse(jwtToken, refreshToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred during the login process for {EmailAddress}", request.EmailAddress);
            return Result.Failure<LoginResponse>(new Error("Error.AccountLoggingError", e.Message));
        }
    }
}