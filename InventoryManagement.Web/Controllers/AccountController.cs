using System;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using InventoryManagement.Data;
using static System.String;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger, IUnitOfWork db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _db = db;
        }

        //GET: Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //POST: Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

           
            //access to login
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                //check user is locked
                var isValid = _db.Registrations.GetValidation(model.UserName);
                if (!isValid)
                {
                    ModelState.AddModelError(Empty, "Your Account Is Locked");
                    await _signInManager.SignOutAsync();
                    return View(model);
                }

                var type = _db.Registrations.UserTypeByUserName(model.UserName);

                return type switch
                {
                    UserType.Admin => LocalRedirect(returnUrl ??= Url.Content($"/Dashboard/Index")),
                    UserType.SubAdmin => LocalRedirect(returnUrl ??= Url.Content($"/Dashboard/Index")),
                    UserType.SalesPerson => LocalRedirect(returnUrl ??= Url.Content($"/Salesman/Dashboard")),
                    _ => LocalRedirect(returnUrl ??= Url.Content($"/Account/Login"))
                };
            }

            if (result.RequiresTwoFactor)
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, model.RememberMe });

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(Empty, "User account locked out.");
                await _signInManager.SignOutAsync();
                return View(model);
            }

            ModelState.AddModelError(Empty, "Invalid login attempt.");
            return View(model);
        }


        // GET: ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            _db.Registrations.PasswordChanged(user.UserName, model.NewPassword);
            _logger.LogInformation("User changed their password successfully.");

            return RedirectToAction("ChangePassword", "Account", new { Message = "Your password has been changed." });
        }

        //POST: logout
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            if (returnUrl != null) return LocalRedirect(returnUrl);

            return RedirectToAction("Login", "Account");
        }

        //access denied user
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
