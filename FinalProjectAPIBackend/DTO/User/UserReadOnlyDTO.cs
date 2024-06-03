using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.DTO.User
{
    public class UserReadOnlyDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        //public UserRole? UserRole { get; set; }
    }
}
