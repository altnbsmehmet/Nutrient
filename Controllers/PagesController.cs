using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http.Headers;
using Services;


[Route("/")]
public class PagesController : Controller
{
    private readonly FoodService _foodService;

    public PagesController(FoodService foodService)
    {
    _foodService = foodService;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View("signIn");
    }

    [HttpGet("signUp")]
    public IActionResult SignUp()
    {
        return View("signUp");
    }

    [Authorize]
    [HttpGet("pages")] 
    public async Task<IActionResult> Index()
    {
        ViewBag.Foods = await _foodService.GetAllFoodsAsync();
        return View("index");
    }

    [Authorize]
    [HttpGet("pages/details")] 
    public async Task<IActionResult> Details(int id)
    {
        ViewBag.food = await _foodService.GetFoodByIdAsync(id);
        return View("viewinfo");
    }

    [Authorize]
    [HttpGet("pages/edit")] 
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.food = await _foodService.GetFoodByIdAsync(id);
        return View("update");
    }

}