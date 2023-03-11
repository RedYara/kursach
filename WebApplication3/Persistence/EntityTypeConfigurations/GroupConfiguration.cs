using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Persistence.EntityTypeConfigurations
{
    class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasIndex(group => group.Id);

            builder.HasMany(group => group.Apps)
                    .WithOne(apps => apps.Group)
                    .HasForeignKey(a => a.GroupId)
                    .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(group => group.GroupTypes)
                .WithOne(type => type.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
