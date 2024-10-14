using LinkDev.Talabat.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkDev.Talabat.Application.Abstraction.Interfaces;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors
{
    internal class CustomSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public CustomSaveChangesInterceptor(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }


        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            UpdateEntities(eventData.Context);
            return base.SavedChanges(eventData, result);
        }


        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context); 
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }



        private void UpdateEntities (DbContext? dbContext)
        {
            if (dbContext == null)
                return;


            foreach (var entity in dbContext.ChangeTracker.Entries<BaseAuditableEntity<int>>().Where(e => e.State is EntityState.Added or EntityState.Modified))
            {
                if (entity.State is EntityState.Added)
                {
                    entity.Entity.CreatedBy = _loggedInUserService.UserId!;
                    entity.Entity.CreatedOn = DateTime.UtcNow;
                }
                entity.Entity.LastModifiedBy = _loggedInUserService.UserId!;
                entity.Entity.LastModifiedOn = DateTime.UtcNow;

            }
        }



    }
}
