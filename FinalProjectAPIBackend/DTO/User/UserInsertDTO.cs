using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinalProjectAPIBackend.Models;

namespace FinalProjectAPIBackend.DTO.User
{
    public class UserInsertDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters.")]
        public string? Username { get; set; }

        [StringLength(100, ErrorMessage = "The email should not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "The password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,}$",
        ErrorMessage = "The password must contain at least one uppercase letter, " +
            "one lowercase letter, one digit, and one special character.")]
        public string? Password { get; set; }

        [StringLength(50, ErrorMessage = "First name should not exceed 50 characters.")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name should not exceed 50 characters.")]
        public string? LastName { get; set; }

        [StringLength(15, ErrorMessage = "Phone number should not exceed 15 characters.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }

        //public UserRole? UserRole { get; set; }
    }
}
