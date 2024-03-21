using Bookify.Domain.Shared;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand
{
    
}

public interface ICommand<TResult> : IRequest<Result<TResult>>
{
    
}

public interface IBaseCommand
{
    
}