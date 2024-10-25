using LinkDev.Talabat.Core.Domain.Entities.Identities;
using LinkDev.Talabat.Infrastructure.Persistence.Identities.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identities
{
    internal class StoreIdentityDbContext : IdentityDbContext<ApplicationUser>
    {

        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options)
            :base(options) 
        {
            
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            //builder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly);    // => this will call every class in Assembly so doing this manually is better 

            builder.ApplyConfiguration(new ApplicationUserConfigurations());
            builder.ApplyConfiguration(new AddressConfigurations());
        }


    }
}
