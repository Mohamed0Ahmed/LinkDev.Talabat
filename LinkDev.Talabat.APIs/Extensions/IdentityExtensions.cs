using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Auth;
using LinkDev.Talabat.Core.Application.Services.Auth;
using LinkDev.Talabat.Core.Domain.Entities.Identities;
using LinkDev.Talabat.Infrastructure.Persistence.Identities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices (this IServiceCollection services , IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));


            services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
            {

                identityOptions.User.RequireUniqueEmail = true;

                identityOptions.SignIn.RequireConfirmedAccount = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;


                //identityOptions.Password.RequireNonAlphanumeric = true;
                //identityOptions.Password.RequiredUniqueChars = 2;
                //identityOptions.Password.RequiredLength = 6;
                //identityOptions.Password.RequireDigit = true;
                //identityOptions.Password.RequireLowercase = true;
                //identityOptions.Password.RequireUppercase = true;

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 10;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);


            })
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));
            services.AddScoped(typeof(Func<IAuthServices>), (serviceProvider) =>
            {
                return () => serviceProvider.GetService<IAuthServices>();
            });

            return services;
        }
    }
}
