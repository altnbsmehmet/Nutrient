using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Services;
using System.Text.Json;
using Model;
using Data;

//[Authorize]
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

    [HttpGet]
    public async Task<IActionResult> GetFoodById(int id)
    {
        var result = await _foodService.GetFoodByIdAsync(id);
        Console.WriteLine(result);
        return Json(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateFood([FromBody] FoodCreation foodCreation)
    {
        return Ok(await _foodService.AddFoodAsync(foodCreation));
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateFood()
    {
        return View();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteFood(int foodId)
    {
        await _foodService.DeleteFoodAsync(foodId);
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
            result.Foods = result.Foods.Take(10).ToList();
            foreach (var food in result.Foods) {
                food.FoodMeasures.Add(new FoodMeasure {  GramWeight = 0, DisseminationText ="gram" });
                food.FoodMeasures.RemoveAll(f => f.DisseminationText == "Quantity not specified");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
            return StatusCode(500, "An internal server error occured. Please try again later.");
        }
    }
    
}