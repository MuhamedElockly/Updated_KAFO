using System;

namespace KAFO.ASPMVC.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }
        public string UserMessage { get; }

        public BusinessException(string message, string userMessage = null, string errorCode = null) 
            : base(message)
        {
            UserMessage = userMessage ?? message;
            ErrorCode = errorCode ?? "BUSINESS_ERROR";
        }

        public BusinessException(string message, Exception innerException, string userMessage = null, string errorCode = null) 
            : base(message, innerException)
        {
            UserMessage = userMessage ?? message;
            ErrorCode = errorCode ?? "BUSINESS_ERROR";
        }
    }

    public class ValidationException : BusinessException
    {
        public ValidationException(string message, string userMessage = null) 
            : base(message, userMessage, "VALIDATION_ERROR")
        {
        }
    }

    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message, string userMessage = null) 
            : base(message, userMessage, "NOT_FOUND")
        {
        }
    }

    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message, string userMessage = null) 
            : base(message, userMessage, "UNAUTHORIZED")
        {
        }
    }
} 