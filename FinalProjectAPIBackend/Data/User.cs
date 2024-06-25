using FinalProjectAPIBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.Data
{
    public class User
    {
        [Key]
        public int UserId {  get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber {  get; set; }
        public UserRole Role { get; set; }
        public virtual ICollection<Event>? Events { get; set; }
        public ICollection<EventSave>? EventSaves { get; set; }

    }
}
