using System;
using Newtonsoft.Json;
using Data;

namespace Services
{
    public class USDAClient
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "YdgWdIhAtbb3b97vVIm8TE3UFL62wSefF5FbGKEu";

        public USDAClient(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        /*public async Task<List<FoodItem>> SearchFoodAsync(string query)
        {
            var url = $"https://api.nal.usda.gov/fdc/v1/foods/search?api_key={_apiKey}&query={query}";
            var response = await _httpClient.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<object>(response);
            return result.foods;
        }*/

    }
}