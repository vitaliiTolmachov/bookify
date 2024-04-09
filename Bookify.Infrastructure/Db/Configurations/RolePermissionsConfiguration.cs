using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Db.Configurations;

public class RolePermissionsConfiguration : IEntityTypeConfiguration<RolePermissions>
{
    public void Configure(EntityTypeBuilder<RolePermissions> builder)
    {
        builder.ToTable("roles_permissions");
        builder.HasKey(x => new {x.RoleId, x.PermissionId});
        builder.HasData(new RolePermissions()
        {
            RoleId = Role.Registered.Id,
            PermissionId = Permission.UsersRead.Id
        });
    }
}