namespace Data
{
    public class FoodItem
    {
        public int Id { get; set;}
        public string Description { get; set; }
        public string Category { get; set; }
        public string Portion { get; set; }
        public int GramWeight { get; set; }
        public double Calorie { get; set; }
        public double? Carbohydrate { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? VitaminA {get; set; }
        public double? Calcium { get; set; }


        public ICollection<FoodNutrient> FoodNutrients { get; set; } = new List<FoodNutrient>();

        public int MealId { get; set; }
        public Meal Meal { get; set; }
    }
}