using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReviewSystem.Data;
using RestaurantReviewSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace RestaurantReviewSystem.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Reviews/Create
        // [Authorize] - User must be logged in to add reviews
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Rating,Comment,RestaurantId")] Review review)
        {
            // Get current logged-in user
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }

            // Verify restaurant exists
            if (!_context.Restaurants.Any(r => r.Id == review.RestaurantId))
            {
                ModelState.AddModelError("RestaurantId", "Invalid restaurant");
                return RedirectToAction("Details", "Restaurants", 
                    new { id = review.RestaurantId });
            }

            if (ModelState.IsValid)
            {
                // Set the UserId to the current user
                review.UserId = user.Id;
                
                _context.Add(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Restaurants", 
                new { id = review.RestaurantId });
        }

        // GET: Reviews/Edit/5
        // [Authorize] - User must be logged in to edit reviews
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            // Check if user owns the review or is admin
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");

            if (review.UserId != user?.Id && !isAdmin)
            {
                return Forbid();  // User doesn't own this review and is not admin
            }

            return View(review);
        }

        // POST: Reviews/Edit/5
        // [Authorize] - User must be logged in to edit reviews
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rating,Comment,RestaurantId,UserId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            // Get current user
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");

            // Check ownership or admin status
            if (review.UserId != user?.Id && !isAdmin)
            {
                return Forbid();  // User doesn't own this review and is not admin
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Restaurants", 
                    new { id = review.RestaurantId });
            }

            return View(review);
        }

        // GET: Reviews/Delete/5
        // [Authorize] - User must be logged in to delete reviews
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            // Check if user owns the review or is admin
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");

            if (review.UserId != user?.Id && !isAdmin)
            {
                return Forbid();  // User doesn't own this review and is not admin
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        // [Authorize] - User must be logged in to delete reviews
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            
            if (review == null)
            {
                return NotFound();
            }

            // Get current user
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");

            // Check ownership or admin status
            // Users can delete only their own reviews
            // Admins can delete any review
            if (review.UserId != user?.Id && !isAdmin)
            {
                return Forbid();  // User doesn't own this review and is not admin
            }

            var restaurantId = review.RestaurantId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
