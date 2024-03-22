using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Db.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rating)
            .HasConversion(x => x.Value,
                dbValue => Rating.Create(dbValue).Value);

        builder.Property(x => x.Comment)
            .HasMaxLength(200)
            .HasConversion(x => x.Value,
                dbValue => new Comment(dbValue));

        builder.HasOne<Apartment>()
            .WithMany()
            .HasForeignKey(x => x.ApartmentId);

        builder.HasOne<Booking>()
            .WithMany()
            .HasForeignKey(x => x.BookingId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}