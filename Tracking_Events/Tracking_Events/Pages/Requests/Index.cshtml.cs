using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Requests
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Request> Requests { get; set; }

        public async Task OnGetAsync()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user.AccountType == 0)
            {
                var requests = _context.Request.Include(r => r.Venue).Where(r => r.Status.Equals("Waiting Approval")).AsQueryable();
                Requests = await requests.AsNoTracking().ToListAsync();
            }
            else if (user.AccountType == 1)
            {
                var requests = _context.Request.Include(r => r.Venue).ThenInclude(v => v.User).Where(r => r.Venue.User.Id == user.Id).AsQueryable();
                Requests = await requests.AsNoTracking().ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync(int id, string judgement)
        {
            var request = _context.Request.Include(r => r.Venue).SingleOrDefault(r => r.RequestID == id);

            if (judgement.Equals("Approve"))
            {
                Event Event = new Event
                {
                    Venue = request.Venue,
                    EventName = request.EventName,
                    Genre = request.Genre,
                    AgeRequirement = request.AgeRequirement,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Capacity = request.Capacity,
                    Description = request.Description
                };

                request.Status = "Approved";

                await _context.Event.AddAsync(Event);
                _context.Update(request);

                await _context.SaveChangesAsync();
            }
            else if (judgement.Equals("Reject"))
            {
                request.Status = "Rejected";

                _context.Update(request);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
