using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectAPIBackend.Data
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }
        [Required]
        public string? Name {  get; set; }
        [ForeignKey("VenueAddress")]
        public int VenueAddressId { get; set; }
        public virtual VenueAddress? VenueAddress { get; set; }
        [JsonIgnore]
        public virtual ICollection<Event>? Events { get; set; } = new List<Event>(); 

    }
}
