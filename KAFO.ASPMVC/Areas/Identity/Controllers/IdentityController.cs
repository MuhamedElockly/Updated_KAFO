using KAFO.ASPMVC.Areas.Identity.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Users;
using KAFO.Utility.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> Login(IdentityViewModel IdentityViewModel, string? ReturnUrl = null)
		{

			if (ModelState.IsValid)
			{
				User userByEmailOrPhone = _userManager.FindByEmailOrPhone(IdentityViewModel.userName);
				if (userByEmailOrPhone == null)
				{
					return Json(new { success = false, message = "رقم الهاتف أو الايميل غير صحيح" });
				}
				User user = VerifyUser(IdentityViewModel.userName, IdentityViewModel.password);
				if (user != null)
				{
					Claim info0 = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
					Claim info1 = new Claim(ClaimTypes.Name, user.Name.Trim());
					Claim info2 = new Claim(ClaimTypes.Role, user.Role.ToLower().Trim());
					Claim info3 = new Claim(ClaimTypes.MobilePhone, user.PhoneNumber.Trim());
					ClaimsIdentity card = new ClaimsIdentity(new[] { info0, info1, info2 }, "CustomIdentity");
					ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(card);
					await HttpContext.SignInAsync("CustomIdentity", claimsPrincipal, new AuthenticationProperties
					{

						IsPersistent = IdentityViewModel.RememberMe, 
						ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),                                           
						AllowRefresh = true,

					});

					HttpContext.User = claimsPrincipal;

					string redirectUrl;
					if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
					{
						redirectUrl = ReturnUrl;
					}
					else
					{
						if (user.Role.ToLower() == UserRole.admin.ToString())
						{
							redirectUrl = Url.Action("Index", "Admin", new { area = "Admin" });
						}
						else
						{
							redirectUrl = Url.Action("Index", "POS", new { area = "Seller" });
						}
					}
					return Json(new { success = true, redirectUrl });
				}
				else
				{
					return Json(new { success = false, message = "كلمة المرور غير صحيحة" });
				}
			}
			else
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
			//	return Json(new { success = false, message = string.Join("\n", errors) });
				return Json(new { success = false, message = "gggggffff" });
			}
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
