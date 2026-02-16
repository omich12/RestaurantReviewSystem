using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReviewSystem.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Restaurant name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 100 characters")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Cuisine type is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Cuisine type must be between 2 and 50 characters")]
        public string? CuisineType { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Helper property to calculate average rating
        [NotMapped]
        public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;
    }
}
