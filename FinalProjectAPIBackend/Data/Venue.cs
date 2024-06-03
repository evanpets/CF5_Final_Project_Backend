namespace FinalProjectAPIBackend.Data
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string? Name {  get; set; }
        public virtual VenueAddress? VenueAddress { get; set; }
        public virtual ICollection<Event>? Events { get; set; } = new List<Event>(); 

    }
}
