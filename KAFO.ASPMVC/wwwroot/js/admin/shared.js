// Shared functionality for admin area
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
        loadAdminContent(action,1);
    });
}); 