using Microsoft.AspNetCore.Mvc;
using System;
using Services;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[Route("/meals")]
public class MealsController : Controller
{
    private readonly MealService _mealService;
    private readonly FoodService _foodService;

    public MealsController(FoodService foodService, MealService mealService)
    {
        _foodService = foodService;
        _mealService = mealService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMeal(string name)
    {
        await _mealService.AddMealAsync(name);
        return RedirectToAction("Meals", "Pages");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateMeal(int mealId, string mealName)
    {
        await _mealService.UpdateMealAsync(mealId, mealName);
        return RedirectToAction("Meals", "Pages");
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteMeal(int mealId)
    {
        await _mealService.DeleteMealAsync(mealId);
        return RedirectToAction("Meals", "Pages");
    }

}