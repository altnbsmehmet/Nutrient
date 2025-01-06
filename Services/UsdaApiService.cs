using System;
using Newtonsoft.Json;
using Data;
using Model;

namespace Services
{
    public class UsdaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _usdaApiKey = "YdgWdIhAtbb3b97vVIm8TE3UFL62wSefF5FbGKEu";
        private readonly string _nutritionixApiKey = "3c6fb560a1e35f05b99de9b124409b1b";
        private readonly string _nutritionixApiId = "5cedbe56";

        public UsdaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FoodSearchResponse> SearchFoodsAsync(string keyword)
        {
            var url = $"https://api.nal.usda.gov/fdc/v1/foods/search?query={keyword}&api_key={_usdaApiKey}&pageNumber=1";
            var response = await _httpClient.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                throw;
            }
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FoodSearchResponse>(json);

            var filteredFoods = result.Foods.Where(food => 
                    food.DataType != "Branded")
                .ToList();

            result.Foods = filteredFoods;

            return result;
        }

    }
}