using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PopularGameEngines.Models;

namespace PopularGameEngines.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userMngr, SignInManager<AppUser> signInMgr)
        {
            userManager = userMngr;
            signInManager = signInMgr;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Username, Name = model.Name };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) return RedirectToAction("Index", "Home");
                else foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult LogIn(string returnURL = "")
        {
            var model = new LoginVM { ReturnUrl = returnURL };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded) return (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)) ? Redirect(model.ReturnUrl) : RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid USERNAME / PASSWORD");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public ViewResult AccessDenied() => View();
    }
}
