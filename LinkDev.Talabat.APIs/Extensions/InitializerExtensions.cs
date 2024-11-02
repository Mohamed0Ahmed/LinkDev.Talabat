using LinkDev.Talabat.Core.Domain.Contracts.Persistence;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task<WebApplication> InitializeDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;

            // Retrieve all services for migration and seeding
            var dataSeeders = services.GetServices<IDataSeeder>();            // Get all IDataSeeder 
            var migrationServices = services.GetServices<IMigrationService>(); // Get all IMigrationService 
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
            
                logger.LogInformation("Starting database migrations...");
                foreach (var migrationService in migrationServices)
                {
                    await migrationService.UpdateDatabaseAsync(); 
                }
                logger.LogInformation("Database migrations completed successfully");

         
                logger.LogInformation("Starting data seeding...");
                foreach (var seeder in dataSeeders)
                {
                    await seeder.SeedAsync(); 
                }
                logger.LogInformation("Data seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database migrations or data seeding");
                throw; 
            }

            return app;
        }
    }
}
