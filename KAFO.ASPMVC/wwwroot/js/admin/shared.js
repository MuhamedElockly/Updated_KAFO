// Global function to close mobile sidebar
function closeMobileSidebar() {
    try {
        const sidebar = $('#sidebar');
        const overlay = $('#sidebarOverlay');
        const toggleBtn = $('#mobileSidebarToggle');

        if (sidebar.length) {
            // Re-enable body scroll
            $('body').removeClass('sidebar-open');

            // Hide overlay
            if (overlay.length) {
                overlay.removeClass('active');
            }

            // Close sidebar
            sidebar.removeClass('sidebar-open');

            // Reset toggle button
            if (toggleBtn.length) {
                toggleBtn.removeClass('rotating');
                toggleBtn.find('i').removeClass('fa-times').addClass('fa-bars');
            }

            // Reset sidebar items
            sidebar.find('li').css({
                'opacity': '1',
                'transform': 'translateX(0)'
            });

            // Add haptic feedback
            if ('vibrate' in navigator) {
                navigator.vibrate(25);
            }
        }
    } catch (error) {
        console.warn('Close sidebar error:', error);
    }
}

function loadAdminContent(action, pageNumber) {
    // Show loading indicator with enhanced styling
    $('#mainContent').html(`
        <div class="text-center py-5">
            <div class="spinner-container">
                <i class="fas fa-spinner fa-spin fa-3x text-primary mb-3"></i>
                <p class="text-muted">جاري التحميل...</p>
            </div>
        </div>
    `);

    // Construct URL with optional page number
    let url = `/Admin/Admin/${action}`;
    if (pageNumber) {
        url += `?page=${pageNumber}`;
    }

    // Load the partial view via AJAX with enhanced error handling
    return $.ajax({
        url: url,
        type: 'GET',
        timeout: 30000, // 30 second timeout
        success: function (result) {
            $('#mainContent').html(result);

            // Initialize report functionality if we're on the reports page
            if (action === 'Reports' && typeof initializeReportFunctionality === 'function') {
                initializeReportFunctionality();
            }

            // Initialize invoice functionality if we're on the invoices page
            if (action === 'Invoices' && typeof initializeInvoiceFunctionality === 'function') {
                initializeInvoiceFunctionality();
            }

            // Initialize daily inventory functionality if we're on the daily inventory page
            if (action === 'DailyInventory' && typeof initializeDailyInventory === 'function') {
                // Reset and re-initialize to prevent duplicate pagination
                if (typeof resetDailyInventory === 'function') {
                    resetDailyInventory();
                } else {
                    initializeDailyInventory();
                }
            }
            
            // Add smooth scroll to top for mobile
            if ($(window).width() <= 768) {
                $('html, body').animate({
                    scrollTop: 0
                }, 300);
            }

            // Add smooth scroll to top for mobile
            if ($(window).width() <= 768) {
                $('html, body').animate({
                    scrollTop: 0
                }, 300);
            }
        },
        error: function (xhr, status, error) {
            let errorMessage = 'حدث خطأ أثناء تحميل المحتوى';

            if (status === 'timeout') {
                errorMessage = 'انتهت مهلة الاتصال، يرجى المحاولة مرة أخرى';
            } else if (xhr.status === 404) {
                errorMessage = 'الصفحة المطلوبة غير موجودة';
            } else if (xhr.status === 500) {
                errorMessage = 'خطأ في الخادم، يرجى المحاولة لاحقاً';
            }

            $('#mainContent').html(`
                <div class="alert alert-danger text-center">
                    <i class="fas fa-exclamation-triangle fa-2x mb-3"></i>
                    <h5>خطأ في التحميل</h5>
                    <p>${errorMessage}</p>
                    <button class="btn btn-primary mt-3" onclick="location.reload()">
                        <i class="fas fa-redo"></i> إعادة المحاولة
                    </button>
                </div>
            `);
        }
    });
}

// Handle sidebar navigation
$(document).ready(function () {
    // Enhanced mobile detection with better breakpoints
    const isMobile = $(window).width() <= 768;
    const isTablet = $(window).width() > 768 && $(window).width() <= 1024;
    const sidebarExists = $('#sidebar').length > 0;

    // Initialize sidebar functionality
    initSidebarNavigation();

    function initSidebarNavigation() {
        $('.sidebar-link').on('click', function (e) {
            e.preventDefault();

            // Add loading state to clicked link
            const $this = $(this);
            $this.addClass('loading');
            $this.find('i').addClass('fa-spin');

            // Remove active class from all links
            $('.sidebar-link').parent().removeClass('active');
            // Add active class to clicked link
            $this.parent().addClass('active');

            const action = $this.data('action');

            // Load content with enhanced error handling
            loadAdminContent(action, 1).always(function () {
                // Remove loading state
                $this.removeClass('loading');
                $this.find('i').removeClass('fa-spin');
            });

            // Close mobile sidebar after navigation
            if ((isMobile || isTablet) && sidebarExists) {
                closeMobileSidebar();
            }
        });
    }

    // Enhanced mobile sidebar functionality
    function initMobileSidebar() {
        const sidebar = $('#sidebar');
        const overlay = $('#sidebarOverlay');
        const toggleBtn = $('#mobileSidebarToggle');

        // Only initialize if elements exist
        if (!sidebar.length || !overlay.length || !toggleBtn.length) {
            return;
        }

        // Enhanced toggle sidebar with better animations
        toggleBtn.on('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            try {
                const isOpen = sidebar.hasClass('sidebar-open');

                if (!isOpen) {
                    // Opening animation with enhanced effects
                    openMobileSidebar();
                } else {
                    // Closing animation
                    closeMobileSidebar();
                }
            } catch (error) {
                console.warn('Sidebar toggle error:', error);
            }
        });

        function openMobileSidebar() {
            // Prevent body scroll when sidebar is open
            $('body').addClass('sidebar-open');

            // Show overlay first
            overlay.addClass('active');

            // Open sidebar with smooth animation
            sidebar.addClass('sidebar-open');

            // Animate toggle button
            toggleBtn.addClass('rotating');

            // Change icon with smooth transition
            const icon = toggleBtn.find('i');
            icon.fadeOut(150, function () {
                icon.removeClass('fa-bars').addClass('fa-times').fadeIn(150);
            });

            // Add staggered entrance animation to sidebar items
            setTimeout(() => {
                sidebar.find('li').each(function (index) {
                    $(this).css({
                        'opacity': '0',
                        'transform': 'translateX(30px)'
                    }).delay(index * 80).animate({
                        'opacity': '1',
                        'transform': 'translateX(0)'
                    }, 400, 'swing');
                });
            }, 300);

            // Add haptic feedback
            if ('vibrate' in navigator) {
                navigator.vibrate([50, 25, 50]);
            }
        }

        // Close sidebar when clicking overlay with enhanced feedback
        overlay.on('click', function () {
            try {
                closeMobileSidebar();

                // Add ripple effect to overlay
                const ripple = $('<div class="ripple-effect"></div>');
                $(this).append(ripple);

                setTimeout(() => {
                    ripple.remove();
                }, 600);
            } catch (error) {
                console.warn('Overlay click error:', error);
            }
        });

        // Enhanced keyboard navigation
        $(document).on('keydown', function (e) {
            try {
                if (e.key === 'Escape' && sidebar.hasClass('sidebar-open')) {
                    closeMobileSidebar();
                }
            } catch (error) {
                console.warn('Keyboard navigation error:', error);
            }
        });

        // Close sidebar when clicking outside with improved detection
        $(document).on('click', function (e) {
            try {
                if ($(window).width() <= 768 &&
                    !sidebar.is(e.target) &&
                    sidebar.has(e.target).length === 0 &&
                    !toggleBtn.is(e.target) &&
                    toggleBtn.has(e.target).length === 0 &&
                    !overlay.is(e.target)) {
                    closeMobileSidebar();
                }
            } catch (error) {
                console.warn('Outside click error:', error);
            }
        });

        // Enhanced window resize handling
        $(window).on('resize', function () {
            try {
                if ($(window).width() > 768) {
                    closeMobileSidebar();
                }
            } catch (error) {
                console.warn('Resize error:', error);
            }
        });
    }

    // Initialize mobile sidebar only if needed
    if (isMobile && sidebarExists) {
        initMobileSidebar();
    }

    // Add swipe gesture support for mobile with enhanced sensitivity
    function initSwipeGestures() {
        // Only initialize on mobile devices and if sidebar exists
        if ($(window).width() > 768 || !$('#sidebar').length) return;

        let startX = 0;
        let startY = 0;
        let isDragging = false;
        let touchStartTime = 0;

        // Touch start with enhanced detection
        document.addEventListener('touchstart', function (e) {
            try {
                if (e.touches && e.touches.length > 0) {
                    startX = e.touches[0].clientX;
                    startY = e.touches[0].clientY;
                    isDragging = false;
                    touchStartTime = Date.now();
                }
            } catch (error) {
                console.warn('Touch start error:', error);
            }
        }, { passive: true });

        // Touch move with improved gesture detection
        document.addEventListener('touchmove', function (e) {
            try {
                if (!isDragging && e.touches && e.touches.length > 0) {
                    const deltaX = Math.abs(e.touches[0].clientX - startX);
                    const deltaY = Math.abs(e.touches[0].clientY - startY);

                    if (deltaX > 8 && deltaX > deltaY * 1.5) {
                        isDragging = true;
                    }
                }
            } catch (error) {
                console.warn('Touch move error:', error);
            }
        }, { passive: true });

        // Touch end with enhanced gesture recognition
        document.addEventListener('touchend', function (e) {
            try {
                if (isDragging && $(window).width() <= 768 && e.changedTouches && e.changedTouches.length > 0) {
                    const endX = e.changedTouches[0].clientX;
                    const deltaX = endX - startX;
                    const touchDuration = Date.now() - touchStartTime;

                    // Swipe right to open sidebar (more sensitive)
                    if (deltaX > 40 && endX < 120 && touchDuration < 500) {
                        const sidebar = $('#sidebar');
                        const overlay = $('#sidebarOverlay');
                        const toggleBtn = $('#mobileSidebarToggle');

                        if (sidebar.length && !sidebar.hasClass('sidebar-open')) {
                            // Add haptic feedback safely
                            try {
                                if ('vibrate' in navigator && navigator.vibrate) {
                                    navigator.vibrate([50, 25, 50]);
                                }
                            } catch (vibrateError) {
                                console.warn('Vibration not supported:', vibrateError);
                            }

                            sidebar.addClass('sidebar-open');
                            overlay.addClass('active');
                            toggleBtn.find('i').removeClass('fa-bars').addClass('fa-times');
                        }
                    }
                    // Swipe left to close sidebar (more sensitive)
                    else if (deltaX < -40 && touchDuration < 500) {
                        closeMobileSidebar();

                        // Add haptic feedback safely
                        try {
                            if ('vibrate' in navigator && navigator.vibrate) {
                                navigator.vibrate(25);
                            }
                        } catch (vibrateError) {
                            console.warn('Vibration not supported:', vibrateError);
                        }
                    }
                }
            } catch (error) {
                console.warn('Touch end error:', error);
            }
        }, { passive: true });
    }

    // Initialize swipe gestures only on mobile
    if (isMobile && sidebarExists) {
        initSwipeGestures();
    }

    // Add haptic feedback for mobile devices with enhanced patterns
    function addHapticFeedback() {
        // Only add haptic feedback on mobile devices
        if ($(window).width() > 768) return;

        try {
            if ('vibrate' in navigator && navigator.vibrate) {
                $('.sidebar-link').on('click', function () {
                    try {
                        navigator.vibrate([30, 20, 30]);
                    } catch (error) {
                        console.warn('Vibration error on sidebar link:', error);
                    }
                });

                $('.mobile-sidebar-toggle').on('click', function () {
                    try {
                        const isOpen = $('#sidebar').hasClass('sidebar-open');
                        navigator.vibrate(isOpen ? [50, 25] : [25, 50]);
                    } catch (error) {
                        console.warn('Vibration error on toggle button:', error);
                    }
                });
            }
        } catch (error) {
            console.warn('Haptic feedback initialization error:', error);
        }
    }

    // Initialize haptic feedback only on mobile
    if (isMobile && sidebarExists) {
        addHapticFeedback();
    }

    // Performance optimization: Debounce resize events with enhanced timing
    let resizeTimeout;
    $(window).on('resize', function () {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(function () {
            if ($(window).width() > 768) {
                closeMobileSidebar();
            }
        }, 300);
    });

    // Enhanced keyboard navigation with focus management
    $(document).on('keydown', function (e) {
        if (e.key === 'Tab' && $('#sidebar').hasClass('sidebar-open')) {
            // Keep focus within sidebar when open
            const sidebarLinks = $('#sidebar .sidebar-link');
            const firstLink = sidebarLinks.first();
            const lastLink = sidebarLinks.last();

            if (e.shiftKey && document.activeElement === firstLink[0]) {
                e.preventDefault();
                lastLink.focus();
            } else if (!e.shiftKey && document.activeElement === lastLink[0]) {
                e.preventDefault();
                firstLink.focus();
            }
        }
    });

    // Add loading states and visual feedback
    $('.sidebar-link').on('click', function () {
        const $this = $(this);
        const originalText = $this.text();

        // Add loading state
        $this.addClass('loading');
        $this.find('i').addClass('fa-spin');

        // Remove loading state after content loads
        setTimeout(() => {
            $this.removeClass('loading');
            $this.find('i').removeClass('fa-spin');
        }, 1000);
    });

    // Add CSS for new animations
    $('<style>')
        .prop('type', 'text/css')
        .html(`
            .mobile-sidebar-toggle.rotating {
                animation: rotate 0.3s ease-in-out;
            }
            
            @keyframes rotate {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(180deg); }
            }
            
            .ripple-effect {
                position: absolute;
                border-radius: 50%;
                background: rgba(255,255,255,0.3);
                transform: scale(0);
                animation: ripple 0.6s linear;
                pointer-events: none;
            }
            
            @keyframes ripple {
                to {
                    transform: scale(4);
                    opacity: 0;
                }
            }
            
            .sidebar-link.loading {
                opacity: 0.7;
                pointer-events: none;
            }
            
            .fa-spin {
                animation: fa-spin 1s infinite linear;
            }
            
            @keyframes fa-spin {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
            }
        `)
        .appendTo('head');
});

function toggleSpinner(show, message = '', options = {}) {
    // Modern default options
    const defaults = {
        color: '#6200ee',
        secondaryColor: 'rgba(98, 0, 238, 0.2)',
        size: 60,
        thickness: 4,
        speed: 1,
        overlayColor: 'rgba(255, 255, 255, 0.85)',
        textColor: '#4a4a4a',
        blurAmount: '3px',
        animation: 'spin' // 'spin', 'pulse', or 'wave'
    };

    const config = { ...defaults, ...options };

    let spinnerOverlay = document.getElementById('modern-spinner-overlay');

    if (show) {
        // Create spinner if it doesn't exist
        if (!spinnerOverlay) {
            spinnerOverlay = document.createElement('div');
            spinnerOverlay.id = 'modern-spinner-overlay';
            spinnerOverlay.className = 'spinner-overlay';
            spinnerOverlay.style.setProperty('--overlay-color', config.overlayColor);
            spinnerOverlay.style.setProperty('--blur-amount', config.blurAmount);

            const spinnerContainer = document.createElement('div');
            spinnerContainer.className = 'spinner-container';

            // Create spinner based on animation type
            let spinner;
            if (config.animation === 'pulse') {
                spinner = createPulseSpinner(config);
            } else if (config.animation === 'wave') {
                spinner = createWaveSpinner(config);
            } else {
                spinner = createSpinSpinner(config);
            }

            const textElement = document.createElement('div');
            textElement.className = 'spinner-text';
            textElement.textContent = message;
            textElement.style.color = config.textColor;

            spinnerContainer.appendChild(spinner);
            if (message) {
                spinnerContainer.appendChild(textElement);
            }

            spinnerOverlay.appendChild(spinnerContainer);
            document.body.appendChild(spinnerOverlay);
        } else {
            // Update existing spinner
            const spinner = spinnerOverlay.querySelector('.spinner-element');
            const textElement = spinnerOverlay.querySelector('.spinner-text');

            if (spinner) {
                // Update spinner styles based on type
                if (spinner.classList.contains('spin-spinner')) {
                    spinner.style.width = `${config.size}px`;
                    spinner.style.height = `${config.size}px`;
                    spinner.style.borderWidth = `${config.thickness}px`;
                    spinner.style.borderTopColor = config.color;
                    spinner.style.borderLeftColor = config.secondaryColor;
                }
                // Add similar updates for other spinner types
            }

            if (textElement) {
                textElement.textContent = message;
                textElement.style.color = config.textColor;
            } else if (message) {
                const newTextElement = document.createElement('div');
                newTextElement.className = 'spinner-text';
                newTextElement.textContent = message;
                newTextElement.style.color = config.textColor;
                spinnerOverlay.querySelector('.spinner-container').appendChild(newTextElement);
            }
        }

        spinnerOverlay.style.display = 'flex';
    } else if (spinnerOverlay) {
        // Add fade-out animation before hiding
        spinnerOverlay.style.opacity = '0';
        setTimeout(() => {
            spinnerOverlay.style.display = 'none';
            spinnerOverlay.style.opacity = '1';
        }, 300); // Match this with the CSS transition time
    }
}

// Helper functions to create different spinner types
function createSpinSpinner(config) {
    const spinner = document.createElement('div');
    spinner.className = 'spinner-element spin-spinner';
    spinner.style.width = `${config.size}px`;
    spinner.style.height = `${config.size}px`;
    spinner.style.borderWidth = `${config.thickness}px`;
    spinner.style.borderTopColor = config.color;
    spinner.style.borderLeftColor = config.secondaryColor;
    spinner.style.animationDuration = `${1 / config.speed}s`;
    return spinner;
}

function createPulseSpinner(config) {
    const spinner = document.createElement('div');
    spinner.className = 'spinner-element pulse-spinner';
    spinner.style.width = `${config.size}px`;
    spinner.style.height = `${config.size}px`;
    spinner.style.backgroundColor = config.color;
    spinner.style.animationDuration = `${1 / config.speed}s`;
    return spinner;
}

function createWaveSpinner(config) {
    const container = document.createElement('div');
    container.className = 'wave-container';
    container.style.width = `${config.size * 1.5}px`;
    container.style.height = `${config.size}px`;

    for (let i = 0; i < 5; i++) {
        const bar = document.createElement('div');
        bar.className = 'wave-bar';
        bar.style.backgroundColor = config.color;
        bar.style.animationDelay = `${i * 0.1}s`;
        bar.style.animationDuration = `${0.8 / config.speed}s`;
        container.appendChild(bar);
    }

    return container;
}
/**
 * Shows a confirmation dialog before executing a delete operation
 * @param {number} itemId - The ID of the item to delete
 * @param {string} controllerName - Name of the controller (without "Controller")
 * @param {string} actionName - Name of the action method (default: "Delete")
 * @param {string} confirmationMessage - Custom confirmation message
 * @param {string} successMessage - Custom success message
 * @param {function} [callback] - Optional callback after success
 */
function showDeleteConfirmation(options) {
    // Default values with Arabic text
    const settings = {
        confirmText: 'هل أنت متأكد من الحذف؟',
        confirmDescription: 'لن تتمكن من استعادة هذا العنصر مرة أخرى!',
        confirmButtonText: 'نعم، احذفه!',
        cancelButtonText: 'إلغاء',
        processingText: 'جاري المعالجة...',
        waitingText: 'الرجاء الانتظار بينما نعالج طلبك',
        successText: 'تم الحذف بنجاح',
        errorText: 'فشل الاتصال بالخادم',
        callback: null,
        ajaxData: {},
        ...options
    };

    // Confirmation dialog
    Swal.fire({
        title: settings.confirmText,
        text: settings.confirmDescription,
        icon: 'warning',
        iconColor: '#dc3545',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: settings.confirmButtonText,
        cancelButtonText: settings.cancelButtonText,
        background: 'white',
        // Removed problematic backdrop with GIF
        backdrop: true,
        // Removed problematic animation classes
        customClass: {
            confirmButton: 'btn btn-danger shadow-sm px-4 py-2 me-2',
            cancelButton: 'btn btn-secondary shadow-sm px-4 py-2 ms-2',
            actions: 'swal2-actions-custom'
        },
        buttonsStyling: false,
        allowOutsideClick: true,
        allowEscapeKey: true,
        focusConfirm: false,
        reverseButtons: false
    }).then((result) => {
        // Check if user confirmed (not dismissed/canceled)
        if (result.isConfirmed) {
            // Validate required settings before proceeding
            if (!settings.actionUrl) {
                Swal.fire({
                    title: 'خطأ!',
                    text: 'رابط العملية غير محدد',
                    icon: 'error',
                    confirmButtonText: 'موافق'
                });
                return;
            }

            // Show processing dialog with proper timeout handling
            const processingDialog = Swal.fire({
                title: settings.processingText,
                html: settings.waitingText,
                timerProgressBar: true,
                timer: 30000, // 30 second timeout
                didOpen: () => {
                    Swal.showLoading();
                },
                background: 'white',
                backdrop: true, // Simplified backdrop
                allowOutsideClick: false,
                allowEscapeKey: false,
                showConfirmButton: false
            });

            // Prepare AJAX data
            const requestData = {
                id: settings.itemId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                ...settings.ajaxData
            };

            // Store the AJAX request so we can abort it if needed
            const ajaxRequest = $.ajax({
                url: settings.actionUrl,
                type: 'POST',
                data: requestData,
                timeout: 25000, // 25 second timeout (less than Swal timer)
                success: function (response) {
                    // Close processing dialog
                    Swal.close();

                    if (response && response.success) {
                        Swal.fire({
                            title: 'تم!',
                            confirmButtonText: "موافق",
                            text: settings.successText,
                            icon: 'success',
                            iconColor: '#28a745',
                            confirmButtonColor: '#28a745',
                            background: 'white',
                            customClass: {
                                confirmButton: 'btn btn-success shadow-sm px-4 py-2'
                            },
                            allowOutsideClick: true,
                            allowEscapeKey: true
                        }).then(() => {
                            if (settings.callback && typeof settings.callback === 'function') {
                                settings.callback(response);
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'خطأ!',
                            text: response.message || 'حدث خطأ أثناء المعالجة',
                            icon: 'error',
                            confirmButtonText: 'موافق',
                            allowOutsideClick: true,
                            allowEscapeKey: true
                        });
                    }
                },
                error: function (xhr, status, error) {
                    // Close processing dialog
                    Swal.close();

                    let errorMessage = settings.errorText;

                    // Handle different error types
                    if (status === 'timeout') {
                        errorMessage = 'انتهت مهلة الاتصال. يرجى المحاولة مرة أخرى.';
                    } else if (xhr.responseJSON && xhr.responseJSON.message) {
                        errorMessage = xhr.responseJSON.message;
                    } else if (xhr.responseText) {
                        try {
                            const errorObj = JSON.parse(xhr.responseText);
                            errorMessage = errorObj.message || errorMessage;
                        } catch (e) {
                            // Keep default error message
                        }
                    }

                    Swal.fire({
                        title: 'خطأ!',
                        text: errorMessage,
                        icon: 'error',
                        confirmButtonText: 'موافق',
                        allowOutsideClick: true,
                        allowEscapeKey: true
                    });
                },
                complete: function () {
                    // This runs regardless of success or error
                    console.log('AJAX request completed');
                }
            });

            // Handle processing dialog timeout
            processingDialog.then((result) => {
                if (result.dismiss === Swal.DismissReason.timer) {
                    // Dialog timed out, abort AJAX request
                    if (ajaxRequest && ajaxRequest.readyState !== 4) {
                        ajaxRequest.abort();
                    }

                    Swal.fire({
                        title: 'انتهت المهلة الزمنية!',
                        text: 'العملية تستغرق وقتاً أطول من المتوقع',
                        icon: 'warning',
                        confirmButtonText: 'موافق'
                    });
                }
            });
        }
        // If result.isDismissed or result.isDenied, do nothing (user canceled)
        // This prevents any freezing issues
    }).catch((error) => {
        // Handle any unexpected errors in the Swal promise chain
        console.error('Error in delete confirmation:', error);
        Swal.fire({
            title: 'خطأ!',
            text: 'حدث خطأ غير متوقع',
            icon: 'error',
            confirmButtonText: 'موافق'
        });
    });
}

function showAddCreditCustomerModal() {
    $.get('/Admin/CreditCustomerManagement/Edit/0', function (data) {
        $('#creditCustomerModal').html(data);
        $('#creditCustomerModal').modal('show');
    });
}

function editCreditCustomer(id) {
    $.get('/Admin/CreditCustomerManagement/Edit/' + id, function (data) {
        $('#creditCustomerModal').html(data);
        $('#creditCustomerModal').modal('show');
    });
}

function deleteCreditCustomer(id) {
    Swal.fire({
        title: 'هل أنت متأكد من الحذف؟',
        text: 'لن تتمكن من استعادة هذا العميل مرة أخرى!',
        icon: 'warning',
        iconColor: '#dc3545',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'نعم، احذفه!',
        cancelButtonText: 'إلغاء',
        background: 'white',
        showClass: { popup: 'animate__animated animate__fadeInDown' },
        hideClass: { popup: 'animate__animated animate__fadeOutUp' },
        customClass: {
            confirmButton: 'btn btn-danger shadow-sm px-4 py-2',
            cancelButton: 'btn btn-secondary shadow-sm px-4 py-2'
        },
        buttonsStyling: false
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: 'جاري الحذف...',
                html: 'الرجاء الانتظار بينما نعالج طلبك',
                timerProgressBar: true,
                didOpen: () => { Swal.showLoading(); },
                background: 'white',
                allowOutsideClick: false
            });
            $.post('/Admin/CreditCustomerManagement/Delete', { id: id }, function (response) {
                Swal.close();
                if (response.success) {
                    Swal.fire({
                        title: 'تم الحذف!',
                        text: 'تم الحذف بنجاح.',
                        icon: 'success',
                        iconColor: '#28a745',
                        confirmButtonColor: '#28a745',
                        background: 'white',
                        showClass: { popup: 'animate__animated animate__bounceIn' },
                        customClass: { confirmButton: 'btn btn-success shadow-sm px-4 py-2' }
                    }).then(() => {
                        loadAdminContent('CreditCustomerManagement', 1);
                    });
                } else {
                    Swal.fire('خطأ!', response.message, 'error');
                }
            }).fail(function () {
                Swal.close();
                Swal.fire('خطأ!', 'فشل الاتصال بالخادم', 'error');
            });
        }
    });
}

function viewCreditCustomer(id) {
    $.get('/Admin/CreditCustomerManagement/Details/' + id, function (data) {
        Swal.fire({
            title: `<span style='color:#6a11cb'><i class='fas fa-user-credit me-2'></i>بيانات العميل</span>`,
            html: `
                <div style='text-align:right;font-size:1.1rem;'>
                    <div><strong>الاسم:</strong> <span style='color:#2575fc'>${data.name}</span></div>
                    <div><strong>الرصيد:</strong> <span style='color:#43cea2'>${data.balance}</span></div>
                    <div><strong>الآجل:</strong> <span style='color:#185a9d'>${data.credit}</span></div>
                </div>
            `,
            icon: 'info',
            showConfirmButton: true,
            confirmButtonText: 'إغلاق',
            confirmButtonColor: '#6a11cb',
            background: 'white',
            customClass: {
                confirmButton: 'btn add-credit-customer-btn px-4 py-2'
            }
        });
    });
}