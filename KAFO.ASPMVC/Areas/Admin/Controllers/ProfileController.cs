using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Users;
using KAFO.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KAFO.ASPMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ProfileController : Controller
    {
        private readonly UserManager _userManager;

        public ProfileController(UserManager userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _userManager.Get(userId);

            if (user == null)
                return NotFound();

            var profileVM = new ProfileVM
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Gender = user.Gender,
                Role = user.Role
            };

            return PartialView("~/Areas/Admin/Views/Admin/_ProfileManagement.cshtml", profileVM);
        }

        [HttpPost]
        public IActionResult UpdateProfile(ProfileVM model)
        {
            // Clear password validation errors if password fields are empty (user doesn't want to change password)
            if (string.IsNullOrEmpty(model.NewPassword) && string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmPassword");
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "بيانات غير صحيحة", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var user = _userManager.Get(model.Id);
            if (user == null)
                return Json(new { success = false, message = "المستخدم غير موجود" });

            // Check if email is already taken by another user
            if (!string.IsNullOrEmpty(model.Email))
            {
                var existingUser = _userManager.FindByEmailOrPhone(model.Email);
                if (existingUser != null && existingUser.Id != model.Id)
                {
                    return Json(new { success = false, message = "البريد الإلكتروني مستخدم بالفعل" });
                }
            }

            // Check if phone is already taken by another user
            if (!string.IsNullOrEmpty(model.Phone))
            {
                var existingUser = _userManager.FindByEmailOrPhone(model.Phone);
                if (existingUser != null && existingUser.Id != model.Id)
                {
                    return Json(new { success = false, message = "رقم الهاتف مستخدم بالفعل" });
                }
            }

            try
            {
                // Ensure Gender is not null or empty
                if (string.IsNullOrEmpty(model.Gender))
                {
                    return Json(new { success = false, message = "النوع مطلوب" });
                }

                user.Name = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.Gender = model.Gender;
                
                _userManager.Update(user);
                return Json(new { success = true, message = "تم تحديث البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث البيانات: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdatePassword(ProfileVM model)
        {
            if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
                return Json(new { success = false, message = "كلمة المرور مطلوبة" });

            if (model.NewPassword != model.ConfirmPassword)
                return Json(new { success = false, message = "كلمة المرور غير متطابقة" });

            if (model.NewPassword.Length < 6)
                return Json(new { success = false, message = "كلمة المرور يجب أن تكون على الأقل 6 أحرف" });

            var user = _userManager.Get(model.Id);
            if (user == null)
                return Json(new { success = false, message = "المستخدم غير موجود" });

            try
            {
                user.Password = PasswordHelper.HashPassword(model.NewPassword);
                _userManager.Update(user);
                return Json(new { success = true, message = "تم تحديث كلمة المرور بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث كلمة المرور: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            var user = _userManager.Get(id);
            if (user == null)
                return Json(new { success = false, message = "المستخدم غير موجود" });

            try
            {
                _userManager.Delete(id);
                return Json(new { success = true, message = "تم حذف الحساب بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حذف الحساب: " + ex.Message });
            }
        }
    }
} 