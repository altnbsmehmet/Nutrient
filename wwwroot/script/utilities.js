

// rounding for two fractional digits
export function roundToTwo(value) {
    return Math.round(value * 100) / 100;
}




// form toggle
export function toggleForm(formType, buttonId) {
    console.log("toggleform function triggered");
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




// food creation by search
export function createFoodBySearch(event) {
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