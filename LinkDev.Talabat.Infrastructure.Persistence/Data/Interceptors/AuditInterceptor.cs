using LinkDev.Talabat.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors
{
    public class AuditInterceptor(IServiceProvider serviceProvider) : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
            var loggedInUserService = scope.ServiceProvider.GetRequiredService<ILoggedInUserService>();

            UpdateEntities(eventData.Context, loggedInUserService);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? dbContext, ILoggedInUserService loggedInUserService)
        {
            if (dbContext is null) return;

            var utcNow = DateTime.UtcNow;
            var entries = dbContext.ChangeTracker.Entries<IBaseAuditableEntity>();

            Console.WriteLine($"[AuditInterceptor] Found {entries.Count()} auditable entries.");
            Console.WriteLine($"[AuditInterceptor] Logged-in User: {loggedInUserService.UserId}");

            foreach (var entry in entries)
            {
                if (entry is { State: EntityState.Added or EntityState.Modified })
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = loggedInUserService.UserId;
                        entry.Entity.CreatedOn = utcNow;
                    }

                    entry.Entity.LastModifiedBy = loggedInUserService.UserId;
                    entry.Entity.LastModifiedOn = utcNow;
                }
            }
        }
    }

}
