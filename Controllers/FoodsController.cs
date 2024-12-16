using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Services;

[Authorize]
[Route("/foods")]
public class FoodsController : Controller
{
    private readonly FoodService _foodService;
    public FoodsController(FoodService foodService)
    {
        _foodService = foodService;
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
}