using System.ComponentModel.DataAnnotations;

namespace EventFlow.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        [Range(1, 500)]
        public int MaxAttendees { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public Guid VenueId { get; set; }

        public Venue Venue { get; set; } = null!;

        public ICollection<Rsvp> Rsvps { get; set; }
            = new List<Rsvp>();
    }
}