using FinalProjectAPIBackend.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.Data
{
    public class Performer
    {
        [Key]
        public int PerformerId { get; set; }
        [Required]
        public string? Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    };
}
