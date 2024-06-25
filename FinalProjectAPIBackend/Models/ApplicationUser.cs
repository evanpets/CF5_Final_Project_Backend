namespace FinalProjectAPIBackend.Models
{
    /// <summary>
    /// A model reflecting the current app user.
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>
        /// The current user's ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The current user's username
        /// </summary>
        public string? Username { get; set; } = string.Empty;
        /// <summary>
        /// The current user's email.
        /// </summary>
        public string? Email { get; set; } = string.Empty;
    }
}
