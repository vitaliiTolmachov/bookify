namespace Bookify.Domain.Apartments;

public record Currency
{
    public static readonly Currency Usd = new Currency("USD");
    public static readonly Currency Eur = new Currency("EUR");

    private static IReadOnlyCollection<Currency> AllowedCurrencies => new[]
    {
        Usd, Eur
    };

    public static Currency FromCurrencyCode(string currencyCode)
    {
        return AllowedCurrencies.FirstOrDefault(x => x.Code == currencyCode) ??
               throw new ApplicationException("Invalid currency code");
    }
    
    private Currency(string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentNullException(nameof(code));

        // if (AllowedCurrencies.All(x => x.Code != code))
        //     throw new ApplicationException("Unsupported currency code");
        
        Code = code;
    }

    public string Code { get; init; }
}