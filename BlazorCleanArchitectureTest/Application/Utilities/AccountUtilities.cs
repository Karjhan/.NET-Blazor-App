using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Accounts.Commands.CreateRole;
using Domain.Exceptions;
using Domain.Models.Authentication;
using Domain.Primitives;
using Infrastructure.Configurations;
using Infrastructure.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.Utilities;

public class AccountUtilities
{
    public static async Task<string> GenerateToken(ApplicationUser user, UserManager<ApplicationUser> userManager, JWTConfiguration jwtConfiguration)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault()),
                new Claim("FullName", user.Name)
            };

            var token = new JwtSecurityToken(
                issuer: jwtConfiguration.Issuer,
                audience: jwtConfiguration.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            return null!;
        }
    }
    
    public static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public static string? CheckIdentityResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description);
            return string.Join(Environment.NewLine, errors);
        }

        return null;
    }

    public static async Task<Result> AssignUserToRole(
        ApplicationUser user, 
        IdentityRole role, 
        ISender sender, 
        RoleManager<IdentityRole> roleManager, 
        UserManager<ApplicationUser> userManager,
        ILogger logger)
    {
        var userIsNullOrRoleIsNull = user is null || role is null;
        if (userIsNullOrRoleIsNull)
        {
            logger.LogWarning("AssignUserToRole failed due to null user or role. {@UserIsNull}, {@RoleIsNull}", user is null, role is null);
            return Result.Failure(Error.NullValue);
        }

        if (await roleManager.FindByNameAsync(role.Name) is null)
        {
            logger.LogInformation("Role '{RoleName}' not found. Creating default role.", role.Name);
            await sender.Send(new CreateRoleCommand(role.Name));
        }

        IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
        string error = CheckIdentityResult(result);

        if (!string.IsNullOrEmpty(error))
        {
            logger.LogError("Failed to assign role '{RoleName}' to user '{UserName}'. Error: {Error}", role.Name, user.Name, error);
            return Result.Failure(new Error("Error.Identity", error));
        }

        logger.LogInformation("User '{UserName}' successfully assigned to role '{RoleName}'.", user.Name, role.Name);
        return Result.Success();
    }

    public static async Task<Result> SaveRefreshToken(string userId, string token, ApplicationContext context, ILogger logger)
    {
        try
        {
            var user = await context.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.UserId == userId);
            if (user is null)
            {
                logger.LogInformation("No existing refresh token found for UserId: {UserId}. Creating a new one.", userId);
                context.RefreshTokens.Add(RefreshToken.Create(Guid.NewGuid(), userId, token));
            }
            else
            {
                logger.LogInformation("Existing refresh token found for UserId: {UserId}. Updating the token.", userId);
                user.UpdateToken(token);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Refresh token successfully saved for UserId: {UserId}", userId);
            
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while saving the refresh token for UserId: {UserId}", userId);
            return Result.Failure(new Error("Error.RefreshToken", e.Message));
        }
    }
}