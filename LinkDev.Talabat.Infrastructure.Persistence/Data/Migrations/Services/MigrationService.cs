using LinkDev.Talabat.Core.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Migrations.Services
{
    internal class MigrationService : IMigrationService
    {
        private readonly StoreContext _dbContext;

        public MigrationService(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task UpdateDatabaseAsync()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();    // Update-Database
        }


    }
}
