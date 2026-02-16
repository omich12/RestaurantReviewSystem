using Microsoft.AspNetCore.Identity;

namespace RestaurantReviewSystem.Models
{
    /// <summary>
    /// Extended Identity user class for the application.
    /// Inherits from IdentityUser to use ASP.NET Core Identity.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        // Navigation property for user's reviews
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
