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
    public class ManageEventsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;

        public ManageEventsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Event> Event { get;set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string CurrentEventSearch { get; set; }

        public async Task OnGetAsync(string statusMessage, string searchEvent)
        {
            StatusMessage = statusMessage;
            _user = _userManager.GetUserAsync(User).Result;

            if (!String.IsNullOrEmpty(searchEvent))
            {
                CurrentEventSearch = searchEvent;
                Event = await _context.Event.Include(e => e.Venue).ThenInclude(e => e.User).Where(e => e.Venue.User.Id == _user.Id && e.EndTime > TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, _user.TimeZone).ToUniversalTime() && e.EventName.Contains(searchEvent)).OrderBy(e => e.EventName).ToListAsync();
            }
            else
            {
                Event = await _context.Event.Include(e => e.Venue).ThenInclude(e => e.User).Where(e => e.Venue.User.Id == _user.Id && e.EndTime > TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, _user.TimeZone).ToUniversalTime()).OrderBy(e => e.EventName).ToListAsync();
            }
        }
    }
}
