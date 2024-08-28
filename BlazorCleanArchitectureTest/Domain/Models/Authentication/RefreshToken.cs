using Domain.Primitives;

namespace Domain.Models.Authentication;

public class RefreshToken(Guid id) : Entity(id)
{
    public string? UserId { get; private set; }

    public string? Token { get; private set; }

    public RefreshToken(Guid id, string? userId, string? token) : this(id)
    {
        UserId = userId;
        Token = token;
    }

    public static RefreshToken Create(
        Guid id,
        string userId,
        string token
    )
    {
        return new RefreshToken(id, userId, token);
    }

    public void UpdateToken(string newToken)
    {
        Token = newToken;
    }
}