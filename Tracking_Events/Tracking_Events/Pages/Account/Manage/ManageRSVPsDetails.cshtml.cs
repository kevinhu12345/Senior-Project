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
    public class ManageRSVPsDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageRSVPsDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public RSVP RSVP { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RSVP = await _context.RSVP.Include(r => r.Event).ThenInclude(e => e.Venue).SingleOrDefaultAsync(m => m.RsvpID == id);

            if (RSVP == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
