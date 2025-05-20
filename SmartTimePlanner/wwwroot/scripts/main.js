document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.querySelector(".navbar-toggle");
    const navLinks = document.querySelector(".navbar-links");

    toggleButton?.addEventListener("click", () => {
        navLinks.classList.toggle("active");
    });
});
