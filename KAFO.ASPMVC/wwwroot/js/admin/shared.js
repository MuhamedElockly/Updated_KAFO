function loadAdminContent(action, pageNumber) {
    // Show loading indicator
    $('#mainContent').html('<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>');

    // Construct URL with optional page number
    let url = `/Admin/Admin/${action}`;
    if (pageNumber) {
        url += `?page=${pageNumber}`;
    }

    // Load the partial view via AJAX
    $.ajax({
        url: url,
        type: 'GET',
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
        },
        error: function (error) {
            $('#mainContent').html('<div class="alert alert-danger">Error loading content</div>');
        }
    });
}

// Handle sidebar navigation
$(document).ready(function () {
    $('.sidebar-link').on('click', function (e) {
        e.preventDefault();

        // Remove active class from all links
        $('.sidebar-link').parent().removeClass('active');
        // Add active class to clicked link
        $(this).parent().addClass('active');

        const action = $(this).data('action');
        loadAdminContent(action, 1);
    });
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
        backdrop: `
            rgba(220,53,69,0.1)
            url("/images/trash-icon.gif")
            center top
            no-repeat
        `,
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        },
        customClass: {
            confirmButton: 'btn btn-danger shadow-sm px-4 py-2 me-2', // Added me-2 for margin
            cancelButton: 'btn btn-secondary shadow-sm px-4 py-2 ms-2', // Added ms-2 for margin
            actions: 'swal2-actions-custom'
        },
        buttonsStyling: false
    }).then((result) => {
        if (result.isConfirmed) {
            // Show processing dialog
            Swal.fire({
                title: settings.processingText,
                html: settings.waitingText,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading()
                },
                background: 'white',
                backdrop: `
                    rgba(0,0,0,0.5)
                    center
                    no-repeat
                `,
                allowOutsideClick: false
            });

            // Prepare AJAX data
            const requestData = {
                id: settings.itemId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                ...settings.ajaxData
            };

            // Execute AJAX request
            $.ajax({
                url: settings.actionUrl,
                type: 'POST',
                data: requestData,
                success: function (response) {
                    Swal.close();
                    if (response.success) {
                        Swal.fire({
                            title: 'تم!',
                            confirmButtonText: "موافق",
                            text: settings.successText,
                            icon: 'success',
                            iconColor: '#28a745',
                            confirmButtonColor: '#28a745',
                            background: 'white',
                            showClass: {
                                popup: 'animate__animated animate__bounceIn'
                            },
                            customClass: {
                                confirmButton: 'btn btn-success shadow-sm px-4 py-2'
                            }
                        }).then(() => {
                            if (settings.callback) {
                                settings.callback(response);
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'خطأ!',
                            text: response.message || 'حدث خطأ أثناء المعالجة',
                            icon: 'error',
                            confirmButtonText: 'موافق'
                        });
                    }
                },
                error: function (xhr) {
                    Swal.close();
                    Swal.fire({ title: 'خطأ!', text: xhr.responseJSON?.message || settings.errorText, confirmButtonText: "موافق" });
                }
            });
        }
    });
}