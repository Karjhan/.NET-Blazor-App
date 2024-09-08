using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
            var fullname = token.Claims.FirstOrDefault(claim => claim.Type == "FullName")!.Value;

            return new AccountClaims(fullname, name, email, role);
        }
        catch (Exception e)
        {
            return null!;
        }
    }
    
    public static string GenerateCodeVerifier()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        
        return Convert.ToBase64String(randomBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
            
        return Convert.ToBase64String(challengeBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}