
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
    // Default options
    const defaults = {
        color: '#6200ee',
        size: 50,
        thickness: 3
    };

    const config = { ...defaults, ...options };

    let spinnerOverlay = document.getElementById('modern-spinner-overlay');

    if (show) {
        // Create spinner if it doesn't exist
        if (!spinnerOverlay) {
            spinnerOverlay = document.createElement('div');
            spinnerOverlay.id = 'modern-spinner-overlay';
            spinnerOverlay.className = 'spinner-overlay';

            const spinnerContainer = document.createElement('div');
            spinnerContainer.className = 'spinner-container';

            const spinner = document.createElement('div');
            spinner.className = 'modern-spinner';

            // Apply custom styles
            spinner.style.width = `${config.size}px`;
            spinner.style.height = `${config.size}px`;
            spinner.style.borderWidth = `${config.thickness}px`;
            spinner.style.borderTopColor = config.color;

            const textElement = document.createElement('div');
            textElement.className = 'spinner-text';
            textElement.textContent = message;

            spinnerContainer.appendChild(spinner);
            if (message) {
                spinnerContainer.appendChild(textElement);
            }

            spinnerOverlay.appendChild(spinnerContainer);
            document.body.appendChild(spinnerOverlay);
        } else {
            // Update existing spinner
            const spinner = spinnerOverlay.querySelector('.modern-spinner');
            const textElement = spinnerOverlay.querySelector('.spinner-text');

            if (spinner) {
                spinner.style.width = `${config.size}px`;
                spinner.style.height = `${config.size}px`;
                spinner.style.borderWidth = `${config.thickness}px`;
                spinner.style.borderTopColor = config.color;
            }

            if (textElement) {
                textElement.textContent = message;
            } else if (message) {
                const newTextElement = document.createElement('div');
                newTextElement.className = 'spinner-text';
                newTextElement.textContent = message;
                spinnerOverlay.querySelector('.spinner-container').appendChild(newTextElement);
            }
        }

        spinnerOverlay.style.display = 'flex';
    } else if (spinnerOverlay) {
        spinnerOverlay.style.display = 'none';
    }
}
