
// sidebar menu toggle
let sidebar = document.getElementById("sidebar");
let menuToggle = document.getElementById("menuToggle");
let menuBars = document.querySelectorAll(".menu-toggle .bar");

function toggleMenu() {
    if (sidebar.style.left === "0px") {
        sidebar.style.left = "-250px";
        menuBars.forEach(function(bar) {
            bar.style.backgroundColor = "#333";
        });
    } else {
        sidebar.style.left = "0px";
        menuBars.forEach(function(bar) {
            bar.style.backgroundColor = "white";
        });
    }
}


// form toggle
document.body.addEventListener('click', (event) => {
    if (event.target.matches('[data-toggle="form"]')) {
        const formType = event.target.dataset.formType;
        const buttonId = event.target.dataset.id;
        toggleForm(formType, buttonId);
    }
});

function toggleForm(formType, buttonId) {
    const form = document.getElementById(formType);
    const button = document.querySelector(`[data-id="${buttonId}"]`);

    if (button.hasAttribute('data-meal-id')) {
        document.querySelectorAll('input[type="hidden"][name="mealId"]').forEach(input => input.value = button.dataset.mealId);
        document.querySelector('textarea[name="mealName"]').innerHTML = document.querySelector(`button[data-meal-id="${button.dataset.mealId}"]`).dataset.mealName;
    }
    if (button.hasAttribute('data-food-id')) {
        document.querySelectorAll('input[type="hidden"][name="foodId"]').forEach(input => input.value = button.dataset.foodId);
    }

    if (!button || !form) return;

    if (form.classList.contains('hidden')) {
        const buttonRect = button.getBoundingClientRect();
        form.style.left = `${buttonRect.right}px`;
        form.style.top = `${buttonRect.bottom}px`;
        form.classList.remove('hidden');
    } else form.classList.add('hidden');
}