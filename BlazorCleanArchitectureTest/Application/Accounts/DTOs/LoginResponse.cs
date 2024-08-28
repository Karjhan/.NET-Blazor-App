namespace Application.Accounts.DTOs;

public record LoginResponse(
    string Token,
    string RefreshToken
);