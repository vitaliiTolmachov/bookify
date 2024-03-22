using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Db.Configurations;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Duration);

        builder.OwnsOne(x => x.PriceForPeriod, buildAction =>
        {
            buildAction.Property(x => x.Currency)
                .HasConversion(x => x.Code,
                    dbCurrencyCode => Currency.FromCurrencyCode(dbCurrencyCode));
        });
        
        builder.OwnsOne(x => x.CleaningFee, buildAction =>
        {
            buildAction.Property(x => x.Currency)
                .HasConversion(x => x.Code,
                    dbCurrencyCode => Currency.FromCurrencyCode(dbCurrencyCode));
        });
        
        builder.OwnsOne(x => x.AmenitiesUpCharge, buildAction =>
        {
            buildAction.Property(x => x.Currency)
                .HasConversion(x => x.Code,
                    dbCurrencyCode => Currency.FromCurrencyCode(dbCurrencyCode));
        });
        
        builder.OwnsOne(x => x.TotalPrice, buildAction =>
        {
            buildAction.Property(x => x.Currency)
                .HasConversion(x => x.Code,
                    dbCurrencyCode => Currency.FromCurrencyCode(dbCurrencyCode));
        });
        
        builder.HasOne<Apartment>()
            .WithMany()
            .HasForeignKey(x => x.ApartmentId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

    }
}