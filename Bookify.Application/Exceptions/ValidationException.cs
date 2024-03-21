namespace Bookify.Application.Exceptions;

public sealed class ValidationException(IEnumerable<ValidationError> Errors) : Exception
{
    public IEnumerable<ValidationError> Errors { get; } = Errors;
}