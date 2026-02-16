using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantReviewSystem.Models;

namespace RestaurantReviewSystem.Data
{
    /// <summary>
    /// Database context for the Restaurant Review System.
    /// Uses IdentityDbContext to include Identity tables (Users, Roles, etc.).
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Restaurant-Review relationship (one-to-many)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Restaurant)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Review-ApplicationUser relationship (one-to-many)
            // When a user is deleted, their reviews are deleted
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial restaurant data
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1,
                    Name = "The Italian Kitchen",
                    Location = "Downtown",
                    CuisineType = "Italian",
                    CreatedDate = new DateTime(2026, 2, 16, 0, 0, 0, DateTimeKind.Utc)
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Spice Route",
                    Location = "Midtown",
                    CuisineType = "Indian",
                    CreatedDate = new DateTime(2026, 2, 16, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Note: Review seeding would require user IDs, so reviews are created through the application
        }
    }
}
