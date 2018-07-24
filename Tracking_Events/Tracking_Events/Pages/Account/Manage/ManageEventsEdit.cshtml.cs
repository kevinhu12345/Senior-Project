using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace Tracking_Events.Pages.Events
{
    public class ManageEventsEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private string zone;
        //Used for Capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public ManageEventsEditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Event Event { get; set; }

        public IActionResult OnGet(int id)
        {
            zone = _userManager.GetUserAsync(User).Result.TimeZone;
            //Used to get Parent and Foreign tables
            Event = _context.Event.Include(ev => ev.Venue).Where(e => e.EventID == id).SingleOrDefault();
            Event.StartTime = TimeZoneInfo.ConvertTimeFromUtc(Event.StartTime, TimeZoneInfo.FindSystemTimeZoneById(zone));
            Event.EndTime = TimeZoneInfo.ConvertTimeFromUtc(Event.EndTime, TimeZoneInfo.FindSystemTimeZoneById(zone));

            if (Event == null)
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

            Event.EventName = capitalize.ToTitleCase(Event.EventName);
            Event.Genre = capitalize.ToTitleCase(Event.Genre);
            Event.StartTime = Event.StartTime.ToUniversalTime();
            Event.EndTime = Event.EndTime.ToUniversalTime();

            _context.Update(Event);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(Event.EventID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ManageEvents", new { statusMessage = "Successfully edited Event: " + Event.EventName });
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }
    }
}
