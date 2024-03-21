using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = Bookify.Application.Exceptions.ValidationException;

namespace Bookify.Application.Behaviours;

public class ValidatingCommandBehaviour<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
    where TCommand : IBaseCommand
{
    private readonly IEnumerable<AbstractValidator<TCommand>> _validators;

    public ValidatingCommandBehaviour(IEnumerable<AbstractValidator<TCommand>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var errors = _validators.Select(x => x.Validate(request))
            .Where(x => x.IsValid == false)
            .SelectMany(x => x.Errors)
            .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
            .ToArray();

        if (errors.Length == 0)
            return await next();

        throw new ValidationException(errors);
    }
}