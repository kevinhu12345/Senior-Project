using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Account.Manage
{
    public class ManageReviewsEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageReviewsEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; }

        public async Task<IActionResult> OnGetAsync(int eventId, int venueId, string userId)
        {
            Review = await _context.Review
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Venue).SingleOrDefaultAsync(m => m.UserID == userId && m.EventID == eventId && m.VenueID == venueId);

            if (Review == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _context.Review.Update(Review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToPage("./ManageReviews");
        }
    }
}
