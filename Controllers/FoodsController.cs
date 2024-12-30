using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Services;
using System.Text.Json;
using Model;


[Route("/foods")]
public class FoodsController : Controller
{
    private readonly FoodService _foodService;
    private readonly UsdaApiService _usdaApiService;
    public FoodsController(FoodService foodService, UsdaApiService usdaApiService)
    {
        _foodService = foodService;
        _usdaApiService = usdaApiService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateFood(int mealId, string name, int calorie, string type)
    {
        await _foodService.AddFoodAsync(mealId, name, calorie, type);
        return RedirectToAction("Meals", "Pages");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateFood(int id, string name, int calorie, string type)
    {
        await _foodService.UpdateFoodAsync(id, name, calorie, type);
        return RedirectToAction("Meals", "Pages");
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteFood(int id)
    {
        await _foodService.DeleteFoodAsync(id);
        return RedirectToAction("Meals", "Pages");
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchFoods()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var payload = JsonDocument.Parse(body).RootElement;

            if (!payload.TryGetProperty("keyword", out JsonElement keywordElement) || keywordElement.ValueKind != JsonValueKind.String)
            {
                return BadRequest("Keyword required.");
            }

            string keyword = keywordElement.GetString();

            var result = await _usdaApiService.SearchFoodsAsync(keyword);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
            return StatusCode(500, "An internal server error occured. Please try again later.");
        }
    }
    
}