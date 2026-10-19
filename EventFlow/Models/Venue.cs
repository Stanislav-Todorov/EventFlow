using System.ComponentModel.DataAnnotations;

namespace EventFlow.Models
{
    public class Venue
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(60)]
        public string City { get; set; } = null!;

        public ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}