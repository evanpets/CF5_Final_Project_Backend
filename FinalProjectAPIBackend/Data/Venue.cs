using Newtonsoft.Json;

namespace FinalProjectAPIBackend.Data
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string? Name {  get; set; }
        public int VenueAddressId { get; set; }
        //[JsonIgnore]
        public virtual VenueAddress? VenueAddress { get; set; }
        [JsonIgnore]
        public virtual ICollection<Event>? Events { get; set; } = new List<Event>(); 

    }
}
