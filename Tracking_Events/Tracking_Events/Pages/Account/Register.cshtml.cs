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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Tracking_Events.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        //Used for capitalization
        private readonly TextInfo capitalize = CultureInfo.CurrentCulture.TextInfo;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        [Display(Name = "Register a location")]
        public bool Register { get; set; }

        public IEnumerable<SelectListItem> TZ { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(65, ErrorMessage = "Must be no more than 65 characters")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(65, ErrorMessage = "Must be no more than 65 characters")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [MaxLength(50)]
            [Display(Name = "Time Zone")]
            public string TimeZone { get; set; }

            [Required]
            [Display(Name = "Account Type")]
            public int AccountType { get; set; } = 1;
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            TZ = TimeZoneInfo.GetSystemTimeZones().Select(t => new SelectListItem { Value = t.Id, Text = t.DisplayName });
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { FirstName = capitalize.ToTitleCase(Input.FirstName), LastName = capitalize.ToTitleCase(Input.LastName), UserName = Input.Email, Email = Input.Email, AccountType = Input.AccountType, TimeZone = Input.TimeZone };
                
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (Register)
                    {
                        return RedirectToPage("./RegisterVenue");
                    }

                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
