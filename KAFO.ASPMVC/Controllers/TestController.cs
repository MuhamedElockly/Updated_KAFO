using KAFO.ASPMVC.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace KAFO.ASPMVC.Controllers
{
    public class TestController : Controller
    {
        [Route("Test/ThrowException")]
        public IActionResult ThrowException()
        {
            throw new Exception("This is a test exception to demonstrate global error handling.");
        }

        [Route("Test/ThrowBusinessException")]
        public IActionResult ThrowBusinessException()
        {
            throw new BusinessException(
                "This is a business logic error", 
                "حدث خطأ في منطق الأعمال. يرجى المحاولة مرة أخرى.",
                "BUSINESS_LOGIC_ERROR"
            );
        }

        [Route("Test/ThrowValidationException")]
        public IActionResult ThrowValidationException()
        {
            throw new ValidationException(
                "Validation failed", 
                "البيانات المدخلة غير صحيحة. يرجى التحقق من المعلومات."
            );
        }

        [Route("Test/ThrowNotFoundException")]
        public IActionResult ThrowNotFoundException()
        {
            throw new NotFoundException(
                "Resource not found", 
                "المورد المطلوب غير موجود."
            );
        }

        [Route("Test/ThrowUnauthorizedException")]
        public IActionResult ThrowUnauthorizedException()
        {
            throw new UnauthorizedException(
                "Access denied", 
                "ليس لديك صلاحية للوصول إلى هذا المورد."
            );
        }

        [Route("Test/AjaxError")]
        public IActionResult AjaxError()
        {
            return Json(new { 
                Success = false, 
                Message = "هذا خطأ تجريبي للاختبار",
                ErrorTitle = "خطأ تجريبي"
            });
        }
    }
} 