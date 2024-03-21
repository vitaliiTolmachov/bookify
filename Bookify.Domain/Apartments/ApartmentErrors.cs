using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartments;

public static class ApartmentErrors
{
    public static Error NotFound(Guid id) => new(
        $"{nameof(Apartment)}.{nameof(NotFound)}",
        $"The apartment with the id: {id} was not found");
}