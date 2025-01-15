using Newtonsoft.Json;
using Model;

namespace Services
{
    public class UsdaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string usdaApiKey = "YdgWdIhAtbb3b97vVIm8TE3UFL62wSefF5FbGKEu";

        public UsdaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FoodSearchResponse> SearchFoodsAsync(string keyword)
        {
            var url = $"https://api.nal.usda.gov/fdc/v1/foods/search?query={keyword}&api_key={usdaApiKey}&pageNumber=1";
            var response = await _httpClient.GetAsync(url);
            var result = JsonConvert.DeserializeObject<FoodSearchResponse>(await response.Content.ReadAsStringAsync());

            result.Foods = result.Foods
                .Where(food => food.DataType != "Branded")
                .Take(10)
                .ToList();
            foreach (var food in result.Foods) {
                food.FoodMeasures.Add(new FoodMeasure {  GramWeight = 0, DisseminationText ="gram" });
                food.FoodMeasures.RemoveAll(f => f.DisseminationText == "Quantity not specified");
            }

            return result;
        }

    }
}