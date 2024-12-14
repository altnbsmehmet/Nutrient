using Microsoft.EntityFrameworkCore;

namespace Data 
{
    public class AppDbContext : DbContext 
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
    }

    }
}