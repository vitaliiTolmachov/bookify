using Bookify.Domain.Apartments;

namespace Bookify.Domain.Shared;

public record Money
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Expecting positive amount");
        
        Amount = amount;
        Currency = currency;
    }
    public static Money operator +(Money first, Money second)
    {
        if (first.Currency.Code != second.Currency.Code)
            throw new InvalidOperationException("Currencies have to be the same");

        return new Money(first.Amount + second.Amount, first.Currency);
    }

    public static Money Zero(Currency currency) => new Money(0, currency);

    public bool IsZero()
    {
        return this.Amount == 0;
    }
}