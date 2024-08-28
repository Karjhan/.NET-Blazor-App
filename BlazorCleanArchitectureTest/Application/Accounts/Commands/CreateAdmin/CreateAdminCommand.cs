using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.CreateAdmin;

public sealed record CreateAdminCommand(

) : ICommand;