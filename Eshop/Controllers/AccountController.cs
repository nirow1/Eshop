using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Eshop.Data.Models;
using Eshop.Models.AccountViewModel;

namespace Eshop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> singInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager)
        {
            this.userManager = userManager;
            this.singInManager = singInManager;
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

        [HttpGet]
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
                        await singInManager.SignInAsync(user, isPersistent: false);
                        return string.IsNullOrWhiteSpace(returnUrl) ? RedirectToAction("Index", "Home") : RedirectToLocal(returnUrl);
                    }
                    AddErrors(result);
                }
                AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Email {model.Email} je již zaregistrován" }));
            }
            return View(model);
        }
        #endregion
    }
}
