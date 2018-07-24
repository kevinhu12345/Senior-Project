using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Tracking_Events.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(65, ErrorMessage = "Max length is 65 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [MaxLength(65, ErrorMessage = "Max length is 65 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int AccountType { get; set; }

        [MaxLength(50)]
        [Display(Name = "Time Zone")]
        public string TimeZone { get; set; }

        //Navigation Properties
        public List<Venue> Venues { get; set; }
        public List<RSVP> Rsvps { get; set; }
        public List<Review> Reviews { get; set; }
    }

    public class Venue
    {
        [Key]
        public int VenueID { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Max length is 150 characters")]
        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Max length is 150 characters")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Max length is 150 characters")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [MaxLength(2, ErrorMessage = "Max length is 2 characters")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public int Zip { get; set; }

        [Required]
        [Display(Name = "Max Capacity")]
        [Range(5, int.MaxValue, ErrorMessage = "Must be minimum of 5")]
        public int Capacity { get; set; }
        
        [Display(Name = "Venue Type")]
        public int VenueType { get; set; }

        public ApplicationUser User { get; set; }

        public List<Request> Requests { get; set; }

        public List<Event> Events { get; set; }

        public List<Review> Reviews { get; set; }
    }

    public class Request
    {
        [Key]
        public int RequestID { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Max length is 150 characters")]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Max length is 150 characters")]
        [Display(Name = "Genre(s)")]
        public string Genre { get; set; }

        [Required]
        [Range(0, 21, ErrorMessage = "Age Requirement has to be between 0 and 21")]
        [Display(Name = "Age Req.")]
        public int AgeRequirement { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        [Services.DateGreaterThan("StartTime", ErrorMessage = "End Time has to be later than Start Time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        [Range(5, int.MaxValue, ErrorMessage = "Must be minimum of 5")]
        public int Capacity { get; set; }

        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [MaxLength(30)]
        [Display(Name = "Approval Status")]
        public string Status { get; set; } = "Waiting Approval";

        //Used to reference Venue table where the account is stored
        public Venue Venue { get; set; }
    }

    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Max length is 150 characters")]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Max length is 150 characters")]
        [Display(Name = "Genre(s)")]
        public string Genre { get; set; }

        [Required]
        [Range(0, 21, ErrorMessage = "Age Requirement has to be between 0 and 21")]
        [Display(Name = "Age Requirement")]
        public int AgeRequirement { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        [Services.DateGreaterThan("StartTime", ErrorMessage = "End Time has to be later than Start Time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        [Range(5, int.MaxValue, ErrorMessage = "Must be minimum of 5")]
        public int Capacity { get; set; }

        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        //Used to reference and navigate Venue table where the account is stored
        public Venue Venue { get; set; }

        //Navigation property for RSVPs
        public List<RSVP> Rsvps { get; set; }

        public List<Review> Reviews { get; set; }
    }

    public class Review
    {
        public int EventID { get; set; }
        public int VenueID { get; set; }
        public string UserID { get; set; }

        //Used to reference Other tables
        public Venue Venue { get; set; }
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Rating out of 5")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        [Display(Name = "Feedback")]
        public string Description { get; set; }
    }

    public class RSVP
    {
        [Key]
        [Display(Name = "Unique RSVP ID")]
        public int RsvpID { get; set; }

        public ApplicationUser User { get; set; }
        public Event Event { get; set; }

        [Display(Name = "RSVP Count")]
        [Range(0, int.MaxValue, ErrorMessage = "Cannot be less tha 0")]
        public int RSVPCount { get; set; }
    }
}
