using System;

namespace Model
{


    public class FoodSearchResponse
    {
        public int TotalHits { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<Food> Foods { get; set; }
    }


    public class Food
    {
        public string Description { get; set; }
        public List<FoodNutrient> FoodNutrients { get; set; }
        public List<FoodMeasure> FoodMeasures { get; set; }
        public string FoodCategory { get; set; }
        public string DataType { get; set; }
    }


    public class FoodMeasure
    {
        public double GramWeight { get; set; }
        public string DisseminationText { get; set; }
    }


    public class FoodNutrient
    {
        public string NutrientName { get; set; }
        public double Value { get; set; }
        public string UnitName { get; set; }
    }


}