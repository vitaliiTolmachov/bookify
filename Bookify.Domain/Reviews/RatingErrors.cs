using Bookify.Domain.Shared;

namespace Bookify.Domain.Reviews;

public static class RatingErrors
{
    public static readonly Error Invalid = new(
        $"{nameof(Rating)}.{nameof(Invalid)}",
        "The rating is invalid");
}