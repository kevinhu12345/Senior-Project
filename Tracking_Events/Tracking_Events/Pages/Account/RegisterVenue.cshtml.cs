using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Tracking_Events.Data;
using Tracking_Events.Services;
using System.Globalization;

namespace Tracking_Events.Pages.Account
{
    public enum VenueType
    {
        Official = 1, Personal
    }

    public class RegisterVenueModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        //Used for capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public RegisterVenueModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<LoginModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [Display(Name = "State")]
            public string State { get; set; }

            [Required]
            [Display(Name = "Zip")]
            public int Zip { get; set; }

            [Required]
            [Display(Name = "Max Capacity")]
            [Range(5, int.MaxValue, ErrorMessage = "Must be minimum of 5")]
            public int Capacity { get; set; }

            [Required]
            [Display(Name = "Venue Name")]
            public string VenueName { get; set; }

            [Display(Name = "Venue Type")]
            public VenueType VenueType { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                Venue venue = new Venue { Address = capitalize.ToTitleCase(Input.Address), City = capitalize.ToTitleCase(Input.City), State = Input.State.ToUpper(), Zip = Input.Zip, VenueName = capitalize.ToTitleCase(Input.VenueName), Capacity = Input.Capacity, VenueType = Convert.ToInt32(Input.VenueType) };

                venue.User = _userManager.GetUserAsync(User).Result;

                //Add Venue to DB
                await _context.Venue.AddAsync(venue);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Venue created with user.");

                return RedirectToPage("./Manage/ManageVenues", new { statusMessage = "Successfully registered a location" });
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}