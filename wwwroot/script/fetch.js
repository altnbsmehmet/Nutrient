document.addEventListener("DOMContentLoaded", function () {


    // Form submission event
    const searchForm = document.getElementById("searchForm");
    searchForm.addEventListener("submit", function (event) {
        event.preventDefault(); // Prevent page reload

        const keyword = document.getElementById("searchKeyword").value; // Get the keyword input value

        // Perform a POST request using Fetch API
        fetch("/foods/search", {
            method: "POST", // POST method
            headers: {
                "Content-Type": "application/json" // Send data in JSON format
            },
            body: JSON.stringify({ keyword: keyword }) // Data to send
        })
        .then(response => response.json()) // Parse the JSON response
        .then(data => {
            console.log("Response:", data);

            const resultContainer = document.getElementById("searchResults");
            resultContainer.innerHTML = ""; // Clear previous results

            // Display search results
            let i = 0;
            data.foods.forEach(food => {
                const foodElement = document.createElement("form");
                foodElement.className = "flexbox";
                foodElement.style.cssText = "flex-direction: row; height: auto; width: 75%; justify-content: space-between";
                foodElement.setAttribute("asp-action", "CreateFood");
                foodElement.setAttribute("asp-controller", "Foods");
                foodElement.setAttribute("method", "POST");

                food.foodMeasures.unshift({ gramWeight: "100", disseminationText: "standart"});
                
                // Construct the inner HTML of the result
                foodElement.innerHTML = `
                    <input type="hidden" name="foodDescription${i}" value="${food.description}" />
                    <input type="hidden" name="foodCalorie${i}" value="${food.foodNutrients.find(nutrient => nutrient.unitName === "KCAL")?.value || "-"}" />
                    <input type="hidden" name="foodGramWeight${i}" value="${food.foodMeasures[0].gramWeight}" />
                    <input type="hidden" name="foodPortion${i}" value="${food.foodMeasures[0].disseminationText}" />
                    <p id="foodDescription${i}">${food.description}</p>
                    <p id="foodCategory${i}">${food.foodCategory}</p>
                    <p id="foodCalorie${i}">${food.foodNutrients.find(nutrient => nutrient.unitName === "KCAL")?.value || "-"}</p>
                    <select id="foodPortion${i}">
                        ${food.foodMeasures.map(measure => `<option value="${measure.gramWeight}">${measure.disseminationText} (${measure.gramWeight} g)</option>`).join('')}
                    </select>
                `;
                resultContainer.appendChild(foodElement);

                const sFoodPortion = document.getElementById(`foodPortion${i}`);
                const pFoodCalorie = document.getElementById(`foodCalorie${i}`);
                const standartValue = pFoodCalorie.textContent;
                const iFoodCalorie = document.querySelector(`input[name ="foodCalorie${i}"]`);
                const iFoodGramWeight = document.querySelector(`input[name ="foodGramWeight${i}"]`);
                const iFoodPortion = document.querySelector(`input[name ="foodPortion${i}"]`);

                sFoodPortion.addEventListener('change', function() {
                    pFoodCalorie.textContent = standartValue*`${sFoodPortion.value}`/100;
                    iFoodCalorie.value = standartValue*`${sFoodPortion.value}`/100;
                    iFoodGramWeight.value = sFoodPortion.value;
                    iFoodPortion.value = food.foodMeasures.find(foodMeasure => foodMeasure.gramWeight == sFoodPortion.value).disseminationText;
                });
                i++;
            });
        })
        .catch(error => {
            // Handle errors
            console.error("Error fetching data:", error);
            alert("An error occurred while searching for foods. Please try again.");
        });

    });


});
