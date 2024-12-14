using System;

namespace Data
{
    public class MealFoodItem 
    {
        public int MealId { get; set;}
        public Meal Meal { get; set; } = null!;
        
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; } = null!;
    }
}