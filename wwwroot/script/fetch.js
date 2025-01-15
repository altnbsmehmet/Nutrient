
function stopPropagation(event) {
    event.stopPropagation();
}

function roundToTwo(value) {
    return Math.round(value * 100) / 100;
}

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

document.addEventListener("DOMContentLoaded", function () {

    // Form submission event
    document.getElementById("searchForm").addEventListener("submit", function (event) {
        event.preventDefault();

        const keyword = document.getElementById("searchKeyword").value;

        fetch(`/foods/search?keyword=${keyword}`)
        .then(response => response.json())
        .then(data => {

            const resultContainer = document.getElementById("searchResults");
            resultContainer.innerHTML = "";

            // display search results
            let i = 0;
            data.foods.forEach(food => {
                const foodElement = document.createElement("form");
                foodElement.className = "flexbox clickable-form ";
                foodElement.style.cssText = "flex-direction: row; height: auto; width: 75%; justify-content: space-between;";
                foodElement.setAttribute("asp-action", "CreateFood");
                foodElement.setAttribute("asp-controller", "Foods");
                foodElement.setAttribute("method", "POST");
                foodElement.setAttribute("onclick", "createFoodBySearch(event)");
                
                const standartValue = (food.foodNutrients.find(nutrient => nutrient.unitName === "KCAL")?.value || 0);
                const standarts = food.foodNutrients;
                let foodNutrients = food.foodNutrients;
                foodNutrients = JSON.parse(JSON.stringify(food.foodNutrients.filter(fn => fn.unitName !== "KCAL")));
                foodNutrients.forEach(fn => {
                    const standart = standarts.find(standart => standart.nutrientName === fn.nutrientName);
                    fn.value = roundToTwo(standart.value * food.foodMeasures[0].gramWeight / 100);
                });
                // construct the inner HTML of the result
                foodElement.innerHTML = `
                    <input type="hidden" name="foodSerialization${i}" value"">
                    <input type="hidden" name="foodDescription${i}" value="${food.description}">
                    <input type="hidden" name="foodCategory${i}" value="${food.foodCategory}">
                    <input type="hidden" name="foodPortion${i}" value="${food.foodMeasures[0].disseminationText}">
                    <input type="hidden" name="foodGramWeight${i}" value="${food.foodMeasures[0].gramWeight}">
                    <input type="hidden" name="foodCalorie${i}" value="${roundToTwo(standartValue * ((food.foodMeasures[0]?.gramWeight || 100) / 100))}" />
                    <p id="foodDescription${i}">${food.description}</p>
                    <p id="foodCategory${i}">${food.foodCategory}</p>
                    <p id="foodCalorie${i}">${roundToTwo(standartValue * food.foodMeasures[0].gramWeight / 100)}</p>
                    <select id="foodPortion${i}" onclick="stopPropagation(event)">
                        ${food.foodMeasures.map(measure => `<option value="${measure.gramWeight}">${measure.disseminationText} ${measure.disseminationText === "gram" ? "" : `(${measure.gramWeight} g)`}</option>`).join('')}
                    </select>
                    <input type="number" name="foodCustom${i}" style="width: 50px" onclick="stopPropagation(event)">
                `;
                resultContainer.appendChild(foodElement);

                const sFoodPortion = document.getElementById(`foodPortion${i}`);
                let previousFoodPortion = sFoodPortion.value;
                const iFoodSerialization = document.querySelector(`input[name = "foodSerialization${i}"]`);
                iFoodSerialization.value = JSON.stringify(foodNutrients);
                const iFoodCustom = document.querySelector(`input[name ="foodCustom${i}"]`);
                const iFoodCalorie = document.querySelector(`input[name ="foodCalorie${i}"]`);
                const iFoodGramWeight = document.querySelector(`input[name ="foodGramWeight${i}"]`);
                const iFoodPortion = document.querySelector(`input[name ="foodPortion${i}"]`);
                const pFoodCalorie = document.getElementById(`foodCalorie${i}`);
                sFoodPortion.addEventListener('change', () => {
                    if (previousFoodPortion !== sFoodPortion.value) {
                        iFoodCustom.value = "";
                        previousFoodPortion = sFoodPortion.value;
                    }
                    foodNutrients.forEach(fn => {
                        const standart = standarts.find(standart => standart.nutrientName === fn.nutrientName);
                        fn.value = roundToTwo(standart.value * sFoodPortion.value / 100);
                    });
                    iFoodSerialization.value = JSON.stringify(foodNutrients);
                    iFoodCalorie.value = roundToTwo(standartValue * sFoodPortion.value / 100);
                    iFoodGramWeight.value = sFoodPortion.value;
                    iFoodPortion.value = food.foodMeasures.find(foodMeasure => foodMeasure.gramWeight == sFoodPortion.value).disseminationText;
                    pFoodCalorie.textContent = roundToTwo(standartValue * sFoodPortion.value / 100);
                });

                const changeEvent = new Event('change', {
                    bubbles: true,
                    cancelable: true,
                });
                iFoodCustom.addEventListener('input', () => {
                    if (iFoodCustom.value > 0) {
                        sFoodPortion.dispatchEvent(changeEvent);
                        if (iFoodPortion.value === "gram") {
                            foodNutrients.forEach(fn => {
                                const standart = standarts.find(standart => standart.nutrientName === fn.nutrientName);
                                fn.value = roundToTwo(standart.value * iFoodCustom.value / 100);
                            });
                            iFoodSerialization.value = JSON.stringify(foodNutrients);
                            iFoodCalorie.value = roundToTwo((standartValue * iFoodCustom.value) / 100);
                            iFoodGramWeight.value = iFoodCustom.value;
                            pFoodCalorie.textContent = iFoodCalorie.value;
                        } else  {
                            foodNutrients.forEach(fn => fn.value = roundToTwo(fn.value * iFoodCustom.value));
                            iFoodSerialization.value = JSON.stringify(foodNutrients);
                            iFoodCalorie.value = roundToTwo(iFoodCalorie.value * iFoodCustom.value);
                            iFoodGramWeight.value = iFoodGramWeight.value * iFoodCustom.value;
                            pFoodCalorie.textContent = iFoodCalorie.value;
                        }
                    }
                });

                i++;
            });
        })
        .catch(error => {
            console.error("Error fetching data:", error);
            alert("An error occurred while searching for foods. Please try again.");
        });

    });


});