using Microsoft.AspNetCore.Mvc;
using Services;

[Route("/identity")]
public class IdentityController : Controller
{
    private readonly FoodService _foodService;
    private readonly IdentityService _identityService;
    public IdentityController(FoodService foodService, IdentityService identityService)
    {
        _foodService = foodService;
        _identityService = identityService;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(string userName, string password)
    {
        var result = await _identityService.SignInAsync(userName, password);
        if (result.Succeeded) return RedirectToAction("Index", "Pages");
        ViewBag.result = result;
        return View("signIn");
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(string email, string userName, string password, string firstName, string lastName, string gender, double height, double weight)
    {
        var result = await _identityService.SignUpAsync(email, userName, password, firstName, lastName, gender, height, weight);
        if (result.Succeeded) return RedirectToAction("SignIn", "Pages");
        ViewBag.result = result.Errors;
        return View("signUp");
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(string firstname, string lastname, double height, double weight)
    {
        await _identityService.Update(firstname, lastname, height, weight);
        return RedirectToAction("Profile", "Pages");
    }

}