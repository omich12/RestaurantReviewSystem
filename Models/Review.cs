using System.ComponentModel.DataAnnotations;

namespace RestaurantReviewSystem.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Comment must be between 5 and 500 characters")]
        public string? Comment { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        // Navigation property to Restaurant
        public Restaurant? Restaurant { get; set; }

        // Foreign key to ApplicationUser (review author)
        public string? UserId { get; set; }

        // Navigation property to ApplicationUser (review author)
        public ApplicationUser? User { get; set; }
    }
}
