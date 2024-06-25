using FinalProjectAPIBackend.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinalProjectAPIBackend.Data
{

    public class Event
    {

        [Key]
        public int EventId {  get; set; }

        [Required]
        public string? Title {  get; set; }

        public string? Description {  get; set; }

        [ForeignKey("Venue")]
        public int? VenueId { get; set; }

        public virtual Venue? Venue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public DateOnly? Date { get; set; }

        [Required]
        public EventCategory? Category { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public virtual User? User { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<Performer>? Performers { get; set; }

        public ICollection<EventSave>? EventSaves { get; set; }
    }
}
