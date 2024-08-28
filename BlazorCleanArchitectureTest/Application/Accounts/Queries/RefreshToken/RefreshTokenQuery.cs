using Application.Accounts.DTOs;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Queries.RefreshToken;

public sealed record RefreshTokenQuery
(
    string? Token
) : IQuery<LoginResponse>;