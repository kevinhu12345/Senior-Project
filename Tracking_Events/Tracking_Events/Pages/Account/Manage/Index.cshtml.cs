using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Events.Data;
using Tracking_Events.Services;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tracking_Events.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        //Used for Capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IEnumerable<SelectListItem> TZ { get; set; }

        public class InputModel
        {
            [StringLength(65, ErrorMessage = "Must be no more than 65 characters")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            
            [Display(Name = "Last Name")]
            [StringLength(65, ErrorMessage = "Must be no more than 65 characters")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [MaxLength(50)]
            [Display(Name = "Time Zone")]
            public string TimeZone { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string statusMessage)
        {
            var user = await _userManager.GetUserAsync(User);
            StatusMessage = statusMessage;

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TimeZone = user.TimeZone
            };

            TZ = TimeZoneInfo.GetSystemTimeZones().Select(t => new SelectListItem { Value = t.Id, Text = t.DisplayName });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            if (Input.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                var setUserResult = await _userManager.SetUserNameAsync(user, Input.Email);
                if (!setEmailResult.Succeeded || !setUserResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email/username for user with ID '{user.Id}'.");
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.TimeZone != user.TimeZone)
            {
                user.TimeZone = Input.TimeZone;
            }

            await _userManager.UpdateAsync(user);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage(new { statusMessage = "Successfully changed settings" });
        }

        public IActionResult OnPostDelete()
        {            
            return RedirectToPage("./DeleteConfirm");
        }
    }
}
