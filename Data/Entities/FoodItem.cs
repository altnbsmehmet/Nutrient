using System;

namespace Data
{
    public class FoodItem 
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public string Type { get; set; }
        public int Calories { get; set; }
        public double Carbohydrate { get; set ; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double VitaminA {get; set; }
        public double Calcium { get; set; }

        public ICollection<MealFoodItem> MealFoodItems { get; set; } = new List<MealFoodItem>();
    }
}