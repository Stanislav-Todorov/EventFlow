using System.ComponentModel.DataAnnotations;

namespace EventFlow.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Display(Name = "Start Date & Time")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "End Date & Time")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Maximum Attendees")]
        [Range(1, 500)]
        public int MaxAttendees { get; set; }

        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }

        public Guid VenueId { get; set; }

        public Venue? Venue { get; set; }

        public ICollection<Rsvp> Rsvps { get; set; }
            = new List<Rsvp>();
    }
}