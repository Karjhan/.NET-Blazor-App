using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Responses.Accounts;
using Infrastructure.Constants;

namespace Application.Utilities;

public static class IdentityUtilities
{
    public static ClaimsPrincipal SetClaimPrincipal(AccountClaims claims)
    {
        if (claims.Email is null)
        {
            return new ClaimsPrincipal();
        }

        return new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.Name, claims.Username),
                new(ClaimTypes.Email, claims.Email),
                new(ClaimTypes.Role, claims.Role),
                new("Fullname", claims.Fullname),
            ],
            ApplicationConstants.AuthenticationType
        ));
    }

    public static AccountClaims DecryptToken(string jwtToken)
    {
        try
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                return new AccountClaims();
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var name = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)!.Value;
            var email = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
            var role = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;
            var fullname = token.Claims.FirstOrDefault(claim => claim.Type == "Fullname")!.Value;

            return new AccountClaims(fullname, name, email, role);
        }
        catch (Exception e)
        {
            return null!;
        }
    }
}