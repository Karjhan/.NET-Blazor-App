namespace Application.Responses.Accounts;

public sealed record AccountClaims
(
    string Fullname = null!,
    string Username = null!,
    string Email = null!,
    string Role = null!
);