using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Eshop.Data.Models;
using Eshop.Models.AccountViewModels;

namespace Eshop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager)
        {
            this.userManager = userManager;
            this.signInManager = singInManager;
        }

        #region Helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (await userManager.FindByEmailAsync(model.Email) is null)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return string.IsNullOrWhiteSpace(returnUrl) ? RedirectToAction("Index", "Home") : RedirectToLocal(returnUrl);
                    }
                    AddErrors(result);
                }
                AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Email {model.Email} je již zaregistrován" }));
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            
            if(!ModelState.IsValid)
                return View(model);

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return string.IsNullOrWhiteSpace(returnUrl) ?
                    RedirectToAction("Administration") : RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Neplatn přihlašovací údaje");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User) ??
                throw new ApplicationException($"Nepodařilo se načíst uživatele s ID {userManager.GetUserId(User)}.");

            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User) ??
                throw new ApplicationException($"Nepodarilo se načíst uřivatele s ID: {userManager.GetUserId(User)}");

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Administration");
        }

        [Authorize]
        public IActionResult Administration()
        {
            return View();
        }
        #endregion
    }
}
