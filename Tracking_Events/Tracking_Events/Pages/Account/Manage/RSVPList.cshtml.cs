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
    public class RSVPListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RSVPListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<RSVP> RSVP { get;set; }

        public async Task OnGetAsync(int id)
        {
            RSVP = await _context.RSVP.Include(r => r.Event).Include(r => r.User).Where(r => r.Event.EventID == id).OrderBy(r => r.RsvpID).ToListAsync();
        }
    }
}
