using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Account.Manage
{
    public class ManageRSVPsEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageRSVPsEditModel(ApplicationDbContext context)
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

            RSVP = await _context.RSVP.Include(r => r.Event).Include(r => r.User).SingleOrDefaultAsync(m => m.RsvpID == id);

            if (RSVP == null)
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

            _context.Attach(RSVP).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RSVPExists(RSVP.RsvpID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ManageRSVPs");
        }

        private bool RSVPExists(int id)
        {
            return _context.RSVP.Any(e => e.RsvpID == id);
        }
    }
}
