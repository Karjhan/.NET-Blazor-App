﻿using Infrastructure.Abstractions.CQRS;

namespace Application.Accounts.Commands.CreateRole;

public sealed record CreateRoleCommand
(
    
) : ICommand;