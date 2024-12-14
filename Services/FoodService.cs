using System;
using System.CodeDom.Compiler;
using System.Security.Cryptography.X509Certificates;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services {
    public class FoodService
    {
        private readonly AppDbContext _context;
        Random random = new Random();
        public FoodService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddFoodAsync(string name, int calorie)
        {
            var foods = await GetAllFoodsAsync();
            foreach (var foodObject in foods)
            {
                if (foodObject.Name == name) return $"The {name} name already exists.";
            }
            var food = new FoodItem { Name = name, Calories = calorie};
            await _context.FoodItem.AddAsync(food);
            await _context.SaveChangesAsync();
            return "The FoodItem named {name} created.";
        }

        public async Task<FoodItem> GetFoodByIdAsync(int id)
        {
            return await _context.FoodItem.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<FoodItem>> GetAllFoodsAsync()
        {
            return await _context.FoodItem.ToListAsync();
        }

        public async Task UpdateFoodAsync(int id, string name, int calorie)
        {
            var food = await _context.FoodItem.FirstOrDefaultAsync(f => f.Id == id);
            food.Name = name;
            food.Calories = calorie;
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