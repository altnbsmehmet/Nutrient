using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http.Headers;
using Services;


[Route("/pages")]
public class PagesController : Controller
{
    private readonly FoodService _foodService;

    public PagesController(FoodService foodService)
    {
    _foodService = foodService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.Foods = await _foodService.GetAllFoodsAsync();
        return View("index");
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details(int id)
    {
        ViewBag.food = await _foodService.GetFoodByIdAsync(id);
        return View("viewinfo");
    }

    [HttpGet("edit")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.food = await _foodService.GetFoodByIdAsync(id);
        return View("update");
    }

}