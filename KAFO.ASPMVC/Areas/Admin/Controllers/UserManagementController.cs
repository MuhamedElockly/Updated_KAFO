using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Products;
using KAFO.Domain.Users;
using KAFO.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "admin")]
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
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return BadRequest();
			}

			User user = _userManager.Get(id.Value);

			if (user == null)
			{
				return NotFound();
			}
			UserCreateVM userVM = new UserCreateVM() { Id = user.Id, Name = user.Name, Phone = user.PhoneNumber, Email = user.Email, Gender = user.Gender, Role = user.Role };

			return View("Index", userVM);
		}
		public IActionResult UpSert(UserCreateVM userCreateVM)
		{
			TempData["Spinner"] = false;

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
				if (string.IsNullOrWhiteSpace(userCreateVM.Password))
				{
					ModelState.AddModelError("Password", "كلمة المرور مطلوبة");
					return View("Index", userCreateVM);
				}
				if (userCreateVM.Password != userCreateVM.ConfirmPassword)
				{
					ModelState.AddModelError("ConfirmPassword", "كلمتا المرور غير متطابقتين");
					return View("Index", userCreateVM);
				}

				if (userCreateVM.Email != null && _userManager.FindByEmailOrPhone(userCreateVM.Email) != null)
				{
					ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل");
					return View("Index", userCreateVM);
				}
				else if (_userManager.FindByEmailOrPhone(userCreateVM.Phone) != null)
				{
					ModelState.AddModelError("Phone", "رقم الموبايل مستخدم بالفعل");
					return View("Index", userCreateVM);
				}
				var hashedPassword = PasswordHelper.HashPassword(userCreateVM.Password);

				User user = new User(name: userCreateVM.Name, email: userCreateVM.Email, role: userCreateVM.Role, gender: userCreateVM.Gender, phoneNumber: userCreateVM.Phone);
				user.Password = hashedPassword;
				_userManager.Add(user);
				TempData["Success"] = "تم إضافة المستخدم بنجاح";
			}
			else
			{
				var user = _userManager.Get(userCreateVM.Id);
				if (user == null) return NotFound();

				if (!string.IsNullOrWhiteSpace(userCreateVM.Password))
				{
					if (userCreateVM.Password != userCreateVM.ConfirmPassword)
					{
						ModelState.AddModelError("ConfirmPassword", "كلمتا المرور غير متطابقتين");
						return View("Index", userCreateVM);
					}

					user.Password = PasswordHelper.HashPassword(userCreateVM.Password);
				}
				if (userCreateVM.Email != null && !_userManager.CanUpdateByEmailOrPhone(userCreateVM.Email, userCreateVM.Id))
				{
					ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل");
					return View("Index", userCreateVM);
				}
				else if (!_userManager.CanUpdateByEmailOrPhone(userCreateVM.Phone, userCreateVM.Id))
				{
					ModelState.AddModelError("Phone", "رقم الموبايل مستخدم بالفعل");
					return View("Index", userCreateVM);
				}

				if (userCreateVM.Password != null)
				{
					user.Password = PasswordHelper.HashPassword(userCreateVM.Password);
				}
				user.Email = userCreateVM.Email;
				user.PhoneNumber = userCreateVM.Phone;
				user.Gender = userCreateVM.Gender;
				user.Name = userCreateVM.Name;
				_userManager.Update(user);
				TempData["Success"] = "تم تعديل المستخدم بنجاح";
			}

			return RedirectToAction("Index", "Admin", new { area = "Admin" });
		}
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				//  TempData["Error"] = "حدث خطأ";
				return Json(new { success = false, message = "المستخدم غير موجود" });
			}

			string res = _userManager.Delete(id.Value);
			if (res == null)
			{
				return Json(new { success = true });
			}
			else
			{
				return Json(new { success = false, message = res });
			}

		}
	}
}
