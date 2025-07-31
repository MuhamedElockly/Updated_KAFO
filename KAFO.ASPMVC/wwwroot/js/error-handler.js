///**
// * Global Error Handler for AJAX requests
// * Provides user-friendly error messages and consistent error handling
// */

//class ErrorHandler {
//    constructor() {
//        this.initializeGlobalHandlers();
//    }

//    initializeGlobalHandlers() {
//        // Global AJAX error handler
//        $(document).ajaxError((event, xhr, settings, error) => {
//            this.handleAjaxError(xhr, settings, error);
//        });

//        // Global JavaScript error handler
//        window.addEventListener('error', (event) => {
//            this.handleJavaScriptError(event);
//        });

//        // Global unhandled promise rejection handler
//        window.addEventListener('unhandledrejection', (event) => {
//            this.handlePromiseRejection(event);
//        });
//    }

//    handleAjaxError(xhr, settings, error) {
//        console.error('AJAX Error:', { xhr, settings, error });

//        let errorMessage = 'حدث خطأ أثناء معالجة الطلب.';
//        let errorTitle = 'خطأ في النظام';

//        try {
//            if (xhr.responseJSON) {
//                const response = xhr.responseJSON;
//                errorMessage = response.Message || response.message || errorMessage;
//                errorTitle = response.ErrorTitle || 'خطأ في النظام';
//            } else if (xhr.responseText) {
//                const response = JSON.parse(xhr.responseText);
//                errorMessage = response.Message || response.message || errorMessage;
//                errorTitle = response.ErrorTitle || 'خطأ في النظام';
//            }
//        } catch (e) {
//            console.error('Error parsing error response:', e);
//        }

//        // Handle different HTTP status codes
//        switch (xhr.status) {
//            case 400:
//                errorTitle = 'بيانات غير صحيحة';
//                errorMessage = errorMessage || 'البيانات المدخلة غير صحيحة.';
//                break;
//            case 401:
//                errorTitle = 'غير مصرح';
//                errorMessage = 'يجب تسجيل الدخول للوصول إلى هذه الصفحة.';
//                window.location.href = '/Identity/Identity/Login';
//                return;
//            case 403:
//                errorTitle = 'رفض الوصول';
//                errorMessage = 'ليس لديك صلاحية للوصول إلى هذا المورد.';
//                break;
//            case 404:
//                errorTitle = 'غير موجود';
//                errorMessage = 'المورد المطلوب غير موجود.';
//                break;
//            case 500:
//                errorTitle = 'خطأ في الخادم';
//                errorMessage = 'حدث خطأ في الخادم. يرجى المحاولة مرة أخرى لاحقاً.';
//                break;
//            case 0:
//                errorTitle = 'خطأ في الاتصال';
//                errorMessage = 'لا يمكن الاتصال بالخادم. يرجى التحقق من اتصال الإنترنت.';
//                break;
//        }

//        this.showErrorModal(errorTitle, errorMessage, xhr.status);
//    }

//    handleJavaScriptError(event) {
//        console.error('JavaScript Error:', event.error);
        
//        const errorMessage = 'حدث خطأ في JavaScript. يرجى تحديث الصفحة والمحاولة مرة أخرى.';
//        this.showErrorModal('خطأ في JavaScript', errorMessage, 500);
//    }

//    handlePromiseRejection(event) {
//        console.error('Unhandled Promise Rejection:', event.reason);
        
//        const errorMessage = 'حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.';
//        this.showErrorModal('خطأ في النظام', errorMessage, 500);
//    }

//    showErrorModal(title, message, statusCode = 500) {
//        // Check if SweetAlert2 is available
//        if (typeof Swal !== 'undefined') {
//            Swal.fire({
//                icon: this.getErrorIcon(statusCode),
//                title: title,
//                text: message,
//                confirmButtonText: 'حسناً',
//                confirmButtonColor: '#667eea',
//                background: '#fff',
//                color: '#222',
//                showClass: { popup: 'animate__animated animate__fadeInDown' },
//                hideClass: { popup: 'animate__animated animate__fadeOutUp' }
//            });
//        } else {
//            // Fallback to browser alert
//            alert(`${title}\n\n${message}`);
//        }
//    }

//    getErrorIcon(statusCode) {
//        switch (statusCode) {
//            case 400:
//                return 'warning';
//            case 401:
//            case 403:
//                return 'error';
//            case 404:
//                return 'question';
//            case 500:
//            default:
//                return 'error';
//        }
//    }

//    // Utility method to handle specific errors
//    handleValidationError(errors) {
//        let errorMessage = 'يرجى تصحيح الأخطاء التالية:\n';
        
//        if (typeof errors === 'object') {
//            Object.keys(errors).forEach(key => {
//                if (Array.isArray(errors[key])) {
//                    errors[key].forEach(error => {
//                        errorMessage += `• ${error}\n`;
//                    });
//                } else {
//                    errorMessage += `• ${errors[key]}\n`;
//                }
//            });
//        }

//        this.showErrorModal('خطأ في التحقق من البيانات', errorMessage, 400);
//    }

//    // Method to show success message
//    showSuccessMessage(title, message) {
//        if (typeof Swal !== 'undefined') {
//            Swal.fire({
//                icon: 'success',
//                title: title,
//                text: message,
//                confirmButtonText: 'حسناً',
//                confirmButtonColor: '#28a745',
//                background: '#fff',
//                color: '#222',
//                showClass: { popup: 'animate__animated animate__fadeInDown' },
//                hideClass: { popup: 'animate__animated animate__fadeOutUp' }
//            });
//        } else {
//            alert(`${title}\n\n${message}`);
//        }
//    }

//    // Method to show confirmation dialog
//    showConfirmation(title, message, callback) {
//        if (typeof Swal !== 'undefined') {
//            Swal.fire({
//                icon: 'question',
//                title: title,
//                text: message,
//                showCancelButton: true,
//                confirmButtonText: 'نعم',
//                cancelButtonText: 'لا',
//                confirmButtonColor: '#667eea',
//                cancelButtonColor: '#6c757d',
//                background: '#fff',
//                color: '#222',
//                showClass: { popup: 'animate__animated animate__fadeInDown' },
//                hideClass: { popup: 'animate__animated animate__fadeOutUp' }
//            }).then((result) => {
//                if (result.isConfirmed && callback) {
//                    callback();
//                }
//            });
//        } else {
//            if (confirm(`${title}\n\n${message}`) && callback) {
//                callback();
//            }
//        }
//    }
//}

//// Initialize error handler when document is ready
//$(document).ready(function() {
//    window.errorHandler = new ErrorHandler();
//});

//// Export for use in other modules
//if (typeof module !== 'undefined' && module.exports) {
//    module.exports = ErrorHandler;
//} 