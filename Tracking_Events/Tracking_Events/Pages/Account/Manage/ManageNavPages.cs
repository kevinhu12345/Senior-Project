using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tracking_Events.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";

        public static string ExternalLogins => "ExternalLogins";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string ManageVenues => "ManageVenues";

        public static string ManageRSVPs => "ManageRSVPs";

        public static string ManageEvents => "ManageEvents";

        public static string ManageReviews => "ManageReviews";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        public static string ManageVenuesClass(ViewContext viewContext) => PageNavClass(viewContext, ManageVenues);

        public static string ManageRSVPsClass(ViewContext viewContext) => PageNavClass(viewContext, ManageRSVPs);

        public static string ManageEventsClass(ViewContext viewContext) => PageNavClass(viewContext, ManageEvents);

        public static string ManageReviewsClass(ViewContext viewContext) => PageNavClass(viewContext, ManageReviews);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
