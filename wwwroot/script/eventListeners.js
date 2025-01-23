import { roundToTwo } from './utilities.js';
import { toggleForm } from './utilities.js';


// assigning unique ids for each button
document.addEventListener("DOMContentLoaded", () => {
    const buttons = document.querySelectorAll("button");
    buttons.forEach((button, index) => {
        button.setAttribute("data-id", `button-${index + 1}`);
    });
});




// form toggle
document.body.addEventListener('click', (event) => {
    if (event.target.matches('[data-toggle="form"]')) {
        const formType = event.target.dataset.formType;
        const buttonId = event.target.dataset.id;
        toggleForm(formType, buttonId);
        console.log("click event triggered");
    }
});




// hovering nutritional details
const foodHover = document.getElementById('foodHover');
document.body.addEventListener('mouseover', (event) => {
    if (event.target.matches('[data-hover="food"]')) {
            foodHover.classList.remove('hidden');
            fetch(`/foods?id=${event.target.dataset.foodId}`)
            .then(response => response.json())
            .then(data => {
                foodHover.innerHTML = `
                    <p>Calorie: ${data.Calorie} kcal</p>
                    <p>Portion: ${data.Portion}</p>
                    <p>GramWeight: ${data.GramWeight} g</p>
                    <p>Protein: ${data.FoodNutrients.find(fn => fn.NutrientName == "Protein")?.Value} ${data.FoodNutrients.find(fn => fn.NutrientName == "Protein")?.UnitName}</p>
                    <p>Carbonhydrate: ${data.FoodNutrients.find(fn => fn.NutrientName == "Carbohydrate, by difference")?.Value} ${data.FoodNutrients.find(fn => fn.NutrientName == "Carbohydrate, by difference")?.UnitName}</p>
                    <p>Total Fat: ${data.FoodNutrients.find(fn => fn.NutrientName == "Total lipid (fat)")?.Value} ${data.FoodNutrients.find(fn => fn.NutrientName == "Total lipid (fat)")?.UnitName}</p>
                `;
            })
            .catch(error => alert("Error fetching data: " + error));
    }
});
document.body.addEventListener('mouseout', (event) => {
    if (event.target.matches('[data-hover="food"]')) {
        foodHover.classList.add('hidden');
    }
});




// mouse coordinates
let mouseX, mouseY;
document.body.addEventListener('mousemove', (event) => {
    mouseX = event.pageX;
    mouseY = event.pageY;
    foodHover.style.left = `${mouseX + 2}px`;
    foodHover.style.top = `${mouseY + 2}px`;
});




// form submission
document.getElementById("searchForm").addEventListener("submit", function (event) {
    event.preventDefault();

    const keyword = document.getElementById("searchKeyword").value;

    fetch(`/foods/search?keyword=${keyword}`)
    .then(response => response.json())
    .then(data => {

        const resultContainer = document.getElementById("searchResults");
        resultContainer.innerHTML = "";

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





