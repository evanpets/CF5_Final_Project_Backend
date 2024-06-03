using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.Data
{
    public class Event
    {
        public int EventId {  get; set; }
        public string? Title {  get; set; }
        public string? Description {  get; set; }
        public int? VenueId { get; set; }
        public virtual Venue? Venue { get; set; }
        public decimal? Price { get; set; }
        public virtual ICollection<Performer>? Performers { get; set; } //= new HashSet<Performer>();
        public DateOnly? Date { get; set; }
        public EventCategory? Category { get; set; }
    }
}
