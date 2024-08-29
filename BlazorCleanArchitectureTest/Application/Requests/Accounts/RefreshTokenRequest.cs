using Application.Accounts.Queries.RefreshToken;

namespace Application.Requests.Accounts;

public class RefreshTokenRequest
{
    public string? Token { get; set; }
    
    public RefreshTokenQuery ToRefreshTokenQuery()
    {
        RefreshTokenQuery refreshTokenQuery = new RefreshTokenQuery(
            Token
        );

        return refreshTokenQuery;
    }
}