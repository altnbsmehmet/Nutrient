
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

function toggleMealForm() {
    const form = document.getElementById('createMealForm');
    const button = document.getElementById('showMealFormButton');

    console.log('Form visibility:', form.style.display); 
    if (form.style.display == 'none' || form.style.display == '') {
        const buttonRect = button.getBoundingClientRect();

        form.style.left = `${buttonRect.right}px`;
        form.style.top = `${buttonRect.bottom}px`;
        form.style.display = 'block';
    } else {
        form.style.display = 'none';
    }
}

function toggleFoodForm(id) {
    if (id >= 0) {
        const form = document.getElementById(`createFoodForm${id}`);
        const button = document.getElementById(`showFoodFormButton${id}`);
        if (form.style.display == 'none' || form.style.display == '') {
            const buttonRect = button.getBoundingClientRect();
    
            form.style.left = `${buttonRect.right}px`;
            form.style.top = `${buttonRect.bottom}px`;
            form.style.display = 'block';
        } else  form.style.display = 'none';
    } else {
        const form = document.getElementById(`foodOperations${id}`);
        const button = document.getElementById(`showFoodFormButton${id}`);
        if (form.style.display == 'none' || form.style.display == '') {
            const buttonRect = button.getBoundingClientRect();
    
            form.style.left = `${buttonRect.right}px`;
            form.style.top = `${buttonRect.bottom}px`;
            form.style.display = 'block';
        } else form.style.display = 'none';
    }
}

function toggleUpdateFoodForm(id) {
    const form = document.getElementById(`updateFoodForm${id}`);
    const button = document.getElementById(`showFoodFormButton${id}`);
    if (form.style.display == 'none' || form.style.display == '') {
        const buttonRect = button.getBoundingClientRect();

        form.style.left = `${buttonRect.right}px`;
        form.style.top = `${buttonRect.bottom}px`;
        form.style.display = 'block';
    } else form.style.display = 'none';
}