using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using KAFO.ASPMVC.Exceptions;
using System;
using System.Net;

namespace KAFO.ASPMVC.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An exception occurred: {Message}", context.Exception.Message);

            var result = new ObjectResult(new
            {
                Success = false,
                Message = GetUserFriendlyMessage(context.Exception),
                RequestId = context.HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            })
            {
                StatusCode = GetStatusCode(context.Exception)
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }

        private string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                BusinessException businessEx => businessEx.UserMessage,
                UnauthorizedAccessException => "ليس لديك صلاحية للوصول إلى هذا المورد.",
                ArgumentNullException => "البيانات المطلوبة غير مكتملة.",
                ArgumentException => "البيانات المدخلة غير صحيحة.",
                InvalidOperationException => "العملية المطلوبة غير صحيحة.",
                TimeoutException => "انتهت مهلة العملية. يرجى المحاولة مرة أخرى.",
                _ => "حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى أو التواصل مع الدعم الفني."
            };
        }

        private int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                BusinessException businessEx => businessEx.ErrorCode switch
                {
                    "NOT_FOUND" => (int)HttpStatusCode.NotFound,
                    "UNAUTHORIZED" => (int)HttpStatusCode.Forbidden,
                    "VALIDATION_ERROR" => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.BadRequest
                },
                UnauthorizedAccessException => (int)HttpStatusCode.Forbidden,
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                TimeoutException => (int)HttpStatusCode.RequestTimeout,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
} 