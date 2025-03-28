namespace Data
{
    public class Meal 
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User {get; set; }

        public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
    }
}