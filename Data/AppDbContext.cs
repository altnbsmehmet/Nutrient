using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data 
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<FoodItem> FoodItem { get; set; }
        public DbSet<Meal> Meal { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealFoodItem>()
                .HasKey(mf => new { mf.MealId, mf.FoodItemId });

            modelBuilder.Entity<MealFoodItem>()
                .HasOne(mf => mf.Meal)
                .WithMany(m => m.MealFoodItems)
                .HasForeignKey(mf => mf.MealId);

            modelBuilder.Entity<MealFoodItem>()
                .HasOne(mf => mf.FoodItem)
                .WithMany(f => f.MealFoodItems)
                .HasForeignKey(mf => mf.FoodItemId);

            modelBuilder.Entity<Meal>()
                .HasOne(m => m.User)
                .WithMany(u => u.Meals)
                .HasForeignKey(m => m.UserId);
        }

    }
}