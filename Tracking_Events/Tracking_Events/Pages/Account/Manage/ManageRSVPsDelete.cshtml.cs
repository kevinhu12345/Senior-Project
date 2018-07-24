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
    public class ManageRSVPsDeleteModel : PageModel
    {
        private readonly Tracking_Events.Data.ApplicationDbContext _context;

        public ManageRSVPsDeleteModel(Tracking_Events.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RSVP RSVP { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RSVP = await _context.RSVP.Include(r => r.Event).SingleOrDefaultAsync(m => m.RsvpID == id);

            if (RSVP == null)
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

            RSVP = await _context.RSVP.FindAsync(id);

            if (RSVP != null)
            {
                _context.RSVP.Remove(RSVP);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ManageRSVPs", new { statusMessage = "Successfully unRSVPed" });
        }
    }
}
