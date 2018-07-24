using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Account.Manage
{
    public class ManageReviewsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageReviewsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Review> Review { get;set; }

        public string CurrentVenueSearch { get; set; }

        public async Task OnGetAsync(string searchVenue)
        {
            if (!String.IsNullOrEmpty(searchVenue))
            {
                CurrentVenueSearch = searchVenue;

                Review = Review = await _context.Review
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Venue).Where(r => r.User.Id == _userManager.GetUserId(User) && r.Venue.VenueName.Contains(searchVenue)).ToListAsync();
            }
            else
            {
                Review = await _context.Review
                    .Include(r => r.Event)
                    .Include(r => r.User)
                    .Include(r => r.Venue).Where(r => r.User.Id == _userManager.GetUserId(User)).ToListAsync();
            }
        }
    }
}
