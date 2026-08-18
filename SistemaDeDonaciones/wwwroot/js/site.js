// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function () {
    var toggle = document.getElementById('sidebarToggle');
    var sidebar = document.getElementById('appSidebar');
    var backdrop = document.getElementById('sidebarBackdrop');

    function closeSidebar() {
        sidebar.classList.remove('open');
        backdrop.classList.remove('open');
        if (toggle) toggle.setAttribute('aria-expanded', 'false');
    }

    if (toggle && sidebar && backdrop) {
        toggle.addEventListener('click', function () {
            var isOpen = sidebar.classList.toggle('open');
            backdrop.classList.toggle('open', isOpen);
            toggle.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
        });
        backdrop.addEventListener('click', closeSidebar);
    }
});
