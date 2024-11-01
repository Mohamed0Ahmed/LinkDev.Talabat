using LinkDev.Talabat.Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Dashboard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }



        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }



        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            var roleExist = await _roleManager.RoleExistsAsync(model.Name);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Name", "Roles already exist");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
                await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));

        }



        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var mappedRole = new RoleViewModel
            {
                Name = role!.Name!
            };

            return View(mappedRole);

        }


        [HttpPost]
        public async Task<IActionResult> Edit( RoleViewModel model)
        {
            var roleExist = await _roleManager.RoleExistsAsync(model.Name);
            if (!roleExist)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                role!.Name = model.Name;
                await _roleManager.UpdateAsync(role);
                RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Name", "Roles already exist");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
