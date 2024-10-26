using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identities.DataSeeding.Services
{
    public class UserDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDataSeeder(StoreIdentityDbContext dbContext , UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            if (!_userManager.Users.Any())
            {

                var user = new ApplicationUser
                {
                    DisplayName = "Mohamed Ahmed",
                    UserName = "Mohamed.Ahmed",
                    Email = "mohamed@gmail.com",
                    PhoneNumber = "01122334455",
                };

                await _userManager.CreateAsync(user, "P@ssw0rd"); 
            }

        }


    }
}

