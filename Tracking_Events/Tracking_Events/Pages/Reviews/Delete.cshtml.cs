using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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
                .Include(r => r.Venue).SingleOrDefaultAsync(m => m.UserID == userId || m.EventID == eventId || m.VenueID == venueId);

            if (Review == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            object[] o = { Review.EventID, Review.VenueID, Review.UserID };
            Review = await _context.Review.FindAsync(o);

            if (Review != null)
            {
                _context.Review.Remove(Review);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
