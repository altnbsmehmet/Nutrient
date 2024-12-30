$(document).ready(function () {
    // Arama formu gönderildiğinde çalışacak
    $("#searchForm").on("submit", function (event) {
        event.preventDefault(); // Sayfanın yeniden yüklenmesini engelle

        const keyword = $("#searchKeyword").val(); // Kullanıcının girdiği anahtar kelime
        
        // AJAX ile POST isteği gönder
        $.ajax({
            url: "/foods/search", // Controller'daki endpoint
            type: "POST", // POST methodu
            contentType: "application/json", // JSON formatında veri gönderiyoruz
            data: JSON.stringify({ keyword: keyword }), // Gönderilecek veri
            success: function (response) {
                // Başarılı istek sonrası işlem
                console.log("Response:", response);

                const resultContainer = $("#searchResults");
                resultContainer.empty(); // Önceki sonuçları temizle
                
                // Dönen verilerle arama sonuçlarını göster
                response.foods.forEach(food => {
                    const foodElement = $(`
                        <div class="flexbox" style="flex-direction: row; height: auto; width: 75%; justify-content: space-between">
                            <p>${food.description}</p>
                            <p>${food.foodCategory}</p>
                            <p>${food.foodNutrients.find(nutrient => nutrient.unitName == "KCAL")?.value || "-"}</p>
                        </div>
                    `);
                    resultContainer.append(foodElement);
                });
            },
            error: function (error) {
                // Hata durumunda işlem
                console.error("Error fetching data:", error);
                alert("An error occurred while searching for foods. Please try again.");
            }
        });
    });
});