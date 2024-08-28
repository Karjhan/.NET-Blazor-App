using Domain.Primitives;
using MediatR;

namespace Infrastructure.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}