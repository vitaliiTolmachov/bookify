using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Db.Configurations;

//TODO: Extract entities in another class and use mapper to map entity into domain obj
internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.ToTable("apartment");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasConversion(x => x.Value,
                dbValue => new Name(dbValue));
        
        builder.Property(x => x.Description)
            .HasConversion(x => x.Value,
                dbValue => new Description(dbValue));
        
        builder.OwnsOne(x => x.Address);

        builder.OwnsOne(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(x => x.Currency)
                .HasConversion(x => x.Code,
                    dbCurrencyCode => Currency.FromCurrencyCode(dbCurrencyCode));
        });
        
        builder.OwnsOne(x => x.CleaningFee, priceBuilder =>
        {
            priceBuilder.Property(x => x.Currency)
                .HasConversion(x => x.Code,
                    dbCurrencyCode => Currency.FromCurrencyCode(dbCurrencyCode));
        });
    }
}