using System;

namespace Data
{
    public class FoodItem 
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public int Calories { get; set; }
        public decimal Carbohydrate { get; set ; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal VitaminA {get; set; }
        public decimal Calcium { get; set; }

        public ICollection<MealFoodItem> MealFoodItems { get; set; } = new List<MealFoodItem>();
    }
}