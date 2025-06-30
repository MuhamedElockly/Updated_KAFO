using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Users;
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
           // var user = _userManager.Get(u => u.Id == userId.ToString());

            //if (user == null)
            //    return NotFound();

            var profileVM = new ProfileVM
            {
                Id = 23,
                Name = "Mohamed",
                Email = "elockl;u",
                Phone = "fdre",
                Gender = "Male",
                Role = "Admin"
            };

            return PartialView("~/Areas/Admin/Views/Admin/_ProfileManagement.cshtml", profileVM);
        }

        [HttpPost]
        public IActionResult UpdateProfile(ProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "بيانات غير صحيحة", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            //var user = _userManager.Get(u => u.Id == model.Id);
            //if (user == null)
            //    return Json(new { success = false, message = "المستخدم غير موجود" });

            //user.Name = model.Name;
            //user.PhoneNumber = model.Phone;

            try
            {
                //_userManager.Update(user);
                return Json(new { success = true, message = "تم تحديث البيانات بنجاح" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث البيانات" });
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

            //var user = _userManager.Get(u => u.Id == model.Id);
            //if (user == null)
            //    return Json(new { success = false, message = "المستخدم غير موجود" });

            try
            {
                //user.Password = KAFO.Utility.Helpers.PasswordHelper.HashPassword(model.NewPassword);
                //_userManager.Update(user);
                return Json(new { success = true, message = "تم تحديث كلمة المرور بنجاح" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث كلمة المرور" });
            }
        }

        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            //var user = _userManager.Get(u => u.Id == id);
            //if (user == null)
            //    return Json(new { success = false, message = "المستخدم غير موجود" });

            try
            {
                //_userManager.Delete(user);
                return Json(new { success = true, message = "تم حذف الحساب بنجاح" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حذف الحساب" });
            }
        }
    }
} 