using Microsoft.EntityFrameworkCore;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Intrerfaces
{
    public interface IWebDbContext
    {
        DbSet<App> Apps { get; set; }
    }
}
