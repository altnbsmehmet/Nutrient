using System;

namespace Data
{
    public class Meal 
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public ICollection<MealFoodItem> MealFoodItems { get; set; } = new List<MealFoodItem>();
    }
}