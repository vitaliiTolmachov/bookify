using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Db.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
        
        builder.HasIndex(x => x.IdentityId)
            .IsUnique();

        builder.Property(x => x.FirstName)
            .HasMaxLength(200)
            .HasConversion(x => x.Value,
                dbValue => new FirstName(dbValue));
        
        builder.Property(x => x.LastName)
            .HasMaxLength(200)
            .HasConversion(x => x.Value,
                dbValue => new LastName(dbValue));

        builder.Property(x => x.Email)
            .HasMaxLength(400)
            .HasConversion(x => x.Value.ToLower(),
            dbValue => new Domain.Users.Email(dbValue.ToLower()));
    }
}