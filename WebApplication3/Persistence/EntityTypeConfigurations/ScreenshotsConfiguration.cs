using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Persistence.EntityTypeConfigurations
{
    public class ScreenshotsConfiguration : IEntityTypeConfiguration<Screenshots>
    {
        public void Configure(EntityTypeBuilder<Screenshots> builder)
        {
            builder.HasIndex(x => x.Id);

            builder.HasOne(x => x.App)
                .WithMany(x => x.Screenshots)
                .HasForeignKey(x => x.AppId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
