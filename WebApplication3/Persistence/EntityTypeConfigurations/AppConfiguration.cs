using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Persistence.EntityTypeConfigurations
{
    public class AppConfiguration :IEntityTypeConfiguration<App>
    {
        public void Configure(EntityTypeBuilder<App> builder)
        {
            builder.HasIndex(x => x.Id);

            builder.HasMany(x => x.Screenshots)
                .WithOne(x => x.App)
                .HasForeignKey(x => x.AppId)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
