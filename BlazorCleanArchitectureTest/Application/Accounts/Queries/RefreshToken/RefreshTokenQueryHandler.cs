using Application.Accounts.DTOs;
using Application.Utilities;
using Domain.Exceptions;
using Domain.Models.Authentication;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.Configurations;
using Infrastructure.DataContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Accounts.Queries.RefreshToken;

public class RefreshTokenQueryHandler(
    UserManager<ApplicationUser> userManager,
    ILogger<RefreshTokenQueryHandler> logger,
    IOptions<JWTConfiguration> jwtConfiguration,
    ApplicationContext applicationContext
) : IQueryHandler<RefreshTokenQuery, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var token = await applicationContext.RefreshTokens.FirstOrDefaultAsync(refreshToken =>
            refreshToken.Token == request.Token);
        if (token is null)
        {
            logger.LogWarning("No matching refresh token found for Token: {Token}", request.Token);
            return Result.Failure<LoginResponse>(Error.NullValue);
        }

        var user = await userManager.FindByIdAsync(token.UserId);
        if (user is null)
        {
            logger.LogError("User not found for UserId: {UserId}", token.UserId);
            return Result.Failure<LoginResponse>(Error.UserNotFound);
        }
        
        logger.LogInformation("Generating new JWT and refresh token for UserId: {UserId}", user.Id);
        var newToken = await AccountUtilities.GenerateToken(user, userManager, jwtConfiguration.Value);
        var newRefreshToken = AccountUtilities.GenerateRefreshToken();
        
        logger.LogInformation("Saving new refresh token for UserId: {UserId}", user.Id);
        var saveRefreshTokenResult = await AccountUtilities.SaveRefreshToken(user.Id, newRefreshToken, applicationContext, logger);
        if (saveRefreshTokenResult.IsFailure)
        {
            logger.LogError("Failed to save refresh token for UserId: {UserId}", user.Id);
            return Result.Failure<LoginResponse>(Error.None);
        }

        logger.LogInformation("Successfully handled RefreshTokenQuery for UserId: {UserId}", user.Id);
        return new LoginResponse(newToken, newRefreshToken);
    }
}