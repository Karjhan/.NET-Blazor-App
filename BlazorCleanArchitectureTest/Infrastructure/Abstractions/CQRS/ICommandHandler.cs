﻿using Domain.Primitives;
using MediatR;

namespace Infrastructure.Abstractions.CQRS;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand,Result> 
    where TCommand : ICommand
{
    
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> 
    where TCommand : ICommand<TResponse>, IRequest<Result<TResponse>>
{
    
}