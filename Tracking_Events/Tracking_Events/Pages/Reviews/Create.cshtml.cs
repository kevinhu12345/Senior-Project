using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet(int eventId, int venueId)
        {
            Event = _context.Event.Include(e => e.Venue).SingleOrDefault(e => e.EventID == eventId);

            return Page();
        }

        [BindProperty]
        public Review Review { get; set; }
        
        public Event Event { get; set; }

        [BindProperty]
        public int EventID { get; set; }
        [BindProperty]
        public int VenueID { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Review.User = await _userManager.GetUserAsync(User);
            Review.Venue = await _context.Venue.SingleOrDefaultAsync(v => v.VenueID == VenueID);
            Review.Event = await _context.Event.SingleOrDefaultAsync(e => e.EventID == EventID);
            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}