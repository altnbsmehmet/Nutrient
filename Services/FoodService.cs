using System;
using System.CodeDom.Compiler;
using System.Security.Cryptography.X509Certificates;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using Model;

namespace Services 
{
    public class FoodService
    {
        private readonly AppDbContext _context;
        private readonly IdentityService _identityService;
        public FoodService(AppDbContext context, IdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<string> AddFoodAsync(FoodCreation foodCreation)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();

            var foods = await GetAllFoodsAsync();
            foreach (var foodObject in foods)
            {
                if (foodObject.Description == foodCreation.Description) return $"The {foodCreation.Description} name already exists.";
            }

            var meal = await _context.Meal
                .Include(m => m.MealFoodItems)
                .FirstOrDefaultAsync(m => m.Id == foodCreation.MealId && m.UserId == currentUser.Id);
            if(meal == null) return "Meal not found.";

            var food = new FoodItem { Description = foodCreation.Description, Category = foodCreation.Category, Portion = foodCreation.Portion, GramWeight = foodCreation.GramWeight, Calorie = foodCreation.Calorie};
            var mealFoodItem = new MealFoodItem { Meal = meal, FoodItem = food};
            meal.MealFoodItems.Add(mealFoodItem);
            await _context.SaveChangesAsync();
            return $"The food with description '{foodCreation.Description}' created and assigned to meal named '{(await _context.Meal.FirstOrDefaultAsync(meal => meal.Id == foodCreation.MealId))?.Name}'.";
        }

        public async Task<FoodItem> GetFoodByIdAsync(int id)
        {
            return await _context.FoodItem.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<FoodItem>> GetAllFoodsAsync()
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            return await _context.FoodItem
                .Include(f => f.MealFoodItems)
                .ThenInclude(mfi => mfi.Meal)
                .Where(f => f.MealFoodItems.Any(mfi => mfi.Meal.UserId == currentUser.Id))
                .ToListAsync();
        }

        public async Task UpdateFoodAsync()
        {
            
        }

        public async Task DeleteFoodAsync(int foodId)
        {
            var food = await _context.FoodItem.FirstOrDefaultAsync(f => f.Id == foodId);
            _context.Remove(food);
            await _context.SaveChangesAsync();
        }

    }
}