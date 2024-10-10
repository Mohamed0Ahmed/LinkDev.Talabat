using LinkDev.Talabat.Core.Domain.Abstractions;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Data.DataSeeding.Services;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Migrations.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddDbContext<StoreContext>((options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("StoreContext"));
            });

            services.AddScoped<IDataSeeder, DataSeeder>();
            services.AddScoped<IMigrationService, MigrationService>();
            services.AddScoped<ISaveChangesInterceptor , CustomSaveChangesInterceptor>();
            return services;
        }
    }
}
