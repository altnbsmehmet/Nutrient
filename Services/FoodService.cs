using System;
using System.CodeDom.Compiler;
using System.Security.Cryptography.X509Certificates;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Services 
{
    public class FoodService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FoodService(AppDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
        }

        public async Task<string> AddFoodAsync(int mealId, string name, int calorie, string type)
        {
            var currentUser = await GetCurrentUserAsync();

            var foods = await GetAllFoodsAsync();
            foreach (var foodObject in foods)
            {
                if (foodObject.Name == name) return $"The {name} name already exists.";
            }

            var meal = await _context.Meal
                .Include(m => m.MealFoodItems)
                .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == currentUser.Id);
            if(meal == null) return "Meal not found.";

            var food = new FoodItem { Name = name, Calories = calorie, Type = type};
            var mealFoodItem = new MealFoodItem { Meal = meal, FoodItem = food};
            meal.MealFoodItems.Add(mealFoodItem);
            await _context.SaveChangesAsync();
            return $"The FoodItem named {name} created.";
        }

        public async Task<FoodItem> GetFoodByIdAsync(int id)
        {
            return await _context.FoodItem.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<FoodItem>> GetAllFoodsAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            return await _context.FoodItem
                .Include(f => f.MealFoodItems)
                .ThenInclude(mfi => mfi.Meal)
                .Where(f => f.MealFoodItems.Any(mfi => mfi.Meal.UserId == currentUser.Id))
                .ToListAsync();
        }

        public async Task UpdateFoodAsync(int id, string name, int calorie, string type)
        {
            var food = await _context.FoodItem.FirstOrDefaultAsync(f => f.Id == id);
            food.Name = name;
            food.Calories = calorie;
            food.Type = type;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFoodAsync(int id)
        {
            var food = await _context.FoodItem.FirstOrDefaultAsync(f => f.Id == id);
            _context.Remove(food);
            await _context.SaveChangesAsync();
        }

    }
}