

// sidebar toggle
const sidebar = document.getElementById("sidebar");
const menuBars = document.querySelectorAll(".menu-toggle .bar");
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




// stop any event when clicked
function stopPropagation(event) {
    event.stopPropagation();
}




// creating food via search
function createFoodBySearch(event) {
    event.preventDefault();

    const inputs = event.currentTarget.querySelectorAll('input');
    const requestBody = {
        FoodNutrients: JSON.parse(inputs[0].value),
        MealId: document.querySelector('input[type="hidden"][name="mealId"]').value,
        Description: inputs[1].value,
        Category: inputs[2].value,
        Portion: inputs[3].value,
        GramWeight: (inputs[6].value != "" && inputs[6].value != null) && inputs[6].value > 0 ? inputs[6].value : inputs[4].value,
        Calorie: inputs[5].value
    };

    fetch("/foods/create", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(requestBody)
    })
    .then(response => response.text())
    .then(data => {
        window.location.reload();
        alert(data);
    })
    .catch(error => alert("Error fetching data: " + error));
}