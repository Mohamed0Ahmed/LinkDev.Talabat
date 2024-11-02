using LinkDev.Talabat.Core.Domain.Entities.Identities;
using LinkDev.Talabat.Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Dashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(U => new UserViewModel
            {
                Id = U.Id,
                DisplayName = U.DisplayName,
                UserName = U.UserName!,
                PhoneNumber = U.PhoneNumber!,
                Email = U.Email!,
            }).ToListAsync();

            foreach (var user in users)
            {
             
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user!.Id,
                UserName = user.UserName!,
                Roles = roles.Select(
                    async r => new RoleViewModel
                    {
                        Id = r.Id,
                        Name = r.Name!,
                        IsSelected = await _userManager.IsInRoleAsync(user, r.Name!)
                    }).Select(t => t.Result).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user!);
            var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();

            var rolesToRemove = userRoles.Except(selectedRoles).ToList();
            foreach (var role in rolesToRemove)
            {
                await _userManager.RemoveFromRoleAsync(user!, role);
            }

            var rolesToAdd = selectedRoles.Except(userRoles).ToList();
            foreach (var role in rolesToAdd)
            {
                await _userManager.AddToRoleAsync(user!, role);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
