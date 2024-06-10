using FinalProjectAPIBackend.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinalProjectAPIBackend.Data
{
    public class Event
    {
        public int EventId {  get; set; }
        public string? Title {  get; set; }
        public string? Description {  get; set; }
        public int? VenueId { get; set; }
        //[JsonIgnore]
        public virtual Venue? Venue { get; set; }
        public decimal? Price { get; set; }
        //[JsonIgnore]
        public virtual ICollection<Performer>? Performers { get; set; }
        public DateOnly? Date { get; set; }
        public EventCategory? Category { get; set; }
        public int UserId { get; set; }
        //[ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
