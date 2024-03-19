using Bookify.Domain.Shared;

namespace Bookify.Domain.Reviews;

public static class ReviewErrors
{
    public static readonly Error NotEligible = new(
        $"{nameof(Review)}.{nameof(NotEligible)}",
        "The review is not eligible because the booking is not yet completed");
}