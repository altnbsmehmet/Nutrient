using Microsoft.EntityFrameworkCore;
using Data;

namespace Services
{
    public class MealService
    {
        private readonly AppDbContext _context;
        private readonly IdentityService _identityService;
        public MealService(AppDbContext context, IdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        
        public async Task<string> AddMealAsync(string name)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            var existingMeal = await _context.Meal.Where(m => m.UserId == currentUser.Id && m.Name == name).FirstOrDefaultAsync();
            if (existingMeal != null) return $"The meal named '{name}' already exists.";
            var meal = new Meal { Name = name, Date = DateTime.Now, UserId = currentUser.Id };
            await _context.Meal.AddAsync(meal);
            await _context.SaveChangesAsync();
            return $"The Meal named '{name}' created.";
        }

        public async Task<Meal> GetMealByIdAsync(int id)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            return await _context.Meal
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == currentUser.Id);
        }

        public async Task<List<Meal>> GetAllMealsAsync()
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            return await _context.Meal
                .Where(m => m.UserId == currentUser.Id)
                .Include(m => m.FoodItems)
                .ToListAsync();
        }

        public async Task UpdateMealAsync(int mealId, string mealName)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            var meal = await _context.Meal.FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == currentUser.Id);
            meal.Name = mealName;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMealAsync(int mealId)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            var meal = await _context.Meal.FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == currentUser.Id);
            _context.Remove(meal);
            await _context.SaveChangesAsync();
        }

    }
}