namespace Bookify.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    public DateTime CurrentTimeUtc { get; }
}