using KAFO.ASPMVC.Areas.Identity.ViewModels;
using KAFO.Utility.Helpers;
using KAFO.BLL.Managers;
using KAFO.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KAFO.ASPMVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class IdentityController : Controller
    {
        private readonly UserManager _userManager;

        public IdentityController(UserManager userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IdentityViewModel IdentityViewModel, string? ReturnUrl = null)
        {

            if (ModelState.IsValid)
            {
                User user = VerifyUser(IdentityViewModel.userName, IdentityViewModel.password);
                if (user != null)
                {
                    Claim info0 = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
                    Claim info1 = new Claim(ClaimTypes.Name, user.Name.Trim());
                    Claim info2 = new Claim(ClaimTypes.Role, user.Role.ToLower().Trim());
                    ClaimsIdentity card = new ClaimsIdentity([info0, info1, info2], "CustomIdentity");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(card);
                    await HttpContext.SignInAsync("CustomIdentity", claimsPrincipal, new AuthenticationProperties
                    {
                        IsPersistent = true, // makes the cookie persist after browser close
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                        AllowRefresh = true
                       
                    });

                    HttpContext.User = claimsPrincipal;
                    if (user.Role.ToLower() == UserRole.admin.ToString())
                    {
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }
                    else if (user.Role.ToLower() == UserRole.seller.ToString())
                    {
                        return RedirectToAction("Index", "POS", new { area = "Seller" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "user Not Founded");
                    return View(IdentityViewModel);
                }
            }
            else
            {
                return View(IdentityViewModel);
            }

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }

            return Redirect("~/");
        }
        public User VerifyUser(string userInput, string password)
        {
            var user = _userManager.FindByEmailOrPhone(userInput);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (PasswordHelper.VerifyPassword(user.Password, password))
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }


        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CustomIdentity");
            return Redirect("~/");
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
