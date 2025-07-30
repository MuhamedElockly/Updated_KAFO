using KAFO.ASPMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KAFO.ASPMVC.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode
            };

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "الصفحة المطلوبة غير موجودة.";
                    ViewBag.ErrorTitle = "خطأ 404";
                    ViewBag.ErrorDescription = "عذراً، الصفحة التي تبحث عنها غير موجودة أو تم نقلها.";
                    break;
                case 403:
                    ViewBag.ErrorMessage = "ليس لديك صلاحية للوصول إلى هذه الصفحة.";
                    ViewBag.ErrorTitle = "خطأ 403";
                    ViewBag.ErrorDescription = "عذراً، ليس لديك الصلاحيات المطلوبة للوصول إلى هذه الصفحة.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "حدث خطأ في الخادم.";
                    ViewBag.ErrorTitle = "خطأ 500";
                    ViewBag.ErrorDescription = "عذراً، حدث خطأ في الخادم. يرجى المحاولة مرة أخرى لاحقاً.";
                    break;
                default:
                    ViewBag.ErrorMessage = "حدث خطأ غير متوقع.";
                    ViewBag.ErrorTitle = "خطأ";
                    ViewBag.ErrorDescription = "عذراً، حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.";
                    break;
            }

            return View("Error", errorViewModel);
        }

        [Route("Error")]
        public IActionResult Index(string requestId = null)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = requestId ?? Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = 500
            };

            ViewBag.ErrorMessage = "حدث خطأ غير متوقع.";
            ViewBag.ErrorTitle = "خطأ في النظام";
            ViewBag.ErrorDescription = "عذراً، حدث خطأ غير متوقع في النظام. يرجى المحاولة مرة أخرى أو التواصل مع الدعم الفني.";

            return View("Error", errorViewModel);
        }

        [Route("Error/NotFound")]
        public IActionResult NotFound()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = 404
            };

            ViewBag.ErrorMessage = "الصفحة المطلوبة غير موجودة.";
            ViewBag.ErrorTitle = "صفحة غير موجودة";
            ViewBag.ErrorDescription = "عذراً، الصفحة التي تبحث عنها غير موجودة أو تم نقلها.";

            return View("Error", errorViewModel);
        }

        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = 403
            };

            ViewBag.ErrorMessage = "ليس لديك صلاحية للوصول إلى هذه الصفحة.";
            ViewBag.ErrorTitle = "رفض الوصول";
            ViewBag.ErrorDescription = "عذراً، ليس لديك الصلاحيات المطلوبة للوصول إلى هذه الصفحة.";

            return View("Error", errorViewModel);
        }
    }
} 