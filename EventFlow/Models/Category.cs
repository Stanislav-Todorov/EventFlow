using System.ComponentModel.DataAnnotations;

namespace EventFlow.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}