using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PopularGameEngines.Models;
using Microsoft.AspNetCore.Authorization;

namespace PopularGameEngines.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userMngr, RoleManager<IdentityRole> roleMngr)
        {
            _userManager = userMngr;
            _roleManager = roleMngr;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> users = new();

            foreach (AppUser user in _userManager.Users.ToList())
            {
                user.RoleNames = await _userManager.GetRolesAsync(user);
                users.Add(user);
            }

            UserVM model = new()
            {
                Users = users,
                Roles = _roleManager.Roles
            };

            return View(model);
        }

        public IActionResult Add() => View("../account/register");

        [HttpPost]
        public async Task<IActionResult> Add(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Username };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) return RedirectToAction("Index");
                else foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }

            return View("../account/register");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    string errorMessage = "";

                    foreach (IdentityError error in result.Errors) errorMessage += error.Description + " | ";

                    TempData["message"] = errorMessage;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToAdmin(string id)
        {
            IdentityRole adminRole = await _roleManager.FindByNameAsync("Admin");

            if (adminRole == null) TempData["message"] = "Admin role does not exist. " + "Click 'Create Admin Role' button to create it.";
            else
            {
                AppUser user = await _userManager.FindByIdAsync(id);

                await _userManager.AddToRoleAsync(user, adminRole.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromAdmin(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");

            if (result.Succeeded) { }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminRole()
        {
            var result = await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (result.Succeeded) { }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded) { }

            return RedirectToAction("Index");
        }

    }
}
