using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Data;
using Model;

namespace Services 
{
    public class FoodService
    {
        private readonly AppDbContext _context;
        private readonly IdentityService _identityService;
        private readonly UsdaApiService _usdaApiService;
        public FoodService(AppDbContext context, IdentityService identityService, UsdaApiService usdaApiService)
        {
            _context = context;
            _identityService = identityService;
            _usdaApiService = usdaApiService;
        }

        public async Task<string> AddFoodAsync(FoodCreation foodCreation)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();

            var meal = await _context.Meal
                .Include(m => m.FoodItems)
                .FirstOrDefaultAsync(m => m.Id == foodCreation.MealId && m.UserId == currentUser.Id);
            if(meal == null) return "Meal not found.";

            var foodItem = new FoodItem { 
                FoodNutrients = foodCreation.FoodNutrients.Select(fn => new Data.FoodNutrient { 
                    NutrientName = fn.NutrientName,
                    Value = fn.Value,
                    UnitName = fn.UnitName
                    }).ToList(),
                Description = foodCreation.Description, 
                Category = foodCreation.Category, 
                Portion = foodCreation.Portion, 
                GramWeight = foodCreation.GramWeight, 
                Calorie = foodCreation.Calorie};
            meal.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();
            return $"The food with description '{foodCreation.Description}' created and assigned to meal named '{(await _context.Meal.FirstOrDefaultAsync(meal => meal.Id == foodCreation.MealId))?.Name}'.";
        }

        public async Task<string> GetFoodByIdAsync(int id)
        {
            var food = JsonConvert.SerializeObject(await _context.FoodItem.Include(fi => fi.FoodNutrients).FirstOrDefaultAsync(f => f.Id == id), Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Prevents infinite loops
            });
            return food;
        }

        public async Task<List<FoodItem>> GetAllFoodsAsync()
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            return await _context.FoodItem
                .Where(f => f.Meal.UserId == currentUser.Id)
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

        public async Task<FoodSearchResponse> SearchFoodsAsync(string keyword)
        {
            return await _usdaApiService.SearchFoodsAsync(keyword);
        }

    }
}