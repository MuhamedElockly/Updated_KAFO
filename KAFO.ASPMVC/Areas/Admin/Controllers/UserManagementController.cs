using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Users;
using KAFO.Utility.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class UserManagementController : Controller
	{
		UserManager _userManager;
		public UserManagementController(UserManager userManager)
		{
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult UpSert(UserCreateVM userCreateVM)
		{

			if (userCreateVM == null)
			{
				return BadRequest();
			}
			if (!ModelState.IsValid)
			{
				return View("Index", userCreateVM);
			}
			else if (userCreateVM.Id == 0)
			{
				var hashedPassword = PasswordHelper.HashPassword(userCreateVM.Password);

				User user = new User(name: userCreateVM.Name, email: userCreateVM.Email, role: userCreateVM.Role, gender: userCreateVM.Gender, phoneNumber: userCreateVM.Phone);
				user.Password = hashedPassword;
				_userManager.Add(user);
			}

			return View();
		}
	}
}
