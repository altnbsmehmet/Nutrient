using System;

namespace Model
{

    public class NutritionixResponse
    {
        public List<CommonFood> Common { get; set; }
    }

    public class CommonFood
    {
        public string Food_Name { get; set; }
        public Photo Photo { get; set; }
        public double Serving_Qty { get; set; }
        public string Serving_Unit { get; set; }
    }

    public class Photo
    {
        public string Thumb { get; set; }
    }

}