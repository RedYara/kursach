using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication3.Intrerfaces;
using WebApplication3.Models;
using WebApplication3.Persistence.EntityTypeConfigurations;
using WebApplication3.Persistence.Models;

public class _dbContext : IdentityDbContext<User>, IWebDbContext
{
    public _dbContext(DbContextOptions<_dbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new AppConfiguration());
    }
    public DbSet<App> Apps { get; set; }
    public DbSet<Screenshots> Screenshots { get; set; }
    public DbSet<Group> Group { get; set; }
    public DbSet<GroupType> GroupType { get; set; }
}