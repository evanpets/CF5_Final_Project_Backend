using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.Data
{
    public class Performer
    {
        public int PerformerId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    };
}
