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
    public class MealService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MealService(AppDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
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
        public async Task<string> AddMealAsync(string name)
        {
            var currentUser = await GetCurrentUserAsync();
            var existingMeal = await _context.Meal.Where(m => m.UserId == currentUser.Id && m.Name == name).FirstOrDefaultAsync();
            if (existingMeal != null) return $"The meal named '{name}' already exists.";
            var meal = new Meal { Name = name, Date = DateTime.Now, UserId = currentUser.Id };
            await _context.Meal.AddAsync(meal);
            await _context.SaveChangesAsync();
            return "The Meal named {name} created.";
        }

        public async Task<Meal> GetMealByIdAsync(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            return await _context.Meal.FirstOrDefaultAsync(m => m.Id == id && m.UserId == currentUser.Id);
        }

        public async Task<List<Meal>> GetAllMealsAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            return await _context.Meal
                .Where(m => m.UserId == currentUser.Id)
                .Include(m => m.MealFoodItems)
                .ThenInclude(mfi => mfi.FoodItem)
                .ToListAsync();
        }

        public async Task UpdateMealAsync(int id, string name)
        {
            var currentUser = await GetCurrentUserAsync();
            var meal = await _context.Meal.FirstOrDefaultAsync(m => m.Id == id && m.UserId == currentUser.Id);
            meal.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMealAsync(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            var meal = await _context.Meal.FirstOrDefaultAsync(m => m.Id == id && m.UserId == currentUser.Id);
            _context.Remove(meal);
            await _context.SaveChangesAsync();
        }

    }
}