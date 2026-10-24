using System.ComponentModel.DataAnnotations;

namespace EventFlow.Models
{
    public class Rsvp
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string GuestName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string GuestEmail { get; set; } = null!;

        public DateTime RegisteredOn { get; set; }

        public Guid EventId { get; set; }

        public Event? Event { get; set; }
    }
}