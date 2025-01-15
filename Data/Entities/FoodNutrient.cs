namespace Data
{
    public class FoodNutrient
    {
        public int Id { get; set; }
        public string NutrientName { get; set; }
        public double Value { get; set; }
        public string UnitName { get; set; }

        public int FoodItemId { get; set; }    
        public FoodItem FoodItem { get; set; }
    }
}