using LinkDev.Talabat.Core.Domain.Entities.Identities;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Dashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            // Add services to the container.
            builder.Services.AddControllersWithViews();



            builder.Services.AddDbContext<StoreContext>((options) =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("StoreContext"));
            });


            builder.Services.AddDbContext<StoreIdentityDbContext>((options) =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("IdentityContext"));
            });



            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
            {

                identityOptions.User.RequireUniqueEmail = true;

                //identityOptions.SignIn.RequireConfirmedAccount = true;
                //identityOptions.SignIn.RequireConfirmedEmail = true;
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = true;


                identityOptions.Password.RequireNonAlphanumeric = true;
                identityOptions.Password.RequiredUniqueChars = 2;
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = true;
                identityOptions.Password.RequireUppercase = true;

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 10;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);


            })
             .AddEntityFrameworkStores<StoreIdentityDbContext>();

            #endregion



            var app = builder.Build();


            #region Configure Middlewares

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            #endregion



            app.Run();
        }
    }
}
