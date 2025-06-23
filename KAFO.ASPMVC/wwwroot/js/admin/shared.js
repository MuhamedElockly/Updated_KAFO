
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
