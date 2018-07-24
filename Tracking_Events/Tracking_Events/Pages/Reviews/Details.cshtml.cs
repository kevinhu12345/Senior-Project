using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        //Created to get single event and use that to get user data
        public Review Review { get; set; }

        public IActionResult OnGet(int eventId, int venueId, string userId)
        {
            //Used to get Parent and Foreign tables
            Review = _context.Review.Include(ev => ev.Venue).ThenInclude(v => v.User).Include(ev => ev.Event).Where(r => r.EventID == eventId && r.VenueID == venueId && r.UserID == userId).SingleOrDefault();

            if (Review == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
