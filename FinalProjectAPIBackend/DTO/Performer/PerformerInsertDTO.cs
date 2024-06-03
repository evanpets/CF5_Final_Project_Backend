using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Performer
{
    public class PerformerInsertDTO
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Performer's name should consist of at least 5 characters.")]
        [Required(ErrorMessage = "Performer's name must be included.")]
        public string? Name { get; set; }
    }
}
