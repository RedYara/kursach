using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Persistence.EntityTypeConfigurations
{
    public class GroupTypeConfiguration : IEntityTypeConfiguration<GroupType>
    {
        public void Configure(EntityTypeBuilder<GroupType> builder)
        {
            builder.HasIndex(x => x.Id);

            builder.HasOne(x => x.Group)
                .WithMany(x => x.GroupTypes)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Apps)
                .WithOne(x => x.GroupType)
                .HasForeignKey(x => x.GroupTypeId)
                .OnDelete(DeleteBehavior.SetNull);

        }

    }
}
