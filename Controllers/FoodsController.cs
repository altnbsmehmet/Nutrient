using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;
using Model;

//[Authorize]
[Route("/foods")]
public class FoodsController : Controller
{
    private readonly FoodService _foodService;
    public FoodsController(FoodService foodService)
    {
        _foodService = foodService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFoodById(int id)
    {
        var result = await _foodService.GetFoodByIdAsync(id);
        return Ok(result);
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

    [HttpGet("search")]
    public async Task<IActionResult> SearchFoods(string keyword)
    {
        return Ok(await _foodService.SearchFoodsAsync(keyword));
    }
    
}