using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.Data
{
    public class User
    {
        public int UserId {  get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber {  get; set; }
        public UserRole Role { get; set; }
        public virtual ICollection<Event>? Events { get; set; }

    }
}
