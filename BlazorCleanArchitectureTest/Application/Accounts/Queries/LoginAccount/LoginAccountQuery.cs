using Application.Responses.Accounts;
using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Queries.LoginAccount;

public sealed record LoginAccountQuery
(
    string EmailAddress,
    string Password
) : IQuery<LoginResponse>;