using System;

namespace Model
{

    public class FoodCreation
    {
       public  int MealId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Portion { get; set; }
        public int GramWeight { get; set; }
        public double Calorie { get; set; }
    }
}