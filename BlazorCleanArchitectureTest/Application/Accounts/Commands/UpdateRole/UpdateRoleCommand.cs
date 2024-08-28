using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.UpdateRole;

public sealed record UpdateRoleCommand
(
    string UserEmail,
    string RoleName
) : ICommand;