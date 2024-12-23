using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http.Headers;
using Services;


[Route("/pages")]
public class PagesController : Controller
{
    private readonly FoodService _foodService;
    private readonly MealService _mealService;
    private readonly IdentityService _identityService;

    public PagesController(FoodService foodService, MealService mealService, IdentityService identityService)
    {
        _foodService = foodService;
        _mealService = mealService;
        _identityService = identityService;
    }

    [HttpGet("/")]
    public IActionResult SignIn()
    {
        return View("signIn");
    }

    [HttpGet("/signUp")]
    public IActionResult SignUp()
    {
        return View("signUp");
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        ViewBag.User = await _identityService.GetCurrentUserAsync();
        return View("profile");
    }

    [Authorize]
    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        ViewBag.Foods = await _foodService.GetAllFoodsAsync();
        return View("index");
    }

    [Authorize]
    [HttpGet("dashboards")]
    public async Task<IActionResult> Dashboards()
    {
        return View("dashboards");
    }

    [Authorize]
    [HttpGet("meals")] 
    public async Task<IActionResult> Meals()
    {
        ViewBag.Meals = await _mealService.GetAllMealsAsync();
        return View("meals");
    }

    [Authorize]
    [HttpGet("signout")]
    public async Task<IActionResult> SignOut()
    {
        await _identityService.SignOutAsync();
        return RedirectToAction("SignIn", "Pages");
    }

}