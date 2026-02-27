using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using RestaurantReviewSystem.Data;
using RestaurantReviewSystem.Models; 

namespace RestaurantReviewSystem.Controllers // App controller namespace
{ 
    [Route("api/[controller]")] // Route template
    [ApiController] // Enables API conventions
    public class ReviewApiController : ControllerBase // API controller class
    {
        private readonly ApplicationDbContext _context; // Database context
        
        public ReviewApiController(ApplicationDbContext context) // Constructor
        { // Start constructor
            _context = context; // Store injected context
        } 
    
        // ========================= 
        // CREATE REVIEW (POST)
        // ========================= 
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] Review review) // Create action
        {
            if (!ModelState.IsValid) // Validate request body
                return BadRequest(ModelState); // Return validation errors
            
            // Set server-side timestamp to keep client input consistent.
            review.ReviewDate = DateTime.Now; // Set review date
            
            _context.Reviews.Add(review); // Add review to context
            await _context.SaveChangesAsync(); // Persist changes
            
            return Ok(review); // Return created review
        } 
        
        // =========================
        // GET ALL REVIEWS
        // ========================= 
        [HttpGet] // GET endpoint
        public async Task<IActionResult> GetAllReviews() // List action
        { 
            var reviews = await _context.Reviews // Start query
                .Include(r => r.Restaurant) // Include restaurant data
                .Include(r => r.User) // Include user data
                .ToListAsync(); // Execute query
            
            return Ok(reviews); // Return results
        }
        
        // =========================
        // GET REVIEW BY ID
        // ========================= 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id) // Details action
        { 
            var review = await _context.Reviews // Start query
                .Include(r => r.Restaurant) // Include restaurant data
                .Include(r => r.User) // Include user data
                .FirstOrDefaultAsync(r => r.Id == id); // Find by id
        
            if (review == null) // Check not found
                return NotFound(); // Return 404
    
            return Ok(review); // Return review
        } 
    
        // =========================
        // UPDATE REVIEW
        // ========================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, Review review) // Update action
        { 
            if (id != review.Id) // Ensure ids match
                return BadRequest("ID mismatch"); // Return mismatch error
        
            var existingReview = await _context.Reviews.FindAsync(id); // Find existing
        
            if (existingReview == null) // Check not found
                return NotFound(); // Return 404
            //
            existingReview.Rating = review.Rating; // Update rating
            existingReview.Comment = review.Comment; // Update comment
            existingReview.RestaurantId = review.RestaurantId; // Update restaurant

            await _context.SaveChangesAsync(); // Persist updates
            //
            return Ok(existingReview); // Return updated review
        }

        // =========================
        // DELETE REVIEW
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id) // Delete action
        {
            var review = await _context.Reviews.FindAsync(id); // Find review
            //
            if (review == null) // Check not found
                return NotFound(); // Return 404
    
            _context.Reviews.Remove(review); // Remove from context
            await _context.SaveChangesAsync(); // Persist deletion
    
            return Ok("Review deleted successfully"); // Return success message
        }
    }
}


