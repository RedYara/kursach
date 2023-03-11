using Microsoft.AspNetCore;
using WebApplication3;
using WebApplication3.Intrerfaces;

namespace WebApplication3
{

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();


            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
