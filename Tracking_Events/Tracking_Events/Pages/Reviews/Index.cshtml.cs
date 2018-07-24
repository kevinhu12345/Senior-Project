using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tracking_Events.Data;

namespace Tracking_Events.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Review> Reviews { get;set; }

        public string CurrentSort { get; set; }
        public string CurrentSearch { get; set; }

        #region Sorting Purposes
        public string VenueSort { get; set; }
        public string RatingSort { get; set; }
        #endregion

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            VenueSort = String.IsNullOrEmpty(sortOrder) ? "venue_name_desc" : "";
            RatingSort = sortOrder == "Rating" ? "rating_desc" : "Rating";
            CurrentSort = sortOrder;

            IQueryable<Review> reviews = _context.Review.Include(r => r.Venue).Include(r => r.User).Include(r => r.Event).AsQueryable();

            #region Filtering
            CurrentSearch = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                reviews = reviews.Where(s => s.Venue.VenueName.Contains(searchString) || s.Rating.ToString().Equals(searchString));
            }
            #endregion

            switch (sortOrder)
            {
                case "venue_name_desc":
                    reviews = reviews.OrderByDescending(s => s.Venue.VenueName);
                    break;
                case "Rating":
                    reviews = reviews.OrderBy(s => s.Rating);
                    break;
                case "rating_desc":
                    reviews = reviews.OrderByDescending(s => s.Rating);
                    break;
                default:
                    reviews = reviews.OrderBy(s => s.Venue.VenueName);
                    break;
            }

            Reviews = await reviews.AsNoTracking().ToListAsync();
        }
    }
}
