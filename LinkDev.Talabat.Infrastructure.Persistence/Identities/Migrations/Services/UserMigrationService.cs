using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identities.Migrations.Services
{
    public class UserMigrationService : IMigrationService
    {
        private readonly StoreIdentityDbContext _dbContext;

        public UserMigrationService(StoreIdentityDbContext dbContext)
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
