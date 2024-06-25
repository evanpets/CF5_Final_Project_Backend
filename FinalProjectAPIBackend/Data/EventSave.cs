using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectAPIBackend.Data
{
    public class EventSave
    {
        [Key]
        public int SaveId { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("Event")]
        public int? EventId { get; set; }
        public Event? Event { get; set; }
    }
}
