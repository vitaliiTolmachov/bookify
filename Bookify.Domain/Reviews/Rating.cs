using Bookify.Domain.Shared;

namespace Bookify.Domain.Reviews;

public sealed record Rating
{
    public int Value { get; init; }

    public static Result<Rating> Create(int value)
    {
        if (value is < 1 or > 5)
            return Result<Rating>.Failure(RatingErrors.Invalid);

        return new Rating(value);
    }

    private Rating(int value) => Value = value;
}