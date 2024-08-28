using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.UpdateRole;

public class UpdateRoleCommand
(
    string UserEmail,
    string RoleName
) : ICommand;