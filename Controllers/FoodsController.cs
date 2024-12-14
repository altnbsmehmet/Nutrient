using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services;

[Route("/foods")]
public class FoodsController : Controller
{
    private readonly FoodService _foodService;
    public FoodsController(FoodService foodService)
    {
        _foodService = foodService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateFood(string name, int calorie)
    {
        await _foodService.AddFoodAsync(name, calorie);
        return RedirectToAction("Index", "Pages");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateFood(int id, string name, int calorie)
    {
        await _foodService.UpdateFoodAsync(id, name, calorie);
        return RedirectToAction("Index", "Pages");
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteFood(int id)
    {
        await _foodService.DeleteFoodAsync(id);
        return RedirectToAction("Index", "Pages");
    }
}