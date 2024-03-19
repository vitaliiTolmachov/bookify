namespace Bookify.Domain.Apartments;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money first, Money second)
    {
        if (first.Currency.Code != second.Currency.Code)
            throw new InvalidOperationException("Currencies have to be the same");

        return new Money(first.Amount + second.Amount, first.Currency);
    }
}