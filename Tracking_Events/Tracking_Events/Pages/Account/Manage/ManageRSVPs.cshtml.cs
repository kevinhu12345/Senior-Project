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
    public class ManageRSVPsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;

        public ManageRSVPsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<RSVP> RSVPs { get;set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string CurrentVenurSearch { get; set; }

        public async Task OnGetAsync(string statusMessage, string searchVenue)
        {
            StatusMessage = statusMessage;
            _user = _userManager.GetUserAsync(User).Result;

            if (!String.IsNullOrEmpty(searchVenue))
            {
                CurrentVenurSearch = searchVenue;
                RSVPs = await _context.RSVP.Include(r => r.User).Include(r => r.Event).ThenInclude(e => e.Venue).Where(r => r.User.Id == _user.Id && r.Event.EndTime > TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, _user.TimeZone).ToUniversalTime() && r.Event.Venue.VenueName.Contains(searchVenue)).OrderBy(r => r.Event.EventName).ToListAsync();
            }
            else
            {
                RSVPs = await _context.RSVP.Include(r => r.User).Include(r => r.Event).ThenInclude(e => e.Venue).Where(r => r.User.Id == _user.Id && r.Event.EndTime > TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, _user.TimeZone).ToUniversalTime()).OrderBy(r => r.Event.EventName).ToListAsync();
            }
        }
    }
}
