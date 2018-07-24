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
    public class ManageEventsDeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageEventsDeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _context.Event.SingleOrDefaultAsync(m => m.EventID == id);

            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _context.Event.Include(e => e.Venue).SingleOrDefaultAsync(e => e.EventID == id);
            var Request = await _context.Request.SingleOrDefaultAsync(r => r.Venue == Event.Venue);

            if (Event != null)
            {
                _context.Event.Remove(Event);
                _context.Request.Remove(Request);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ManageEvents", new { statusMessage = "Successfully deleted Event: " + Event.EventName });
        }
    }
}
