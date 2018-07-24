using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Account.Manage
{
    public class ManageVenuesDeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageVenuesDeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Venue Venue { get; set; }
        [BindProperty]
        public string VenueType { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Venue = await _context.Venue.Include(v => v.User).SingleOrDefaultAsync(v => v.VenueID == id);
            VenueType = ((VenueType)Venue.VenueType).ToString();

            if (Venue == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Venue = await _context.Venue.FindAsync(id);

            if (Venue != null)
            {
                var review = await _context.Review.Include(r => r.Venue).Where(r => r.Venue == Venue).ToArrayAsync();
                _context.Review.RemoveRange(review);
                _context.Venue.Remove(Venue);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ManageVenues", new { statusMessage = "Successfully deleted Venue: " + Venue.VenueName });
        }
    }
}
