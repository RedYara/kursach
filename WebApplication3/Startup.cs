using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data.Initializers;
using WebApplication3.Models;

namespace WebApplication3
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });

            app.UseAuthorization();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=App}/{action=Index}/{id?}");

            });
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Account/Login";
                    await next();
                }
            });

            app.UseDeveloperExceptionPage();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                DbIdentityInitializer.Initialize(serviceScope);
            }
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<_dbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<UserManager<User>>();
            services.AddDbContext<_dbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=kursach;Username=postgres;Password=admin; IncludeErrorDetail=true; CommandTimeout = 100"));
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddSession();
        }

    }
}
