namespace Application.Accounts.DTOs;

public sealed record AccountClaims
(
    string Fullname = null!,
    string Username = null!,
    string Email = null!,
    string Role = null!
);