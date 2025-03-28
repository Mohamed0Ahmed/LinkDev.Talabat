using LinkDev.Talabat.Core.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LinkDev.Talabat.Core.Application.Extensions
{
    static class UserManagerExtensions
    {
        public static async Task<ApplicationUser?> FindUserWithAddress(this UserManager<ApplicationUser> userManager, ClaimsPrincipal claims)
        {
            var email = claims.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Where(U => U.Email == email).Include(user => user.Address).FirstOrDefaultAsync();

            return user;
        }
    }
}
