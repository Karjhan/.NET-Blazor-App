using Infrastructure.Primitives;
using MediatR;

namespace Infrastructure.Abstractions.CQRS;

public interface ICommand : IRequest<Result>
{
    
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
    
}