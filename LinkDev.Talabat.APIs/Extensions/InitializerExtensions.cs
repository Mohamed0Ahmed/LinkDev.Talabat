using LinkDev.Talabat.Core.Domain.Abstractions;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task<WebApplication> InitializeStoreContext(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;

            var dataSeeder = services.GetRequiredService<IDataSeeder>();
            var databaseUpdate = services.GetRequiredService<IMigrationService>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();


            try
            {
                await databaseUpdate.UpdateDatabaseAsync(); // Update Database
                await dataSeeder.SeedAsync();               // Seeding Data

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error has been occurred during apply Migrations Or Seeding Data.");

            }

            return app;
        }
    }
}
