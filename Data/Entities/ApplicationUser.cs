using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
}