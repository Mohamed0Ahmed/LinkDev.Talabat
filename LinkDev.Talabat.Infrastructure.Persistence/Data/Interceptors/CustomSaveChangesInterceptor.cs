using LinkDev.Talabat.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors
{
    public class CustomSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public CustomSaveChangesInterceptor(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }


     


        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context); 
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }



        private void UpdateEntities (DbContext? dbContext)
        {

            if (dbContext is null) return;

            var utcNow = DateTime.UtcNow;
            foreach (var entry in dbContext.ChangeTracker.Entries<BaseAuditableEntity<int>>())
            {
                if (entry is { State: EntityState.Added or EntityState.Modified })
                {
                    if (entry.State == EntityState.Added)
                    {

                        entry.Entity.CreatedBy = string.IsNullOrWhiteSpace(_loggedInUserService.UserId)
                           ? "System"
                           : _loggedInUserService.UserId;

                        entry.Entity.CreatedOn = utcNow;
                    }

                    entry.Entity.LastModifiedBy = string.IsNullOrWhiteSpace(_loggedInUserService.UserId)
                       ? "System"
                       : _loggedInUserService.UserId;
                    entry.Entity.LastModifiedOn = utcNow;
                }
            }
        }



    }
}
