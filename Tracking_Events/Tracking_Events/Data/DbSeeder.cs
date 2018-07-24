using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking_Events.Data
{
    public class DbSeeder
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DbSeeder(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SeedAdmin()
        {
            try
            {
                _context.Database.Migrate();
            }
            catch
            {

            }

            if (_userManager.FindByEmailAsync("admin@admin.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    TimeZone = "Eastern Standard Time",
                    AccountType = 0
                };

                var result = await _userManager.CreateAsync(user, "admin12345");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }
    }
}
