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

        public override int SaveChanges()
        {
            ApplyUtcDateTimeConversion();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyUtcDateTimeConversion();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyUtcDateTimeConversion()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime) && property.CurrentValue != null)
                    {
                        var dateTime = (DateTime)property.CurrentValue;
                        property.CurrentValue = dateTime.Kind == DateTimeKind.Unspecified
                            ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
                            : dateTime.ToUniversalTime();
                    }
                }
            }
        }

    }
}