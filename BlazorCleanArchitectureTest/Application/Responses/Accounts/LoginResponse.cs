namespace Application.Responses.Accounts;

public record LoginResponse(
    string Token,
    string RefreshToken
);