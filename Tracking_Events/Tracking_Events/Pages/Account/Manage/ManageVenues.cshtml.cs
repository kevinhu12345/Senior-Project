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
    public class ManageVenuesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageVenuesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Venue> Venues { get;set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string Search { get; set; }

        public async Task OnGetAsync(string statusMessage, string search)
        {
            StatusMessage = statusMessage;

            var users = _context.Venue.Include(v => v.User).Where(v => v.User.Id == _userManager.GetUserAsync(User).Result.Id);

            //Filter by search
            if (!String.IsNullOrEmpty(search))
            {
                Search = search;
                users = users.Where(v => v.VenueName.Contains(search));
            }

            Venues = await users.OrderBy(v => v.VenueName).ToListAsync();
        }
    }
}
