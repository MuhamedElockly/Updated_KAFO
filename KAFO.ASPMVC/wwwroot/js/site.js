// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Handle profile link click
$('.sidebar-link[data-action="Profile"]').click(function(e) {
    e.preventDefault();
    $.get('/Admin/Profile', function(response) {
        $('#mainContent').html(response);
    });
});
